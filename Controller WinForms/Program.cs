using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controller_WinForms
{
    static class Program
    {
        public static string Host { get; set; } = "127.0.0.1";
        public static int Port { get; set; } = 44444;
        public static string Password { get; set; } = "abcd";

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmController());
        }
    }
}
