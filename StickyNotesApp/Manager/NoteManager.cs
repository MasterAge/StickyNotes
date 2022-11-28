using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Newtonsoft.Json;
using static System.Environment;

namespace StickyNotesApp.Manager
{
    public class NoteState
    {
        public byte[] Content { get; set; }

        public bool Open { get; set; }

        public readonly Guid Id;

        public Point Position { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public NoteState()
        {
            Open = true;
            Id = Guid.NewGuid();
            Position = new Point(0, 0);
            Width = 225;
            Height = 350;
        }

        [JsonConstructor]
        public NoteState(byte[] content, bool open, Guid id, Point position, double width, double height)
        {
            Content = content;
            Open = open;
            Id = id;
            Position = position;
            Width = width;
            Height = height;
        }
    }

    /**
     * Manages note state and windows
     */
    public class NoteManager
    {
        public const string DataFolder = "StickyNotes";
        public const string DataFileName = "stickynotes.json";

        private readonly string _dataPath;

        // Maintains the note state
        private Dictionary<Guid, NoteState> _notes;

        // Maps instances of windows to note ids.
        private readonly Dictionary<StickyNoteWindow, Guid> _windows;

        public NoteManager()
        {
            _notes = new Dictionary<Guid, NoteState>();
            _windows = new Dictionary<StickyNoteWindow, Guid>();

            _dataPath = Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), DataFolder);
            Directory.CreateDirectory(_dataPath);
        }

        /**
         * Retrieve the note state from a window reference.
         */
        private NoteState GetNoteState(StickyNoteWindow window)
        {
            return _notes[_windows[window]];
        }

        public void CreateNote()
        {
            var window = new StickyNoteWindow(this);
            var state = new NoteState();
            _windows.Add(window, state.Id);
            _notes.Add(state.Id, state);

            window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            window.Show();
        }

        /**
         * Delete a note from the history.
         */
        public void DeleteNote(StickyNoteWindow window)
        {
            _notes.Remove(_windows[window]);
            _windows.Remove(window);
            SaveNotes();
        }

        /**
         * Open an existing note.
         */
        public void OpenNote(Guid id)
        {
            var noteState = _notes[id];
            var window = new StickyNoteWindow(this);
            _windows.Add(window, id);
            noteState.Open = true;

            if (noteState.Content != null)
            {
                window.SetContent(noteState.Content);
            }

            window.Top = noteState.Position.Y;
            window.Left = noteState.Position.X;
            window.Width = noteState.Width;
            window.Height = noteState.Height;
            window.Show();
        }

        /**
         * Hide a note but keep its contents.
         */
        public void CloseNote(StickyNoteWindow window)
        {
            var noteState = GetNoteState(window);
            noteState.Open = false;
            if (noteState.Content != null)
            {
                SaveNotes();
            }

            _windows.Remove(window);
        }

        public void UpdateNoteContent(StickyNoteWindow window, byte[] noteContent)
        {
            GetNoteState(window).Content = noteContent;
            SaveNotes();
        }

        public void UpdateNotePosition(StickyNoteWindow window, Point position)
        {
            GetNoteState(window).Position = position;
            SaveNotes();
        }

        public void UpdateNoteSize(StickyNoteWindow window, double width, double height)
        {
            var noteState = GetNoteState(window);
            noteState.Width = width;
            noteState.Height = height;
            SaveNotes();
        }

        public void SaveNotes()
        {
            var jsonNotes = JsonConvert.SerializeObject(_notes, Formatting.Indented);
            File.WriteAllText(Path.Combine(_dataPath, DataFileName), jsonNotes);
        }

        public void LoadNotes()
        {
            var notesFIle = Path.Combine(_dataPath, DataFileName);
            if (File.Exists(notesFIle))
            {
                var jsonNotes = File.ReadAllText(notesFIle);
                var deserializedNotes = JsonConvert.DeserializeObject<Dictionary<Guid, NoteState>>(jsonNotes);
                if (deserializedNotes != null)
                {
                    _notes = deserializedNotes;

                    // Open the previously open notes.
                    foreach (var note in _notes.Where(note => note.Value.Open))
                    {
                        OpenNote(note.Key);
                    }
                }
            }

            // If no notes have been opened, make a new one.
            if (!_notes.Values.Any(note => note.Open))
            {
                CreateNote();
            }
        }
    }
}