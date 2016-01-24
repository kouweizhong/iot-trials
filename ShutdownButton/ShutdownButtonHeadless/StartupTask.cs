using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using Windows.ApplicationModel.Background;
using Windows.Devices.Gpio;
using Windows.System;

// The Background Application template is documented at http://go.microsoft.com/fwlink/?LinkID=533884&clcid=0x409

namespace ShutdownButtonHeadless
{
    public sealed class StartupTask : IBackgroundTask
    {
        BackgroundTaskDeferral _deferral;

        private const int BUTTON_PIN = 18;
        private GpioPin _buttonpin;

        public void Run(IBackgroundTaskInstance taskInstance)
        {
            // If you start any asynchronous methods here, prevent the task
            // from closing prematurely by using BackgroundTaskDeferral as
            // described in http://aka.ms/backgroundtaskdeferral
            //

            // Add insights
            //Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
            //    Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
            //    Microsoft.ApplicationInsights.WindowsCollectors.Session);

            _deferral = taskInstance.GetDeferral();

            InitGPIO();

        }
        private void InitGPIO()
        {
            _buttonpin = GpioController.GetDefault().OpenPin(BUTTON_PIN);
            _buttonpin.SetDriveMode(GpioPinDriveMode.Input);
            _buttonpin.ValueChanged += ButtonpinValueChanged;
        }

        private void ButtonpinValueChanged(GpioPin sender, GpioPinValueChangedEventArgs args)
        {
            // Just shutdown on one edge
            if (args.Edge == GpioPinEdge.RisingEdge)
            {
                ShutdownManager.BeginShutdown(ShutdownKind.Shutdown, TimeSpan.FromSeconds(0.5));
            }
        }
    }
}
