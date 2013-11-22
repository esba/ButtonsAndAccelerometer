using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT.Presentation;
using Microsoft.SPOT.Presentation.Media;
using System.Threading;
using Windows.Devices.Sensors;
using AGENT.Contrib.Hardware;

namespace ButtonsAndAccelerometer
{
    public class Program
    {
        static Bitmap _display;
        static Accelerometer _accelerometer;

        static Font _fontNinaB;

        public static void Main()
        {
            // turn off GC messages in the debug window (advanced; advanced users should remvoe this line)
            Debug.EnableGCMessages(false);


            // initialize display buffer
            _display = new Bitmap(Bitmap.MaxWidth, Bitmap.MaxHeight);

            // load our NinaB font; we'll use this for drawing.
            _fontNinaB = Resources.GetFont(Resources.FontResources.NinaB);

            // connect to our watch's accelerometer
            _accelerometer = Accelerometer.GetDefault();
            // get a reading, to display default values.
            UpdateDisplay(_accelerometer.GetCurrentReading());

            // wire up our ReadingChanged event, to update the display whenever our g forces change
            _accelerometer.ReadingChanged += _accelerometer_ReadingChanged;

            ButtonHelper.ButtonSetup = new Buttons[] { Buttons.TopRight, Buttons.BottomRight, Buttons.MiddleRight };
            ButtonHelper.Current.OnButtonPress += Current_OnButtonPress; // Do something when a button is pressed

            // go to sleep; all further code should be timer-driven or event-driven
            Thread.Sleep(Timeout.Infinite);
        }

        static void _accelerometer_ReadingChanged(object sender, AccelerometerReadingChangedEventArgs e)
        {
            UpdateDisplay(e.Reading);
        }

        static void Current_OnButtonPress(Buttons button, InterruptPort port, ButtonDirection direction, DateTime time)
        {
            if (button == Buttons.TopRight && direction == ButtonDirection.Up)
            {
            }

            if (button == Buttons.MiddleRight && direction == ButtonDirection.Up)
            {
            }

            if (button == Buttons.BottomRight && direction == ButtonDirection.Up)
            {
            }
        }

        static void UpdateDisplay(AccelerometerReading reading)
        {
            // clear screen
            _display.Clear();

            // draw our X, Y, and Z axis acceleration values (on -1G to 1G bars)
            DrawAccelerationValue(25, "X", reading.AccelerationX);
            DrawAccelerationValue(57, "Y", reading.AccelerationY);
            DrawAccelerationValue(89, "Z", reading.AccelerationZ);

            // flush our display buffer to the display
            _display.Flush();
        }

        static void DrawAccelerationValue(int yPos, string axisLabel, double acceleration)
        {
            // draw axis text and bar outline
            _display.DrawText(axisLabel, _fontNinaB, Color.White, 4, yPos);
            _display.DrawRectangle(Color.White, 1, 16, yPos + 3, 96, 8, 2, 2, Color.White, 0, 0, Color.White, 0, 0, 0);

            // calculate pixel width of axis reading, based on scale of -1G to 1G on a 96-pixel bar.  NOTE: accelerometer measures -2G to 2G by default on all three axes
            int width = System.Math.Abs((int)(acceleration * 48.0)); // scale {-2G to 2G} to a 96-pixel bar of {-1G to 1G}
            if (width == 0) width = 1; // make sure x is visible by enforcing width of at least 1 pixel

            // draw axis accelerometer value within bar outline, centered, abs(width) pixels wide
            _display.DrawRectangle(Color.White, 1, acceleration < 0.0 ? 64 - width : 64, yPos + 3, width, 8, 0, 0, Color.White, 0, 0, Color.White, 127, 127, 100);
        }
    }
}
