using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
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

        Socket artnetSocket;
        
        public MainWindow()
        {
            InitializeComponent();
            InitializeDMX();
            InitializeArtNet();
        }

        void InitializeDMX()
        {
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

        void InitializeArtNet()
        {
            artnetSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            artnetSocket.Connect(IPAddress.Parse("127.0.0.1"), 6454);
        }

        /// <summary>
        /// Send DMX and Art-Net data
        /// </summary>
        /// <param name="Dimmer"></param>
        /// <param name="R"></param>
        /// <param name="G"></param>
        /// <param name="B"></param>
        public void SendData(byte Dimmer, byte R, byte G, byte B)
        {
            var byteData = new byte[] { Dimmer, R, G, B, 0 };
            
            // Send DMX via serial data
            dmx.SetChannels(1, byteData);
            
            // Send Art-Net data via UDP socket
            byte[] data = { 65, 114, 116, 45, 78, 101, 116, 0, 0, 80, 0, 14, 0, 0, 0, 0, 0, 4, R, G, B, Dimmer };
            artnetSocket.Send(data);
        }

        void PickerControlBase_OnColorChanged(object sender, RoutedEventArgs e)
        {
            Thread thread1 = new Thread(SendDataAsync);
            thread1.Start(e as ColorRoutedEventArgs);
        }

        void SendDataAsync(object obj)
        {
            var colorArgs = obj as ColorRoutedEventArgs;
            SendData(colorArgs.Color.A, colorArgs.Color.R, colorArgs.Color.G, colorArgs.Color.B);
        }
    }
}
