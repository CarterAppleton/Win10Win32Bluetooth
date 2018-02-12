using System;
using Windows.Devices.Bluetooth.Advertisement;

namespace ConsoleApplication1
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // Start the program
            var program = new Program();

            // Close on key press
            Console.WriteLine("Scanning ... Press any key to cancel (finish).");
            Console.ReadKey();
        }

        public Program()
        {
            // Create Bluetooth Listener
            var watcher = new BluetoothLEAdvertisementWatcher();

            watcher.ScanningMode = BluetoothLEScanningMode.Active;

            // Only activate the watcher when we're recieving values >= -80
            watcher.SignalStrengthFilter.InRangeThresholdInDBm = -80;

            // Stop watching if the value drops below -90 (user walked away)
            watcher.SignalStrengthFilter.OutOfRangeThresholdInDBm = -90;

            // Wait 5 seconds to make sure the device is really out of range
            watcher.SignalStrengthFilter.OutOfRangeTimeout = TimeSpan.FromMilliseconds(5000);
            watcher.SignalStrengthFilter.SamplingInterval = TimeSpan.FromMilliseconds(2000);

            // Register callback for when we see an advertisements
            watcher.Received += OnAdvertisementReceived;

            // Register callback for scan stop or error, especially
            watcher.Stopped += OnStopped;

            // Starting watching for advertisements
            watcher.Start();
        }

        private void OnAdvertisementReceived(BluetoothLEAdvertisementWatcher watcher, BluetoothLEAdvertisementReceivedEventArgs eventArgs)
        {
            // Tell the user we see an advertisement and print some properties
            Console.WriteLine(String.Format("Advertisement:"));
            Console.WriteLine(String.Format("  BT_ADDR: {0}", eventArgs.BluetoothAddress));
            Console.WriteLine(String.Format("  FR_NAME: {0}", eventArgs.Advertisement.LocalName));
            Console.WriteLine();
        }

        private void OnStopped(BluetoothLEAdvertisementWatcher sender, BluetoothLEAdvertisementWatcherStoppedEventArgs args)
        {
            // Tell the user when the advertisement finishes
            Console.WriteLine(String.Format("Advertisement stopped with: {0}", args.Error));
            Console.WriteLine("Press any key to finish.");
        }
    }
}