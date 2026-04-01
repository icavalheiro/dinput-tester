using System;

namespace DInputTester.Models;

public sealed record GameDeviceInfo(
    Guid InstanceGuid,
    string ProductName,
    string InstanceName);
