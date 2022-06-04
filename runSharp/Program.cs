﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Windows; // Application

namespace Mms
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException +=
                    new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
        static void CurrentDomain_UnhandledException
                (object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = (Exception)e.ExceptionObject;

                MessageBox.Show("Whoops! Please contact the developers with the following"
                      + " information:\n\n" + ex.Message + ex.StackTrace,
                      "Fatal Error", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            finally
            {
                Application.Exit();
            }
        }
    }
}