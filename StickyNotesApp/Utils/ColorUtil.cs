using System.Windows.Media;

namespace StickyNotesApp.Utils
{
    public static class ColorUtil
    {
        public const int DarkColorCutoff = 50;
        
        public static SolidColorBrush DarkenBackground(SolidColorBrush background)
        {
            var darkerColor = Color.Multiply(background.Color, 0.75f);
            darkerColor.A = 255;
            return new SolidColorBrush(darkerColor);
        }
        
        public static SolidColorBrush SmartDarkenBackground(SolidColorBrush background)
        {
            var darkerColor = Color.Multiply(background.Color, 0.75f);
            if (darkerColor.R < DarkColorCutoff && darkerColor.G < DarkColorCutoff && darkerColor.B < DarkColorCutoff)
            {
                darkerColor.R = (byte)(255 - darkerColor.R);
                darkerColor.G = (byte)(255 - darkerColor.G);
                darkerColor.B = (byte)(255 - darkerColor.B);
            }
            darkerColor.A = 255;
            return new SolidColorBrush(darkerColor);
        }

        public static SolidColorBrush HexToBrush(string hexCode)
        {
            return (SolidColorBrush)new BrushConverter().ConvertFrom(hexCode);
        }
    }
}