using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Threading;
using StickyNotesApp.Manager;

namespace StickyNotesApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NoteManager _noteManager;

        public App()
        {
            _noteManager = new NoteManager();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _noteManager.LoadNotes();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            _noteManager.SaveNotes();
        }

        void UnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            Debug.WriteLine(e.Exception.ToString());
            e.Handled = true;
        }
    }
}