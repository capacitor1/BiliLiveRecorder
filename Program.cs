using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace BiliLiveRecorder
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.ThreadException += Application_ThreadException;
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            //LiveRecMain.fs.Write(Encoding.Default.GetBytes(string.Format("Unhandled exception£º{0}\r\nInformation£º{1}\r\nStackTrace£º{2}", ex.GetType(), ex.Message, ex.StackTrace) + "\r\n"));
            MessageBox.Show(string.Format("Unhandled exception£º{0}\r\nInformation£º{1}\r\nStackTrace£º{2}", ex.GetType(), ex.Message, ex.StackTrace));
        }
    }
}