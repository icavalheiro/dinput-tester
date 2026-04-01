using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using DInputTester.Models;

namespace DInputTester.Views;

public class DeviceTestView : UserControl
{
    public DeviceTestView()
    {
        Background = new SolidColorBrush(Color.Parse("#1E1E1E"));

        var root = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,*"),
            Margin = new Thickness(24)
        };

        var header = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto"),
            Margin = new Thickness(0, 0, 0, 20)
        };
        Grid.SetRow(header, 0);

        var backButton = new Button
        {
            Content = "<- Back",
            Padding = new Thickness(14, 8),
            Background = new SolidColorBrush(Color.Parse("#3A3A3A")),
            Foreground = new SolidColorBrush(Color.Parse("#CCCCCC"))
        };
        backButton.Bind(Button.CommandProperty, new Binding("GoBackCommand"));
        Grid.SetColumn(backButton, 0);

        var deviceInfoPanel = new StackPanel
        {
            Margin = new Thickness(20, 0, 0, 0),
            VerticalAlignment = VerticalAlignment.Center
        };
        var productName = new TextBlock
        {
            FontSize = 18,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Color.Parse("#EEEEEE"))
        };
        productName.Bind(TextBlock.TextProperty, new Binding("DeviceInfo.ProductName"));

        var instanceName = new TextBlock
        {
            FontSize = 11,
            Foreground = new SolidColorBrush(Color.Parse("#888888"))
        };
        instanceName.Bind(TextBlock.TextProperty, new Binding("DeviceInfo.InstanceName"));
        deviceInfoPanel.Children.Add(productName);
        deviceInfoPanel.Children.Add(instanceName);
        Grid.SetColumn(deviceInfoPanel, 1);

        var status = new TextBlock
        {
            FontSize = 12,
            Foreground = new SolidColorBrush(Color.Parse("#FF6B6B")),
            VerticalAlignment = VerticalAlignment.Center
        };
        status.Bind(TextBlock.TextProperty, new Binding("StatusMessage"));
        Grid.SetColumn(status, 2);

        header.Children.Add(backButton);
        header.Children.Add(deviceInfoPanel);
        header.Children.Add(status);
        root.Children.Add(header);

        var main = new Grid { ColumnDefinitions = new ColumnDefinitions("*,300") };
        Grid.SetRow(main, 1);

        var buttonsBorder = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#2A2A2A")),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(16),
            Margin = new Thickness(0, 0, 12, 0)
        };
        Grid.SetColumn(buttonsBorder, 0);

        var buttonsGrid = new Grid { RowDefinitions = new RowDefinitions("Auto,*") };
        buttonsGrid.Children.Add(new TextBlock
        {
            Text = "BUTTONS",
            FontSize = 12,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Color.Parse("#888888")),
            Margin = new Thickness(0, 0, 0, 12)
        });

        var buttonsItems = new ItemsControl();
        buttonsItems.Bind(ItemsControl.ItemsSourceProperty, new Binding("Buttons"));
        buttonsItems.ItemsPanel = new FuncTemplate<Panel?>(() => new WrapPanel { Orientation = Orientation.Horizontal });
        buttonsItems.ItemTemplate = new FuncDataTemplate<ButtonStateModel>((_, _) =>
        {
            var border = new Border
            {
                Width = 60,
                Height = 60,
                CornerRadius = new CornerRadius(6),
                Margin = new Thickness(4)
            };
            border.Bind(Border.BackgroundProperty, new Binding("Background"));

            var label = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 11,
                FontWeight = FontWeight.SemiBold,
                Foreground = new SolidColorBrush(Color.Parse("#EEEEEE"))
            };
            label.Bind(TextBlock.TextProperty, new Binding("Label"));
            border.Child = label;
            return border;
        });

        var buttonsScroll = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = buttonsItems
        };
        Grid.SetRow(buttonsScroll, 1);
        buttonsGrid.Children.Add(buttonsScroll);
        buttonsBorder.Child = buttonsGrid;

        var axesBorder = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#2A2A2A")),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(16)
        };
        Grid.SetColumn(axesBorder, 1);

        var axesGrid = new Grid { RowDefinitions = new RowDefinitions("Auto,*") };
        axesGrid.Children.Add(new TextBlock
        {
            Text = "AXES",
            FontSize = 12,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Color.Parse("#888888")),
            Margin = new Thickness(0, 0, 0, 12)
        });

        var axesItems = new ItemsControl();
        axesItems.Bind(ItemsControl.ItemsSourceProperty, new Binding("Axes"));
        axesItems.ItemTemplate = new FuncDataTemplate<AxisStateModel>((_, _) =>
        {
            var axisRow = new Grid
            {
                RowDefinitions = new RowDefinitions("Auto,Auto"),
                Margin = new Thickness(0, 0, 0, 16)
            };

            var top = new Grid { ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto") };
            var axisName = new TextBlock
            {
                FontSize = 12,
                FontWeight = FontWeight.SemiBold,
                Foreground = new SolidColorBrush(Color.Parse("#CCCCCC")),
                VerticalAlignment = VerticalAlignment.Center
            };
            axisName.Bind(TextBlock.TextProperty, new Binding("Name"));

            var axisValue = new TextBlock
            {
                FontSize = 12,
                FontFamily = FontFamily.Parse("Consolas"),
                Foreground = new SolidColorBrush(Color.Parse("#EEEEEE")),
                VerticalAlignment = VerticalAlignment.Center
            };
            axisValue.Bind(TextBlock.TextProperty, new Binding("ValueText"));
            Grid.SetColumn(axisValue, 2);

            top.Children.Add(axisName);
            top.Children.Add(axisValue);
            Grid.SetRow(top, 0);

            var barGrid = new Grid { Margin = new Thickness(0, 4, 0, 0) };
            Grid.SetRow(barGrid, 1);

            barGrid.Children.Add(new Border
            {
                Height = 10,
                CornerRadius = new CornerRadius(5),
                Background = new SolidColorBrush(Color.Parse("#3A3A3A"))
            });

            barGrid.Children.Add(new Rectangle
            {
                Width = 2,
                Height = 14,
                Fill = new SolidColorBrush(Color.Parse("#666666")),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            });

            var progress = new ProgressBar
            {
                Minimum = 0,
                Maximum = 100,
                Height = 10,
                Background = Brushes.Transparent
            };
            progress.Bind(RangeBase.ValueProperty, new Binding("ProgressValue"));
            progress.Bind(ProgressBar.ForegroundProperty, new Binding("BarBrush"));
            barGrid.Children.Add(progress);

            axisRow.Children.Add(top);
            axisRow.Children.Add(barGrid);
            return axisRow;
        });

        var axesScroll = new ScrollViewer
        {
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto,
            Content = axesItems
        };
        Grid.SetRow(axesScroll, 1);
        axesGrid.Children.Add(axesScroll);
        axesBorder.Child = axesGrid;

        main.Children.Add(buttonsBorder);
        main.Children.Add(axesBorder);
        root.Children.Add(main);

        Content = root;
    }
}
