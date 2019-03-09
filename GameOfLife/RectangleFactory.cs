using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace GameOfLife
{
    public static class RectangleFactory
    {
        public static readonly double tileDimension = 15.0;
        public static readonly double minSeparation = 2.0;
        public static readonly Color aliveColor = Colors.DarkRed;
        public static readonly Color deadColor = Colors.AntiqueWhite;

        public static Rectangle CreateRectangle()
        {
            Rectangle rectangle = new Rectangle()
            {
                Width = tileDimension,
                Height = tileDimension,
                Fill = new SolidColorBrush(deadColor),
                Tag = false
            };

            rectangle.Tapped += 
                (s, te) => {
                    bool state = !(bool)rectangle.Tag;
                    rectangle.Tag = state;
                    rectangle.Fill = new SolidColorBrush(state ? aliveColor : deadColor);
                };

            return rectangle;
        }
    }
}
