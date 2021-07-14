using System;
using System.Linq;
using System.Threading;

namespace OpenDMX.NET.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
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
        }
    }
}
