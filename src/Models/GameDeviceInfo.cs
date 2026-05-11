using System;

namespace DInputTester.Models;

public sealed record GameDeviceInfo(
    uint InstanceId,
    string ProductName,
    string InstanceName);
