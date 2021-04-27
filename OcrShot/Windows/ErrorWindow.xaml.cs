using System;
using System.Diagnostics;
using System.Windows;

namespace OcrShot.Windows
{
    /// <summary>
    /// Interaction logic for ErrorWindow.xaml
    /// </summary>
    public partial class ErrorWindow : Window
    {
        /// <summary>
        /// Shows the error window as dialog
        /// </summary>
        /// <param name="ex"></param>
        public static void Show(Exception ex) => new ErrorWindow(ex).ShowDialog();

        public ErrorWindow()
        {
            InitializeComponent();
        }

        public ErrorWindow(Exception ex) : this()
        {
            TextBlockError.Text = ex.Message;
            TextBlockStackTrace.Text = ex.StackTrace;
        }

        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
