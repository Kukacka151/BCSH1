using System;
using System.Windows.Forms;
using CalendarApp.Ui;

namespace CalendarApp;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new MainForm());
    }
}

