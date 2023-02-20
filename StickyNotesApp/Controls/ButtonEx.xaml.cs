using System.Windows;
using System.Windows.Media;
using StickyNotesApp.Utils;

namespace wpfExt
{
    /// <summary>
    /// Standard button with extensions
    /// </summary>
    public partial class ButtonEx
    {
        private static readonly Brush DefaultHoverBackgroundValue = ColorUtil.HexToBrush("#FFBEE6FD");

        public ButtonEx()
        {
            InitializeComponent();
        }

        public Brush HoverBackground
        {
            get => (Brush)GetValue(HoverBackgroundProperty);
            set => SetValue(HoverBackgroundProperty, value);
        }

        public static readonly DependencyProperty HoverBackgroundProperty = DependencyProperty.Register(
            "HoverBackground", typeof(Brush), typeof(ButtonEx), new PropertyMetadata(DefaultHoverBackgroundValue));

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (e.Property == BackgroundProperty && Background != null)
            {
                HoverBackground = ColorUtil.DarkenBackground((SolidColorBrush)Background);
            }

            base.OnPropertyChanged(e);
        }
    }
}