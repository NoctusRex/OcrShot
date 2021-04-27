using OcrShot.Hotkeys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Drawing;

namespace OcrShot.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Hotkey ScreenshotHotkey { get; set; }
        private NotifyIcon NotifyIcon { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ScreenshotHotkey = new Hotkey(Constants.NOMOD, Keys.Pause, this);
            ScreenshotHotkey.Triggered += ScreenshotHotkeyTriggered;
            ScreenshotHotkey.Register();

            NotifyIcon = new NotifyIcon()
            {
                Visible = true,
                BalloonTipText = "Barcode Reader",
                BalloonTipTitle = "Barcode Reader",
                Text = "Barcode Reader",
                Icon = Properties.Resources.ocr
            };
            NotifyIcon.DoubleClick += NotifyIconDoubleClick;

            ShowInTaskbar = false;
            Hide();
        }

        private void ScreenshotHotkeyTriggered(object sender, EventArgs e)
        {
            TakeScreenshot();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ScreenshotHotkey.Unregister();
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
            {
                NotifyIcon.Visible = true;
                ShowInTaskbar = false;
                WindowState = WindowState.Normal;
                Hide();
            }
        }

        private void NotifyIconDoubleClick(object sender, EventArgs e)
        {
            NotifyIcon.Visible = false;
            ShowInTaskbar = true;
            Show();
            Activate();
        }

        #endregion

        private void TakeScreenshot()
        {
            Bitmap image = new ScreenshotWindow().TakeScreenshot();
            if (image != null) image.Save("./test.jpg");
        }
    }
}
