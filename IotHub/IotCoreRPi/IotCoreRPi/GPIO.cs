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
        private GpioPinValue pinValue;

        private void InitGPIO(int pinNr, GpioPinDriveMode mode)
        {
            var gpio = GpioController.GetDefault();

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                throw new Exception("No GPIO Controller");
            }

            pin = gpio.OpenPin(pinNr);
            pinValue = GpioPinValue.High;
            pin.Write(pinValue);
            pin.SetDriveMode(mode);
        }
    }


}
