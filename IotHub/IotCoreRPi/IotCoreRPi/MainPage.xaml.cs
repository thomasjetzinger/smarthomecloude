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
using IotCoreRPi.Communication;

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
        private bool SimulateDevice { get; set; }
        const string CMD = "#cmd:";
        const string PIN_CMD = "set_pin";
        private SolidColorBrush redBrush = new SolidColorBrush(Windows.UI.Colors.Red);
        private SolidColorBrush grayBrush = new SolidColorBrush(Windows.UI.Colors.LightGray);

        private static bool AlreadyRunning = false;
        public static int I2C_Slave_Address { get; set; }

        private float Temperature { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

            ConnectionString = "HostName=smarthomecloud-iothub.azure-devices.net;DeviceId=Rpi1;SharedAccessKey=8A7gT7QRZpzLSdqaVlGtgXwyQ8Jn7QRTECI+kqQsFUI=";

            ReceiveDataFromAzure();

            // Hard coding guid for sensors. Not an issue for this particular application which is meant for testing and demos
            sensors = new List<IotSensor> {
                new IotSensor("2298a348-e2f9-4438-ab23-82a3930662ab", "Temperature", "C",  "IoT Sensor", "FancyCloud"),

            };

            InitTimer();

            InitRPi();

            InitGPIO();

            GrabDataAndSendToAzure();
        }

        private void InitTimer()
        {
            timer = new DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(1000);
            timer.Start();
        }

        private void GrabDataAndSendToAzure()
        {
            IotSensor sensor = sensors.Find(item => item.measurename == "Temperature");

            if (SimulateDevice)
                sensor.value = TemperatureSlider.Value;
            else
                sensor.value = Temperature;

            sensor.timecreated = DateTime.UtcNow.ToString("o");
            SendDataToAzure(sensor.ToJson());
        }

        private void Timer_Tick(object sender, object e)
        {
            GrabDataAndSendToAzure();
        }

        private async Task SendDataToAzure(String data)
        {
            DeviceClient deviceClient = DeviceClient.CreateFromConnectionString(ConnectionString, TransportType.Http1);

            Debug.WriteLine("Send to IoT Hub: " + data);

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

                                if(SimulateDevice == false)
                                    rpi.SetPinValue(pinNumber, value == 1 ? GpioPinValue.High : GpioPinValue.Low);

                                if (value == 1)
                                    LED.Fill = redBrush;
                                else
                                    LED.Fill = grayBrush;
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
                SimulateDevice = false;
                GpioStatus.Text = "Hello on Pi";
                I2C_Slave_Address = 0x40;
                CollectData();
            }
            else
            {
                GpioStatus.Text = "Device Simulator";
                SimulateDevice = true;
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

        public void CollectData()
        {
            if (AlreadyRunning == false)
            {
                AlreadyRunning = true;

                Task Task_CollectSensorData = new Task(async () =>
                {
                    while (true)
                    {
                        var Response = await I2C.WriteRead(I2C_Slave_Address, I2C.Mode.Mode0);

                        // Update Temperature
                        Temperature = (byte)Response[3];
                        Temperature *= (((byte)Response[2]) == 0) ? -1 : 1; // Update Temperature Sign. Refer mode 0 for details.

                        await Task.Delay(1000);
                    }
                });

                Task_CollectSensorData.Start();
                Task_CollectSensorData.Wait();
            }
        }
    }
}
