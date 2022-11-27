using System.Windows;
using StickyNotesApp.Manager;

namespace StickyNotesApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private NoteManager _noteManager;

        public App()
        {
            _noteManager = new NoteManager();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _noteManager.NewNote();
        }
    }
}