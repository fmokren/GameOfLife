using System;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace GameOfLife
{
    public class LifeCanvas : Canvas
    {
        private LifeBoard board;
        private int columns = 0;
        private int rows = 0;

        private Rectangle[,] controls;
        public bool IsPlaying { get; private set; }

        public LifeCanvas() : base()
        {
            IsPlaying = false;
            SizeChanged += SizeChangedHandler;
        }

        private void SizeChangedHandler(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            int columnsN = NumberOfTilesPerDimension(e.NewSize.Width);
            int rowsN = NumberOfTilesPerDimension(e.NewSize.Height);

            if (board == null)
            {
                board = new LifeBoard(columnsN, rowsN);
            }
            else
            {
                board = new LifeBoard(board, columnsN, rowsN);
            }

            Rectangle[,] controlsN = new Rectangle[columnsN, rowsN];

            double colSeparation = SeparationWidth(e.NewSize.Width, columnsN);
            double rowSeparation = SeparationWidth(e.NewSize.Height, rowsN);

            for (int r = 0; r < Math.Max(rows, rowsN); r++)
            {
                for (int c = 0; c < Math.Max(columns, columnsN); c++)
                {
                    if (r < rows && c < columns)
                    {
                        if (r < rowsN && c < columnsN)
                        {
                            Rectangle rectangle = controls[c, r];
                            rectangle.SetModel(board, c, r);

                            Canvas.SetLeft(rectangle, c * (RectangleFactory.tileDimension + colSeparation));
                            Canvas.SetTop(rectangle, r * (RectangleFactory.tileDimension + rowSeparation));

                            controlsN[c, r] = rectangle;
                        }
                        else
                        {
                            Children.Remove(controls[c, r]);
                        }
                    }
                    else
                    {
                        Rectangle rectangle = RectangleFactory.CreateRectangle();

                        Children.Add(rectangle);
                        rectangle.SetModel(board, c, r);

                        Canvas.SetLeft(rectangle, c * (RectangleFactory.tileDimension + colSeparation));
                        Canvas.SetTop(rectangle, r * (RectangleFactory.tileDimension + rowSeparation));

                        controlsN[c, r] = rectangle;
                    }
                }
            }

            controls = controlsN;
            columns = columnsN;
            rows = rowsN;
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
