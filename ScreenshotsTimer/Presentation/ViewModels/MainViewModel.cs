using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ScreenshotsTimer.Data;
using ScreenshotsTimer.Domain;

namespace ScreenshotsTimer.Presentation.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly Storage _storage = new();
    private readonly WorkTimer _workTimer = new();
    private Timer _timer;

    [ObservableProperty] private string? _folderPath;

    [ObservableProperty] private bool _isRunning;

    [ObservableProperty] private bool _isPaused;

    [ObservableProperty] private bool _isLoading;

    public bool CanReset => IsRunning || IsPaused;

    public string TimerText => _workTimer.WorkTime.ToString(@"hh\:mm\:ss");

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
        ToggleStartStopCommand.NotifyCanExecuteChanged();
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

    private bool CanTogglePlayPause() => !string.IsNullOrEmpty(FolderPath);


    [RelayCommand(CanExecute = nameof(CanTogglePlayPause))]
    private void ToggleStartStop()
    {
        if (IsRunning)
        {
            if (IsPaused)
                Continue();
            else
                Pause();
        }
        else
        {
            Start();
        }
    }

    private void Start()
    {
        _workTimer.Start(TimeSpan.FromMinutes(1), FolderPath!);

        CreateTimer();

        IsRunning = true;
    }

    private void Pause()
    {
        _workTimer.Pause();
        _timer.Dispose();

        IsPaused = true;
    }

    private void Continue()
    {
        _workTimer.Resume();

        CreateTimer();

        IsPaused = false;
    }

    [RelayCommand]
    private void Reset()
    {
        _workTimer.Reset();
        _timer.Dispose();

        IsRunning = false;
        IsPaused = false;
    }

    private void CreateTimer()
    {
        _timer = new Timer(_ => { Dispatcher.UIThread.Invoke(() => OnPropertyChanged(nameof(TimerText))); }, null,
            TimeSpan.Zero, TimeSpan.FromMilliseconds(500));
    }

    partial void OnIsRunningChanged(bool value)
    {
        GetScreenshotCommand.NotifyCanExecuteChanged();

        OnPropertyChanged(nameof(CanReset));
        OnPropertyChanged(nameof(TimerText));
    }

    partial void OnIsPausedChanged(bool value)
    {
        OnPropertyChanged(nameof(CanReset));
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

        await _workTimer.GetScreenshot(FolderPath!);

        IsLoading = false;
    }
}