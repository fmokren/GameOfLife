using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace GameOfLife
{
    public class LifeCanvas : Canvas
    {
        private LifeBoard board;
        private int columns;
        private int rows;
        public bool IsPlaying { get; private set; }

        public LifeCanvas() : base()
        {
            IsPlaying = false;
            SizeChanged += SizeChangedHandler;
        }

        private void SizeChangedHandler(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            Children.Clear();
            columns = NumberOfTilesPerDimension(e.NewSize.Width);
            rows = NumberOfTilesPerDimension(e.NewSize.Height);

            if (board == null)
            {
                board = new LifeBoard(columns, rows);
            }
            else
            {
                board = new LifeBoard(board, columns, rows);
            }

            double colSeparation = SeparationWidth(e.NewSize.Width, columns);
            double rowSeparation = SeparationWidth(e.NewSize.Height, rows);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < columns; c++)
                {
                    Rectangle rectangle = RectangleFactory.CreateRectangle();
                    rectangle.SetModel(board, c, r);

                    Children.Add(rectangle);

                    Canvas.SetLeft(rectangle, c * (RectangleFactory.tileDimension + colSeparation));
                    Canvas.SetTop(rectangle, r * (RectangleFactory.tileDimension + rowSeparation));
                }
            }
        }

        private static int NumberOfTilesPerDimension(double dimension)
        {
            return (int)Math.Floor((dimension - RectangleFactory.minSeparation) / (RectangleFactory.tileDimension + RectangleFactory.minSeparation));
        }

        private static double SeparationWidth(double dimension, int numberOfTiles)
        {
            double delta = dimension - ((RectangleFactory.tileDimension * numberOfTiles) + (RectangleFactory.minSeparation * (numberOfTiles - 1)));
            return (delta / (numberOfTiles - 1)) + RectangleFactory.minSeparation;
        }

        public void Play()
        {
            if(board == null)
            {
                return;
            }

            if (IsPlaying)
            {
                IsPlaying = false;
            }
            else
            {
                IsPlaying = true;

                Task.Run(() =>
                {
                    while (IsPlaying)
                    {
                        board.PlayRound();
                        WorkerUpdate();
                        Task.Delay(500).Wait();
                    }
                });
            }
        }

        public void Randomize()
        {
            if(board == null)
            {
                return;
            }
            Task.Run(() =>
            {
                board.Randomize();

                WorkerUpdate();
            });
        }

        private void WorkerUpdate()
        {
            Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, UpdateCanvas).AsTask().Wait();
        }

        public void UpdateCanvas()
        {
            foreach (Rectangle r in Children)
            {
                r.UpdateBrush();
            }
        }

        public void Clear()
        {
            if(board == null)
            {
                return;
            }

            board.Clear();
            UpdateCanvas();
        }
    }
}
