using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ConnectTheDotsIoT
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        int counter = 0; // dummy temp counter value;

        ConnectTheDotsHelper ctdHelper;
        DispatcherTimer timer;

        /// <summary>
        /// Main page constructor
        /// </summary>
        public MainPage()
        {
            this.InitializeComponent();

            // Hard coding guid for sensors. Not an issue for this particular application which is meant for testing and demos
            List<ConnectTheDotsSensor> sensors = new List<ConnectTheDotsSensor> {
                new ConnectTheDotsSensor("2298a348-e2f9-4438-ab23-82a3930662ab", "Temperature", "F"),
            };


            ctdHelper = new ConnectTheDotsHelper(serviceBusNamespace: "smarthomecloud-ns",
                    eventHubName: "ehdevices",
                    keyName: "D1",
                    key: "3J1lMPNlLSUwimyMn1M7yRimI3Y7U7oa4K8MUOU2/xg=",
                    displayName: "RPI",
                    organization: "FH Hagenberg",
                    location: "Hagenberg",
                    sensorList: sensors);

            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        Random random = new Random();

        private void Timer_Tick(object sender, object e)
        {
            ConnectTheDotsSensor sensor = ctdHelper.sensors.Find(item => item.measurename == "Temperature");
            sensor.value = random.Next(50);
            ctdHelper.SendSensorData(sensor);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ConnectTheDotsSensor sensor = ctdHelper.sensors.Find(item => item.measurename == "Temperature");
            sensor.value = counter++;
            ctdHelper.SendSensorData(sensor);
        }
    }
}
