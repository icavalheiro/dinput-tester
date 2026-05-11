using System;
using System.Collections.Generic;
using DInputTester.Models;
using SDL3;

namespace DInputTester.Services;

public sealed class DirectInputService : IDisposable
{
    private static readonly string[] AxisNames = ["X", "Y", "Z", "RX", "RY", "RZ", "Slider1", "Slider2"];
    private bool _disposed;

    public DirectInputService()
    {
        SDL.InitSubSystem(SDL.InitFlags.Joystick);
    }

    /// <summary>Enumerates all attached joystick devices via SDL3.</summary>
    public List<GameDeviceInfo> EnumerateDevices()
    {
        var result = new List<GameDeviceInfo>();

        var ids = SDL.GetJoysticks(out _);
        if (ids is null)
            return result;

        foreach (var instanceId in ids)
        {
            var name = SDL.GetJoystickNameForID(instanceId) ?? "Unknown";

            result.Add(new GameDeviceInfo(
                instanceId,
                name,
                $"Joystick #{instanceId}"));
        }

        return result;
    }

    /// <summary>Opens a joystick device. Caller is responsible for closing it.</summary>
    public IntPtr AcquireDevice(uint instanceId)
    {
        return SDL.OpenJoystick(instanceId);
    }

    /// <summary>
    /// Polls the device and returns buttons and axes mapped to legacy axis names.
    /// Axis values are normalized to [-1.0, 1.0].
    /// Returns null if polling fails (e.g. device disconnected).
    /// </summary>
    public (bool[] Buttons, Dictionary<string, float> Axes)? PollState(IntPtr joystick)
    {
        if (joystick == IntPtr.Zero)
            return null;

        SDL.UpdateJoysticks();
        if (!SDL.JoystickConnected(joystick))
            return null;

        var buttonCount = Math.Max(0, SDL.GetNumJoystickButtons(joystick));
        var axisCount = Math.Max(0, SDL.GetNumJoystickAxes(joystick));

        var buttons = new bool[buttonCount];
        for (int i = 0; i < buttonCount; i++)
            buttons[i] = SDL.GetJoystickButton(joystick, i);

        var axes = new Dictionary<string, float>(AxisNames.Length);
        for (int i = 0; i < AxisNames.Length; i++)
        {
            var raw = i < axisCount ? SDL.GetJoystickAxis(joystick, i) : (short)0;
            axes[AxisNames[i]] = Normalize(raw);
        }

        return (buttons, axes);
    }

    public void ReleaseDevice(IntPtr joystick)
    {
        if (joystick != IntPtr.Zero)
            SDL.CloseJoystick(joystick);
    }

    private static float Normalize(short rawValue) =>
        rawValue >= 0 ? rawValue / 32767f : rawValue / 32768f;

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        SDL.QuitSubSystem(SDL.InitFlags.Joystick);
    }
}
