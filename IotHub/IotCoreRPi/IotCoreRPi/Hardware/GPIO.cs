using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Gpio;

namespace IotCoreRPi
{
    public class GPIO
    {
        private GpioPin pin;

        public void InitGPIO(int pinNr, GpioPinDriveMode mode)
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                throw new Exception("No GPIO Controller");
            }

            pin = gpio.OpenPin(pinNr);
            pin.SetDriveMode(mode);
        }

        public void SetValue(GpioPinValue value)
        {
            if(pin == null)
                throw new Exception("GPIO Pin not initalized!");

            pin.Write(value);
        }
    }


}
