using Avalonia;
using Avalonia.Controls;
using Avalonia.Data;
using Avalonia.Media;
using Avalonia.Platform;

namespace DInputTester.Views;

public class MainWindow : Window
{
    public MainWindow()
    {
        Title = "DInput Tester";
        Icon = new WindowIcon(AssetLoader.Open(new Uri("avares://DInputTester/joystick.ico")));
        Width = 900;
        Height = 660;
        MinWidth = 700;
        MinHeight = 500;
        Background = new SolidColorBrush(Color.Parse("#1E1E1E"));
        WindowStartupLocation = WindowStartupLocation.CenterScreen;

        var contentHost = new ContentControl();
        contentHost.Bind(ContentControl.ContentProperty, new Binding("CurrentViewModel"));

        Content = contentHost;
    }
}
