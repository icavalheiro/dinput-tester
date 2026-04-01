using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace DInputTester.Models;

public partial class AxisStateModel : ObservableObject
{
    public string Name { get; }

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(ValueText))]
    [NotifyPropertyChangedFor(nameof(ProgressValue))]
    [NotifyPropertyChangedFor(nameof(BarBrush))]
    private float _value;

    // Float value in range [-1.0, 1.0]
    public string ValueText => Value.ToString("+0.000;-0.000; 0.000");

    // Maps [-1, 1] → [0, 100] so center (0) = 50
    public double ProgressValue => Math.Clamp((Value + 1.0) / 2.0 * 100.0, 0.0, 100.0);

    public bool IsActive => MathF.Abs(Value) > 0.02f;

    public IBrush BarBrush => IsActive
        ? new SolidColorBrush(Color.FromRgb(0xFF, 0x8C, 0x00))   // DarkOrange
        : new SolidColorBrush(Color.FromRgb(0x26, 0x7A, 0xDB));  // Blue

    partial void OnValueChanged(float value)
    {
        OnPropertyChanged(nameof(IsActive));
    }

    public AxisStateModel(string name)
    {
        Name = name;
    }
}
