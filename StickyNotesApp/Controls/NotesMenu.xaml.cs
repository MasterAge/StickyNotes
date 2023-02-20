using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using StickyNotesApp.Utils;
using wpfExt;
using Xceed.Wpf.Toolkit;

namespace StickyNotesApp.Controls
{
    public partial class NotesMenu : UserControl
    {
        public NotesMenu()
        {
            InitializeComponent();
        }

        public delegate void CloseMenuHandler(object sender, EventArgs data);

        public event CloseMenuHandler CloseMenu;

        private void OnCloseMenu(object sender, EventArgs data)
        {
            CloseMenu?.Invoke(this, data);
        }

        public delegate void DeleteNoteHandler(object sender, EventArgs data);

        public event DeleteNoteHandler DeleteNote;

        public void OnDeleteNote(object sender, RoutedEventArgs data)
        {
            DeleteNote?.Invoke(this, data);
        }

        public delegate void ShowHistoryHandler(object sender, EventArgs data);

        public event ShowHistoryHandler ShowHistory;

        public void OnShowHistory(object sender, RoutedEventArgs data)
        {
            ShowHistory?.Invoke(this, data);
        }

        public delegate void SetColorHandler(object sender, SolidColorBrush color);

        public event SetColorHandler SetColor;

        public void OnSetColor(object sender, RoutedEventArgs data)
        {
            if (data.Source.GetType() == typeof(ButtonEx))
            {
                SetColor?.Invoke(this, (sender as ButtonEx)?.Background as SolidColorBrush);
            }
        }

        private void OnColorPickerClosed(object sender, RoutedEventArgs e)
        {
            var color = (sender as ColorPicker)?.SelectedColor;
            if (color != null)
            {
                SetColor?.Invoke(this, ColorUtil.HexToBrush(color.ToString()));
            }
        }
    }
}