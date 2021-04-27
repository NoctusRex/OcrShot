using System;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Rectangle = System.Drawing.Rectangle;

namespace OcrShot.Windows
{
    /// <summary>
    /// Interaction logic for ScreenshotWindow.xaml
    /// </summary>
    public partial class ScreenshotWindow : Window
    {
        private Bitmap TotalScreenshot { get; set; }
        private Bitmap Screenshot { get; set; }
        private System.Drawing.Point X { get; set; }
        private System.Drawing.Point Y { get; set; }

        private int GetAreaWidth { get => Math.Abs(X.X - Y.X); }
        private int GetAreaHeight { get => Math.Abs(X.Y - Y.Y); }

        public ScreenshotWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// https://stackoverflow.com/questions/15847637/take-screenshot-of-multiple-desktops-of-all-visible-applications-and-forms
        /// </summary>
        /// <returns></returns>
        public Bitmap TakeScreenshot()
        {
            // Determine the size of the "virtual screen", which includes all monitors.
            int screenLeft = SystemInformation.VirtualScreen.Left;
            int screenTop = SystemInformation.VirtualScreen.Top;
            int screenWidth = SystemInformation.VirtualScreen.Width;
            int screenHeight = SystemInformation.VirtualScreen.Height;

            // Create a bitmap of the appropriate size to receive the screenshot.
            using Bitmap bmp = new(screenWidth, screenHeight);
            // Draw the screenshot into our bitmap.
            using Graphics g = Graphics.FromImage(bmp);
            g.CopyFromScreen(screenLeft, screenTop, 0, 0, bmp.Size);
            // Do something with the Bitmap here, like save it to a file:
            TotalScreenshot = bmp;
            TotalScreenshotImage.Source = BitmapToImageSource(TotalScreenshot);

            Left = screenLeft;
            Top = screenTop;
            Width = screenWidth;
            Height = screenHeight;

            ShowDialog();
            return Screenshot;
        }

        /// <summary>
        /// https://stackoverflow.com/questions/22499407/how-to-display-a-bitmap-in-a-wpf-image
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using MemoryStream memory = new();
            bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
            memory.Position = 0;
            BitmapImage bitmapimage = new();
            bitmapimage.BeginInit();
            bitmapimage.StreamSource = memory;
            bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
            bitmapimage.EndInit();

            return bitmapimage;
        }

        private void RefreshRectangleSelection()
        {
            if (X == System.Drawing.Point.Empty || Y == System.Drawing.Point.Empty) return;

            GuiCanvas.Children.Clear();

            System.Windows.Shapes.Rectangle rect = new()
            {
                Stroke = new SolidColorBrush(Colors.Red),
                Fill = new SolidColorBrush(Colors.Transparent),
                Width = GetAreaWidth,
                Height = GetAreaHeight,
                StrokeThickness = 2
            };

            // -1 because mouse is on rectangle and does not trigger mouse up event
            Canvas.SetLeft(rect, X.X - 1);
            Canvas.SetTop(rect, X.Y - 1);

            GuiCanvas.Children.Add(rect);
        }

        private System.Drawing.Point ConvertPointToPoint(System.Windows.Point point) => new((int)point.X, (int)point.Y);

        private void CaptureScreenArea()
        {
            Rectangle area = new(X.X, X.Y, GetAreaWidth, GetAreaHeight);

            Screenshot = TotalScreenshot.Clone(area, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
        }

        private void GuiCanvas_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Y = ConvertPointToPoint(e.GetPosition(this));
            RefreshRectangleSelection();
        }

        private void GuiCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            X = ConvertPointToPoint(e.GetPosition(this));
        }

        private void GuiCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            DialogResult = true;

            CaptureScreenArea();

            Close();
        }

    }
}
