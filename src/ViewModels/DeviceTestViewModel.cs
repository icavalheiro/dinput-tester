using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using DInputTester.Models;
using DInputTester.Services;
using Vortice.DirectInput;

namespace DInputTester.ViewModels;

public partial class DeviceTestViewModel : ViewModelBase, IDisposable
{
    private static readonly string[] AxisNames = ["X", "Y", "Z", "RX", "RY", "RZ", "Slider1", "Slider2"];

    private readonly MainWindowViewModel _mainVm;
    private readonly DirectInputService _inputService;
    private readonly IDirectInputDevice8? _device;
    private readonly DispatcherTimer _timer;
    private bool _disposed;

    public GameDeviceInfo DeviceInfo { get; }
    public ObservableCollection<ButtonStateModel> Buttons { get; } = new();
    public ObservableCollection<AxisStateModel> Axes { get; } = new();

    public string StatusMessage { get; private set; } = string.Empty;

    public DeviceTestViewModel(MainWindowViewModel mainVm, GameDeviceInfo deviceInfo)
    {
        _mainVm = mainVm;
        DeviceInfo = deviceInfo;
        _inputService = new DirectInputService();

        // Initialize axes
        foreach (var name in AxisNames)
            Axes.Add(new AxisStateModel(name));

        _device = _inputService.AcquireDevice(deviceInfo.InstanceGuid);

        if (_device is null)
        {
            StatusMessage = "Failed to acquire the device.";
        }
        else
        {
            StatusMessage = string.Empty;
            // Initial poll to discover available buttons.
            var initial = _inputService.PollState(_device);
            int buttonCount = 128; // DirectInput joystick state supports up to 128 buttons.
            if (initial.HasValue)
            {
                buttonCount = Math.Max(1, initial.Value.Buttons.Length);
                UpdateState(initial.Value.Buttons, initial.Value.Axes);
            }

            for (int i = 0; i < buttonCount; i++)
                Buttons.Add(new ButtonStateModel(i));
        }

        _timer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(16) }; // ~60fps
        _timer.Tick += OnTick;
        _timer.Start();
    }

    private void OnTick(object? sender, EventArgs e)
    {
        if (_device is null) return;

        var result = _inputService.PollState(_device);
        if (result is null) return;

        UpdateState(result.Value.Buttons, result.Value.Axes);
    }

    private void UpdateState(bool[] buttons, Dictionary<string, float> axes)
    {
        // Update buttons (expand list if device reports more buttons)
        for (int i = 0; i < buttons.Length && i < 128; i++)
        {
            if (i < Buttons.Count)
                Buttons[i].IsPressed = buttons[i];
        }

        // Update axes
        foreach (var axis in Axes)
        {
            if (axes.TryGetValue(axis.Name, out var val))
                axis.Value = val;
        }
    }

    [RelayCommand]
    private void GoBack() => _mainVm.NavigateToList();

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _timer.Stop();
        _device?.Unacquire();
        _device?.Dispose();
        _inputService.Dispose();
    }
}
