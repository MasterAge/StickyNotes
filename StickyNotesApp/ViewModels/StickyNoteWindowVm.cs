using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Media;
using StickyNotesApp.Utils;

namespace StickyNotesApp.ViewModels
{
    public class StickyNoteWindowVm : INotifyPropertyChanged
    {
        private SolidColorBrush _color;

        public SolidColorBrush Color
        {
            get { return _color; }
            set
            {
                _color = value;
                OnPropertyChanged(nameof(Color));
            }
        }

        public StickyNoteWindowVm(string color)
        {
            _color = ColorUtil.HexToBrush(color);
        }

        public StickyNoteWindowVm()
        {
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}