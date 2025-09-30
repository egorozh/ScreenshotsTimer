using System;
using System.Linq;
using System.Threading.Tasks;
using ScreenCapture.NET;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;

namespace ScreenshotsTimer;

public sealed class FastScreenCapture : IAsyncDisposable
{
    private readonly DX11ScreenCaptureService _svc;
    private readonly IScreenCapture _capture;
    private readonly ICaptureZone _zone;

    public FastScreenCapture()
    {
        _svc = new DX11ScreenCaptureService();

        var gpu = _svc.GetGraphicsCards().First();
        var display = _svc.GetDisplays(gpu).First();
        _capture = _svc.GetScreenCapture(display);

        _zone = _capture.RegisterCaptureZone(0, 0, display.Width, display.Height);
    }

    /// <summary>
    /// Быстрый скриншот экрана в JPEG
    /// </summary>
    /// <param name="outputPath">Путь для сохранения .jpg</param>
    /// <param name="jpegQuality">Качество JPEG (1-100)</param>
    public async Task CaptureJpegAsync(string outputPath, int jpegQuality = 80)
    {
        _capture.CaptureScreen();

        using var locked = _zone.Lock();

        var src = _zone.Image;

        using var img = Image.LoadPixelData<Bgra32>(
            configuration: Configuration.Default,
            _zone.RawBuffer,
            src.Width,
            src.Height);

        var enc = new JpegEncoder { Quality = jpegQuality };

        await img.SaveAsJpegAsync(outputPath, enc).ConfigureAwait(false);
    }

    public ValueTask DisposeAsync()
    {
        _capture.Dispose();
        _svc.Dispose();

        return ValueTask.CompletedTask;
    }
}