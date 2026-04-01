using System;
using System.Collections.Generic;
using DInputTester.Models;
using Vortice.DirectInput;

namespace DInputTester.Services;

public sealed class DirectInputService : IDisposable
{
    private IDirectInput8 _directInput;
    private bool _disposed;

    public DirectInputService()
    {
        _directInput = DInput.DirectInput8Create();
    }

    /// <summary>Enumerates all attached HID game controllers via DirectInput.</summary>
    public List<GameDeviceInfo> EnumerateDevices()
    {
        var result = new List<GameDeviceInfo>();

        var infos = _directInput.GetDevices(DeviceClass.GameControl, DeviceEnumerationFlags.AttachedOnly);
        foreach (var info in infos)
        {
            result.Add(new GameDeviceInfo(
                info.InstanceGuid,
                info.ProductName?.Trim('\0') ?? "Unknown",
                info.InstanceName?.Trim('\0') ?? "Unknown"));
        }

        return result;
    }

    /// <summary>Creates and acquires a joystick device. Caller is responsible for disposing.</summary>
    public IDirectInputDevice8? AcquireDevice(Guid instanceGuid)
    {
        var device = _directInput.CreateDevice(instanceGuid);
        if (device is null) return null;

        device.SetDataFormat<RawJoystickState>();
        device.SetCooperativeLevel(IntPtr.Zero, CooperativeLevel.NonExclusive | CooperativeLevel.Background);

        var hr = device.Acquire();
        if (hr.Failure)
        {
            device.Dispose();
            return null;
        }

        return device;
    }

    /// <summary>
    /// Polls the device and returns buttons and axes.
    /// Axes are normalized to [-1.0, 1.0].
    /// Returns null if polling fails (e.g. device disconnected).
    /// </summary>
    public (bool[] Buttons, Dictionary<string, float> Axes)? PollState(IDirectInputDevice8 device)
    {
        var hr = device.Poll();
        if (hr.Failure)
        {
            device.Acquire();
            return null;
        }

        JoystickState state;
        try
        {
            state = device.GetCurrentJoystickState();
        }
        catch
        {
            return null;
        }

        var buttons = state.Buttons;

        var axes = new Dictionary<string, float>(8);
        axes["X"]       = Normalize(state.X);
        axes["Y"]       = Normalize(state.Y);
        axes["Z"]       = Normalize(state.Z);
        axes["RX"]      = Normalize(state.RotationX);
        axes["RY"]      = Normalize(state.RotationY);
        axes["RZ"]      = Normalize(state.RotationZ);
        axes["Slider1"] = Normalize(state.Sliders[0]);
        axes["Slider2"] = Normalize(state.Sliders[1]);

        return (buttons, axes);
    }

    private static float Normalize(int rawValue) =>
        Math.Clamp((rawValue - 32767f) / 32768f, -1f, 1f);

    public void Dispose()
    {
        if (_disposed) return;
        _disposed = true;
        _directInput.Dispose();
    }
}
