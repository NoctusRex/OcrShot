using OcrShot.Windows;
using System;
using System.Data;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace OcrShot
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private Mutex Mutex { get; set; }
        private bool HandleExceptions { get; set; } = false;

        private void Application_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = HandleExceptions;

            ErrorWindow.Show(e.Exception);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                CheckAlreadyRunning();
            }
            catch (Exception ex)
            {
                ErrorWindow.Show(ex);
                Current.Shutdown();
            }
            finally
            {
                HandleExceptions = true;
            }
        }

        private void CheckAlreadyRunning()
        {
            Mutex = new Mutex(false, "OcrShot - 133742069");

            if (!Mutex.WaitOne(0, false))
            {
                throw new DuplicateNameException("The application is already running.");
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            Mutex?.Close();
            Mutex?.Dispose();
            Mutex = null;

            base.OnExit(e);
        }

    }
}
