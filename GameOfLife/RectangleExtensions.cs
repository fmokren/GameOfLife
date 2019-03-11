using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Shapes;

namespace GameOfLife
{
    public class RectangleTag
    {
        public LifeBoard Board { get; set; }
        public int Column { get; set; }
        public int Row { get; set; }
    }
    public static class RectangleExtensions
    {
        public static void SetModel(this Rectangle r, LifeBoard board, int column, int row)
        {
            r.Tag = new RectangleTag()
            {
                Board = board,
                Column = column,
                Row = row
            };
        }

        public static LifeBoard Board(this Rectangle r)
        {
            return ((RectangleTag)r.Tag).Board;
        }

        public static int Column(this Rectangle r)
        {
            return ((RectangleTag)r.Tag).Column;
        }

        public static int Row(this Rectangle r)
        {
            return ((RectangleTag)r.Tag).Row;
        }

        public static void UpdateBrush(this Rectangle r)
        {
            RectangleTag t = (RectangleTag)r.Tag;
            r.Fill = t.Board[t.Column, t.Row] ? RectangleFactory.aliveBrush : RectangleFactory.deadBrush;
        }

        public static void Toggle(this Rectangle r)
        {
            RectangleTag t = (RectangleTag)r.Tag;
            t.Board[t.Column, t.Row] = !t.Board[t.Column, t.Row];
            r.UpdateBrush();
        }
    }
}
