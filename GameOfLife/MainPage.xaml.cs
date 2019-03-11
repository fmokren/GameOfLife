using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private void Play(object sender, RoutedEventArgs e)
        {
            LifeCanvas.Play();
            if(LifeCanvas.IsPlaying)
            {
                PlayButton.Content = "Pause";
            }
            else
            {
                PlayButton.Content = "Play";
            }
        }

        private void Randomize(object send, RoutedEventArgs e)
        {
            LifeCanvas.Randomize();
        }

        private void Clear(object send, RoutedEventArgs e)
        {
            LifeCanvas.Clear();
        }
    }
}
