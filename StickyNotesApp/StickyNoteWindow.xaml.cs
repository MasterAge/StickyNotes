using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using StickyNotesApp.Manager;

namespace StickyNotesApp
{
    public partial class StickyNoteWindow
    {
        private readonly NoteManager _noteManager;
        
        public StickyNoteWindow(NoteManager noteManager)
        {
            _noteManager = noteManager;
            InitializeComponent();
        }

        public void SetContent(byte[] noteContent)
        {
            new TextRange(NoteTextBox.Document.ContentStart, NoteTextBox.Document.ContentEnd)
                .Load(new MemoryStream(noteContent), DataFormats.XamlPackage);
        }

        private void NewNote(object sender, EventArgs data)
        {
            _noteManager.CreateNote();
        }

        private void MoveNote(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseNote(object sender, EventArgs data)
        {
            this.Close();
            _noteManager.CloseNote(this);
        }

        private void UpdateText(object sender, TextChangedEventArgs textChangedEventArgs)
        {
            var noteContent = new MemoryStream();
            new TextRange(NoteTextBox.Document.ContentStart, NoteTextBox.Document.ContentEnd)
                .Save(noteContent, DataFormats.XamlPackage);
            _noteManager.UpdateNoteContent(this, noteContent.ToArray());
        }

        private void UpdatePosition(object sender, EventArgs eventArgs)
        {
            _noteManager.UpdateNotePosition(this, new Point(Left, Top));
        }

        private void UpdateSize(object sender, SizeChangedEventArgs e)
        {
            _noteManager.UpdateNoteSize(this, Width, Height);
        }
    }
}