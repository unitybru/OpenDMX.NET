# OpenDMX.NET
### Simple and easy-to-use library that allows you to communicate with  OpenDMX USB interfaces.

![stage light](https://i.imgur.com/lZ4cffK.jpg)
# Installation
1. Download and install drivers from http://www.ftdichip.com/Drivers/CDM/CDM21218_Setup.zip
2. Install NuGet package (https://www.nuget.org/packages/OpenDMX.NET/) or clone this repository and add it to your project references.
# NuGet package
https://www.nuget.org/packages/OpenDMX.NET/
# Example usage
```cs
using (var dmx = new DmxController())
{
    var devices = dmx.GetDevices();
    Console.WriteLine($"Detected devices ({devices.Length}): ");

    foreach (var d in devices)
    {
        Console.WriteLine($"> {d.Description} S/N: {d.SerialNumber}");
    }

    // Device detected
    if (devices.Length > 0)
    {
        // Open first device
        dmx.Open(devices.First().DeviceIndex);

        // Set RGB channels 
        dmx.SetChannels(1, new byte[] { 255, 255, 255 });
        Thread.Sleep(1000);
    }
}
```
