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
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Devices.Client;
using System.Diagnostics;
using Windows.Devices.Gpio;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace IotCoreRPi
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private String ConnectionString { get; set; }

        private DispatcherTimer timer;

        private List<IotSensor> sensors;

        Random random = new Random();

        RPi rpi;

        const string CMD = "#cmd:";
        const string PIN_CMD = "set_pin";


        public MainPage()
        {
            this.InitializeComponent();

            ConnectionString = "HostName=smarthomecloud.azure-devices.net;DeviceId=Rpi1;SharedAccessKey=NDTJ9dMxEGc2Qdf8RMjpDBBvTAkC3smn8VYu+h6S4iI=";

            ReceiveDataFromAzure();

            // Hard coding guid for sensors. Not an issue for this particular application which is meant for testing and demos
            sensors = new List<IotSensor> {
                new IotSensor("2298a348-e2f9-4438-ab23-82a3930662ab", "Temperature", "C",  "IoT Sensor", "FancyCloud"),

            };

            InitTimer();

            InitRPi();

            InitGPIO();
        }

        private void InitTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {   IotSensor sensor = sensors.Find(item => item.measurename == "Temperature");
            sensor.value = random.Next(50);
            sensor.timecreated = DateTime.UtcNow.ToString("o");
            SendDataToAzure(sensor.ToJson());
        }

        private async Task SendDataToAzure(String data)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(ConnectionString, TransportType.Http1);

            var msg = new Message(Encoding.UTF8.GetBytes(data));

            await deviceClient.SendEventAsync(msg);
        }

        public async Task ReceiveDataFromAzure()
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(ConnectionString, TransportType.Http1);

            Message receivedMessage;
            string messageData;

            while (true)
            {
                receivedMessage = await deviceClient.ReceiveAsync();

                if (receivedMessage != null)
                {
                    messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                    ProcessMessage(messageData);
                    await deviceClient.CompleteAsync(receivedMessage);
                }
            }
        }

        private void ProcessMessage(string messageData)
        {
            try
            {
                // Get cmd
                int cmdIndex = messageData.IndexOf(CMD);

                if (cmdIndex >= 0)
                {
                    string[] tokens = messageData.Substring(cmdIndex + CMD.Length).Split(';');

                    if (tokens.Length >= 1)
                    {
                        if (tokens[0] == PIN_CMD)
                        {
                            if (tokens.Length >= 3)
                            {
                                int pinNumber = int.Parse(tokens[1]);
                                double value = double.Parse(tokens[2]);

                                rpi.SetPinValue(pinNumber, value == 1 ? GpioPinValue.High : GpioPinValue.Low);
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }

        private void InitRPi()
        {
            if(RPi.IsRealDevice())
            {
                rpi = new RPi();
            }
            else
            {
                Debug.WriteLine("Application isn't running on RPi!");
            }
        }

        private void InitGPIO()
        {
            if(rpi != null)
            {
                rpi.Init();
                rpi.SetPinValue(RPi.LED_PIN, GpioPinValue.Low);
                rpi.SetPinValue(RPi.START_UP_LED_PIN, GpioPinValue.High);
            }
        }
    }
}
