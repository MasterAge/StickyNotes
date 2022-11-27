using System;
using System.Security;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StickyNotesApp.Controls
{
    public partial class NotesButton : UserControl
    {
        public string Text { get; set; }
        
        public SolidColorBrush HoverColor { get; set; }
        
        public NotesButton()
        {
            InitializeComponent();
            DataContext = this;
        }

        public delegate void ButtonClickHandler(object sender, EventArgs data);

        public event ButtonClickHandler ButtonClicked;

        private void OnButtonClick(object sender, EventArgs data)
        {
            ButtonClicked?.Invoke(this, data);
        }
    }
}