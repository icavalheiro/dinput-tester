using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using DInputTester.Models;
using DInputTester.Services;

namespace DInputTester.ViewModels;

public partial class DeviceListViewModel : ViewModelBase
{
    private readonly MainWindowViewModel _mainVm;
    private readonly DirectInputService _inputService = new();

    public ObservableCollection<GameDeviceInfo> Devices { get; } = new();

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(TestDeviceCommand))]
    private GameDeviceInfo? _selectedDevice;

    [ObservableProperty]
    private string _statusMessage = "Select a device to test.";

    public DeviceListViewModel(MainWindowViewModel mainVm)
    {
        _mainVm = mainVm;
        RefreshDevices();
    }

    public void RefreshDevices()
    {
        Devices.Clear();
        var found = _inputService.EnumerateDevices();
        foreach (var d in found)
            Devices.Add(d);

        StatusMessage = found.Count == 0
            ? "No devices found. Connect a joystick/wheel and click Refresh."
            : $"{found.Count} device(s) found.";
    }

    [RelayCommand]
    private void Refresh() => RefreshDevices();

    [RelayCommand(CanExecute = nameof(CanTestDevice))]
    private void TestDevice()
    {
        if (SelectedDevice is not null)
            _mainVm.NavigateToTest(SelectedDevice);
    }

    private bool CanTestDevice() => SelectedDevice is not null;
}
