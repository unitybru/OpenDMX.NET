using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ColorPicker.Models;
using OpenDMX.NET;

namespace WPF_DMX
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DmxController dmx;
        
        public MainWindow()
        {
            InitializeComponent();
            dmx = new DmxController();
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
            }
        }

        public void SendData(byte Dimmer, byte R, byte G, byte B)
        {
            // Set RGB channels 
            dmx.SetChannels(1, new byte[] { Dimmer, R, G, B, 0 });
            Thread.Sleep(1000);
        }

        void PickerControlBase_OnColorChanged(object sender, RoutedEventArgs e)
        {
            
            Thread thread1 = new Thread(SendDataAsync);
            thread1.Start(e as ColorRoutedEventArgs);
            
            //var colorArgs = e as ColorRoutedEventArgs;
            //SendData((byte)colorArgs.Color.A, (byte)colorArgs.Color.R, (byte)colorArgs.Color.G, (byte)colorArgs.Color.B);
        }

        void SendDataAsync(object obj)
        {
            var colorArgs = obj as ColorRoutedEventArgs;
            SendData((byte)colorArgs.Color.A, (byte)colorArgs.Color.R, (byte)colorArgs.Color.G, (byte)colorArgs.Color.B);
        }
    }
}
