using CommunityToolkit.Mvvm.ComponentModel;
using DInputTester.Models;

namespace DInputTester.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentViewModel;

    private readonly DeviceListViewModel _deviceListViewModel;

    public MainWindowViewModel()
    {
        _deviceListViewModel = new DeviceListViewModel(this);
        _currentViewModel = _deviceListViewModel;
    }

    public void NavigateToTest(GameDeviceInfo device)
    {
        CurrentViewModel = new DeviceTestViewModel(this, device);
    }

    public void NavigateToList()
    {
        // Dispose the test VM (stops polling) before navigating back
        if (CurrentViewModel is DeviceTestViewModel testVm)
            testVm.Dispose();

        _deviceListViewModel.RefreshDevices();
        CurrentViewModel = _deviceListViewModel;
    }
}
