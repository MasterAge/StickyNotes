using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using StickyNotesApp.Manager;
using StickyNotesApp.ViewModels;

namespace StickyNotesApp
{
    public partial class StickyNoteWindow
    {
        private readonly NoteManager _noteManager;

        private StickyNoteWindowVm viewModel;

        public StickyNoteWindow(NoteManager noteManager, StickyNoteWindowVm viewModel)
        {
            InitializeComponent();
            _noteManager = noteManager;
            this.viewModel = viewModel;
            DataContext = viewModel;
        }

        public void SetContent(byte[] noteContent)
        {
            if (noteContent != null)
            {
                new TextRange(NoteTextBox.Document.ContentStart, NoteTextBox.Document.ContentEnd)
                    .Load(new MemoryStream(noteContent), DataFormats.XamlPackage);
            }
        }

        private void NewNote(object sender, RoutedEventArgs data)
        {
            _noteManager.CreateNote();
        }

        private void MoveNote(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void CloseNote(object sender, RoutedEventArgs data)
        {
            this.Close();
            _noteManager.CloseNote(this);
        }

        private void DeleteNote(object sender, EventArgs data)
        {
            this.Close();
            _noteManager.DeleteNote(this);
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

        private void ShowMenu(object sender, RoutedEventArgs data)
        {
            MenuGrid.Visibility = Visibility.Visible;
        }

        private void HideMenu(object sender, EventArgs data)
        {
            MenuGrid.Visibility = Visibility.Hidden;
        }

        private void ShowHistory(object sender, EventArgs data)
        {
            
        }

        private void OnLostFocus(object sender, EventArgs e)
        {
            TopBar.Visibility = Visibility.Collapsed;
            MinimizedTopBar.Visibility = Visibility.Visible;
        }

        private void OnGotFocus(object sender, EventArgs e)
        {
            TopBar.Visibility = Visibility.Visible;
            MinimizedTopBar.Visibility = Visibility.Collapsed;
        }

        private void OnSetColor(object sender, SolidColorBrush color)
        {
            viewModel.Color = color;
            MenuGrid.Visibility = Visibility.Collapsed;
            _noteManager.UpdateNoteColor(this, color);
        }
    }
}