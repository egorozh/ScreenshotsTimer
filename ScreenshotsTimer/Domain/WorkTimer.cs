using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace ScreenshotsTimer.Domain;

public class WorkTimer
{
    private readonly FastScreenCapture _fastScreenCapture = new();

    private DateTime _startWorkTime= DateTime.Now;
    private DateTime _pauseWorkTime;
    private string _folderPath;
    private TimeSpan _pauseTimeCounter;
    private TimeSpan _screenshotsPeriod;

    private Timer _timer;

    public TimeSpan WorkTime => DateTime.Now - _startWorkTime - _pauseTimeCounter;

    public void Start(TimeSpan screenshotsPeriod, string folderPath)
    {
        _startWorkTime = DateTime.Now;
        _pauseTimeCounter = TimeSpan.Zero;
        _screenshotsPeriod = screenshotsPeriod;
        
        _folderPath = folderPath;

        _timer = new Timer(_onTick, null, TimeSpan.Zero, _screenshotsPeriod);
    }

    public void Reset()
    {
        _timer.Dispose();
        
        _startWorkTime = DateTime.Now;
        _pauseTimeCounter = TimeSpan.Zero;
    }

    public void Pause()
    {
        _timer.Dispose();

        _pauseWorkTime = DateTime.Now;
    }

    public void Resume()
    {
        _pauseTimeCounter += DateTime.Now - _pauseWorkTime;
        
        _timer = new Timer(_onTick, null, TimeSpan.Zero, _screenshotsPeriod);
    }

    public async Task GetScreenshot(string folderPath)
    {
        string nowFolder = $"{DateTime.Now:dd_MM_yyyy}";

        string targetFolder = Path.Combine(folderPath, nowFolder);

        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        await _fastScreenCapture.CaptureJpegAsync(Path.Combine(targetFolder, $"{DateTime.Now:HH_mm_ss_fff}.jpg"));
    }

    private async void _onTick(object? state) => await GetScreenshot(_folderPath);
}