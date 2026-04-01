using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.Media;
using DInputTester.Models;

namespace DInputTester.Views;

public class DeviceListView : UserControl
{
    public DeviceListView()
    {
        Background = new SolidColorBrush(Color.Parse("#1E1E1E"));

        var root = new Grid
        {
            RowDefinitions = new RowDefinitions("Auto,*,Auto"),
            Margin = new Thickness(32)
        };

        var header = new StackPanel
        {
            Spacing = 8,
            Margin = new Thickness(0, 0, 0, 24)
        };
        header.Children.Add(new TextBlock
        {
            Text = "DInput Tester",
            FontSize = 28,
            FontWeight = FontWeight.Bold,
            Foreground = new SolidColorBrush(Color.Parse("#EEEEEE"))
        });
        header.Children.Add(new TextBlock
        {
            Text = "Detects and tests joysticks, flight sticks, steering wheels, and other input devices.",
            FontSize = 13,
            Foreground = new SolidColorBrush(Color.Parse("#AAAAAA")),
            TextWrapping = TextWrapping.Wrap
        });
        Grid.SetRow(header, 0);
        root.Children.Add(header);

        var listContainer = new Border
        {
            Background = new SolidColorBrush(Color.Parse("#2A2A2A")),
            CornerRadius = new CornerRadius(8),
            Padding = new Thickness(16)
        };
        Grid.SetRow(listContainer, 1);

        var listGrid = new Grid { RowDefinitions = new RowDefinitions("Auto,*,Auto") };
        listGrid.Children.Add(new TextBlock
        {
            Text = "Detected devices",
            FontSize = 14,
            FontWeight = FontWeight.SemiBold,
            Foreground = new SolidColorBrush(Color.Parse("#CCCCCC")),
            Margin = new Thickness(0, 0, 0, 12)
        });

        var listBox = new ListBox
        {
            Background = Brushes.Transparent
        };
        ScrollViewer.SetHorizontalScrollBarVisibility(listBox, ScrollBarVisibility.Disabled);
        listBox.Bind(ItemsControl.ItemsSourceProperty, new Binding("Devices"));
        listBox.Bind(SelectingItemsControl.SelectedItemProperty, new Binding("SelectedDevice") { Mode = BindingMode.TwoWay });
        listBox.ItemTemplate = new FuncDataTemplate<GameDeviceInfo>((_, _) =>
        {
            var panel = new StackPanel { Spacing = 2 };

            var product = new TextBlock
            {
                FontSize = 14,
                FontWeight = FontWeight.SemiBold,
                Foreground = new SolidColorBrush(Color.Parse("#EEEEEE"))
            };
            product.Bind(TextBlock.TextProperty, new Binding("ProductName"));

            var instance = new TextBlock
            {
                FontSize = 11,
                Foreground = new SolidColorBrush(Color.Parse("#888888"))
            };
            instance.Bind(TextBlock.TextProperty, new Binding("InstanceName"));

            panel.Children.Add(product);
            panel.Children.Add(instance);
            return panel;
        });
        Grid.SetRow(listBox, 1);
        listGrid.Children.Add(listBox);

        var status = new TextBlock
        {
            FontSize = 12,
            Foreground = new SolidColorBrush(Color.Parse("#888888")),
            Margin = new Thickness(0, 12, 0, 0)
        };
        status.Bind(TextBlock.TextProperty, new Binding("StatusMessage"));
        Grid.SetRow(status, 2);
        listGrid.Children.Add(status);

        listContainer.Child = listGrid;
        root.Children.Add(listContainer);

        var footer = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            HorizontalAlignment = HorizontalAlignment.Right,
            Spacing = 12,
            Margin = new Thickness(0, 20, 0, 0)
        };
        Grid.SetRow(footer, 2);

        var refreshButton = new Button
        {
            Content = "Refresh List",
            Padding = new Thickness(16, 8),
            Background = new SolidColorBrush(Color.Parse("#3A3A3A")),
            Foreground = new SolidColorBrush(Color.Parse("#CCCCCC"))
        };
        refreshButton.Bind(Button.CommandProperty, new Binding("RefreshCommand"));

        var testButton = new Button
        {
            Content = "Test Device ->",
            Padding = new Thickness(16, 8),
            Background = new SolidColorBrush(Color.Parse("#FF8C00")),
            Foreground = Brushes.White,
            FontWeight = FontWeight.SemiBold
        };
        testButton.Bind(Button.CommandProperty, new Binding("TestDeviceCommand"));

        footer.Children.Add(refreshButton);
        footer.Children.Add(testButton);
        root.Children.Add(footer);

        Content = root;
    }
}
