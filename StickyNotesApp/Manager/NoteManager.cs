using System;
using System.Collections.Generic;

namespace StickyNotesApp.Manager
{
    public class NoteManager
    {
        private List<StickyNoteWindow> windows;
        
        public NoteManager()
        {
            windows = new List<StickyNoteWindow>();
        }

        public void NewNote()
        {
            var window = new StickyNoteWindow(this);
            window.Show();
            windows.Add(window);
            Console.Out.WriteLine("Spawned a note");
        }

        public void CloseNote(StickyNoteWindow window)
        {
            windows.Remove(window);
        }
    }
}