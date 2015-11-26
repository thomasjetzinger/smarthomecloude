using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace IotCoreRPi
{
    public sealed class RPi
    {
        Dictionary<int, GPIO> pins = new Dictionary<int, GPIO>();

        public const int LED_PIN = 4;
        public const int START_UP_LED_PIN = 22;

        public static bool IsRealDevice()
        {
            return GpioController.GetDefault() != null;
        }

        public void Init()
        {
            pins.Add(START_UP_LED_PIN, new GPIO());
            InitOutput(START_UP_LED_PIN);

            pins.Add(LED_PIN, new GPIO());
            InitOutput(LED_PIN);
        }

        private void InitOutput(int pinNr)
        {
            pins[pinNr].InitGPIO(pinNr, GpioPinDriveMode.Output);
        }

        public void SetPinValue(int pinNumber, GpioPinValue value)
        {
            pins[pinNumber].SetValue(value);
        }
    }
}
