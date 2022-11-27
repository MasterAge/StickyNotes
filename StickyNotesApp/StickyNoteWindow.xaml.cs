using System;
using System.Windows.Controls;
using System.Windows.Input;
using StickyNotesApp.Manager;

namespace StickyNotesApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class StickyNoteWindow
    {
        private NoteManager _noteManager;
        public StickyNoteWindow(NoteManager noteManager)
        {
            _noteManager = noteManager;
            InitializeComponent();
        }

        private void NewNote(object sender, EventArgs data)
        {
            _noteManager.NewNote();
        }

        private void MoveNote(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseNote(object sender, EventArgs data)
        {
            this.Close();
        }
    }
}