using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GameOfLife
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        

        public MainPage()
        {
            this.InitializeComponent();
        }

        private void LifeCanvas_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var p = e.GetPosition(this.LifeCanvas);
        }

        private void LifeCanvas_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            LifeCanvas.Children.Clear();
            int numColumns = NumberOfTilesPerDimension(e.NewSize.Width);
            int numRows = NumberOfTilesPerDimension(e.NewSize.Height);

            double colSeparation = SeparationWidth(e.NewSize.Width, numColumns);
            double rowSeparation = SeparationWidth(e.NewSize.Height, numRows);

            for (int r = 0; r < numRows; r++)
            {
                for (int c = 0; c < numColumns; c++)
                {
                    Rectangle rectangle = RectangleFactory.CreateRectangle();

                    LifeCanvas.Children.Add(rectangle);

                    Canvas.SetLeft(rectangle, c * (RectangleFactory.tileDimension + colSeparation));
                    Canvas.SetTop(rectangle, r * (RectangleFactory.tileDimension + rowSeparation));
                }
            }
        }

        private void MainPage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            throw new NotImplementedException();
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
    }
}
