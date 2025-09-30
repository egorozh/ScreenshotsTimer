using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScreenshotsTimer.Data;

namespace ScreenshotsTimer.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly Storage _storage = new();

    private readonly FastScreenCapture _fastScreenCapture = new();

    [ObservableProperty] private string? _folderPath;

    [ObservableProperty] private bool _isRunning;

    [ObservableProperty] private bool _isPaused;

    [ObservableProperty] private bool _isLoading;

    private TimeSpan _time;

    private TimeSpan Time
    {
        get => _time;
        set
        {
            _time = value;

            OnPropertyChanged(nameof(TimerText));
        }
    }

    public string TimerText => _time.ToString(@"hh\:mm\:ss");

    public MainViewModel()
    {
        _folderPath = _storage.GetLastFolder();
    }

    [RelayCommand]
    private async Task SetFolder(Window window)
    {
        var result = await window.StorageProvider.OpenFolderPickerAsync(new FolderPickerOpenOptions
        {
            AllowMultiple = false,
            Title = "Папка для сохранения скриншотов"
        });

        if (result.Count > 0)
        {
            string folder = result[0].Path.AbsolutePath;

            FolderPath = folder;

            _storage.UpdateLastFolder(folder);
        }
    }

    private bool CanOpenFolderInNativeExplorer() => !string.IsNullOrEmpty(FolderPath);

    partial void OnFolderPathChanged(string? value)
    {
        StartCommand.NotifyCanExecuteChanged();
        GetScreenshotCommand.NotifyCanExecuteChanged();
        OpenFolderInNativeExplorerCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanOpenFolderInNativeExplorer))]
    private async Task OpenFolderInNativeExplorer()
    {
        try
        {
            new Process
            {
                StartInfo = new ProcessStartInfo(FolderPath)
                {
                    UseShellExecute = true
                }
            }.Start();
        }
        catch (Exception e)
        {
            //
        }
    }

    private bool CanStart() => !IsRunning && !string.IsNullOrEmpty(FolderPath);


    [RelayCommand(CanExecute = nameof(CanStart))]
    private async Task Start()
    {
        IsRunning = true;

        Time = TimeSpan.Zero;

        while (IsRunning)
        {
            if (IsPaused)
            {
                await Task.Delay(1000);
                continue;
            }

            Time += TimeSpan.FromMinutes(1);

            await GetScreenshot();

            await Task.Delay(TimeSpan.FromMinutes(1));
        }
    }

    [RelayCommand]
    private async Task Stop()
    {
        IsRunning = false;
    }

    private bool CanPause() => IsRunning && !IsPaused;

    private bool CanContinue() => IsRunning && IsPaused;

    partial void OnIsRunningChanged(bool value)
    {
        StartCommand.NotifyCanExecuteChanged();
        GetScreenshotCommand.NotifyCanExecuteChanged();
        PauseCommand.NotifyCanExecuteChanged();
        ContinueCommand.NotifyCanExecuteChanged();
    }

    partial void OnIsPausedChanged(bool value)
    {
        PauseCommand.NotifyCanExecuteChanged();
        ContinueCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanPause))]
    private async Task Pause()
    {
        IsPaused = true;
    }

    [RelayCommand(CanExecute = nameof(CanContinue))]
    private async Task Continue()
    {
        IsPaused = false;
    }

    private bool CanGetScreenshot() => !IsLoading && !string.IsNullOrEmpty(FolderPath);

    partial void OnIsLoadingChanged(bool value)
    {
        GetScreenshotCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanGetScreenshot))]
    private async Task GetScreenshot()
    {
        IsLoading = true;

        string nowFolder = $"{DateTime.Now:dd_MM_yyyy}";

        string targetFolder = Path.Combine(FolderPath, nowFolder);

        if (!Directory.Exists(targetFolder))
        {
            Directory.CreateDirectory(targetFolder);
        }

        await _fastScreenCapture.CaptureJpegAsync(Path.Combine(targetFolder, $"{DateTime.Now:HH_mm_ss_fff}.jpg"));

        IsLoading = false;
    }
}