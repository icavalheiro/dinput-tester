using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;

namespace DInputTester.Models;

public partial class ButtonStateModel : ObservableObject
{
    public int Index { get; }
    public string Label => $"B{Index + 1}";

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(Background))]
    private bool _isPressed;

    public IBrush Background => IsPressed
        ? new SolidColorBrush(Color.FromRgb(0xFF, 0x8C, 0x00))   // DarkOrange
        : new SolidColorBrush(Color.FromRgb(0x3A, 0x3A, 0x3A));  // dark grey

    public ButtonStateModel(int index)
    {
        Index = index;
    }
}
