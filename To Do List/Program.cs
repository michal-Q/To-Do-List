using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace To_Do_List
{
    internal static class Program
    {
        static Mutex mutex = new Mutex(true, "{A822B10D-6C33-4F43-AB9C-EE4719E5B9C4}"); // Unique identifier

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (mutex.WaitOne(TimeSpan.Zero, true))
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new ToDoList());
                mutex.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("Application is already running.");
            }
        }
    }
}
