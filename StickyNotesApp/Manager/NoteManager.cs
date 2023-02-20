using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Media;
using Newtonsoft.Json;
using StickyNotesApp.ViewModels;
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
        
        public string Color { get; set; }

        public NoteState()
        {
            Open = true;
            Id = Guid.NewGuid();
            Position = new Point(0, 0);
            Width = 225;
            Height = 350;
            Color = "#E6B905";
        }

        [JsonConstructor]
        public NoteState(byte[] content, bool open, Guid id, Point position, double width, double height, string color)
        {
            Content = content;
            Open = open;
            Id = id;
            Position = position;
            Width = width;
            Height = height;
            Color = color;
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

        private readonly Timer _writeTimer;

        public NoteManager()
        {
            _notes = new Dictionary<Guid, NoteState>();
            _windows = new Dictionary<StickyNoteWindow, Guid>();

            _dataPath = Path.Combine(GetFolderPath(SpecialFolder.LocalApplicationData), DataFolder);
            Directory.CreateDirectory(_dataPath);

            _writeTimer = new Timer(5000);
            _writeTimer.Elapsed += OnWriteTimerElapsed;
        }
        
        private void OnWriteTimerElapsed(object sender, ElapsedEventArgs e)
        {
            SaveNotes();
            _writeTimer.Stop();
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
            var state = new NoteState();
            var window = new StickyNoteWindow(this, new StickyNoteWindowVm(state.Color));
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
            RequestSave();
        }

        /**
         * Open an existing note.
         */
        public void OpenNote(Guid id)
        {
            var noteState = _notes[id];
            var window = new StickyNoteWindow(this, new StickyNoteWindowVm(noteState.Color));
            _windows.Add(window, id);
            noteState.Open = true;

            window.SetContent(noteState.Content);

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
                RequestSave();
            }

            _windows.Remove(window);
        }

        public void UpdateNoteContent(StickyNoteWindow window, byte[] noteContent)
        {
            GetNoteState(window).Content = noteContent;
            RequestSave();
        }

        public void UpdateNotePosition(StickyNoteWindow window, Point position)
        {
            GetNoteState(window).Position = position;
            RequestSave();
        }

        public void UpdateNoteSize(StickyNoteWindow window, double width, double height)
        {
            var noteState = GetNoteState(window);
            noteState.Width = width;
            noteState.Height = height;
            RequestSave();
        }
        
        public void UpdateNoteColor(StickyNoteWindow window, SolidColorBrush color)
        {
            GetNoteState(window).Color = color.Color.ToString();
            RequestSave();
        }

        /**
         * Request the notes to be saved to disk. The app
         */
        private void RequestSave()
        {
            Debug.WriteLine("Save Requested");
            _writeTimer.Stop();
            _writeTimer.Start();
        }

        public void SaveNotes()
        {
            var jsonNotes = JsonConvert.SerializeObject(_notes, Formatting.Indented);
            File.WriteAllText(Path.Combine(_dataPath, DataFileName), jsonNotes);
            Debug.WriteLine("Saved notes to " + Path.Combine(_dataPath, DataFileName).ToString());
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