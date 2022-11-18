﻿using Mmx.Gui.Win.Common;
using Mmx.Gui.Win.Common.Properties;
using System;
using System.Globalization;
using System.Threading;
using System.Windows;

namespace Mmx.Gui.Win.Wpf.Harvester
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(Settings.Default.LanguageCode);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(Settings.Default.LanguageCode);

            if (!SingleInstance.IsFirstInstance())
            {
                if (!SingleInstance.ShowFirstInstance())
                {
                    MessageBox.Show(Common.Properties.Resources.Another_Instance_Running, Common.Properties.Resources.Warning);
                }
                Application.Current.Shutdown();
            }
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show((e.ExceptionObject as Exception).ToString(), "Warning! UnhandledException");
        }
    }
}
