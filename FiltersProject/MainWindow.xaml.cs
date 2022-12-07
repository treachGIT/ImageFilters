using Microsoft.Win32;

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Accord.Imaging.Filters;
using System.Windows.Interop;

namespace FiltersProject
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Bitmap currentBitmap;

        private IFilter currentFilter;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadFile(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = "Select a picture";
            op.Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
              "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
              "Portable Network Graphic (*.png)|*.png";

            if (op.ShowDialog() == true)
            {
                BaseImage.Source = new BitmapImage(new Uri(op.FileName));
            }

            currentBitmap = new Bitmap(op.FileName);

            currentFilter = new Mirror(false, false);
        }

        public void ExecuteFilter(object sender, RoutedEventArgs e)
        {
            Bitmap filteredBitmap = currentFilter.Apply(currentBitmap);

            FilteredImage.Source = ImageSourceFromBitmap(filteredBitmap);
        }

        public ImageSource ImageSourceFromBitmap(Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch
            {
                throw new InvalidCastException();
            }
        }

        ////////////////////////////////////////////////////////////////////////////

        private void InvertSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new Invert();
        }

        private void JitterSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new Jitter(4);        
        }

        private void KirschEdgeDetectorSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new KirschEdgeDetector();
        }

        private void MirrorSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new Mirror(false, true);
        }

        private void WaterWaveSelected(object sender, RoutedEventArgs e)
        {
            WaterWave filter = new WaterWave();
            filter.HorizontalWavesCount = 10;
            filter.HorizontalWavesAmplitude = 5;
            filter.VerticalWavesCount = 3;
            filter.VerticalWavesAmplitude = 15;

            currentFilter = filter;
        }

        private void SepiaSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new Sepia();
        }

        private void RotateSelected(object sender, RoutedEventArgs e)
        {
            RotationField.Visibility = Visibility.Visible;

            currentFilter = new RotateNearestNeighbor(45, true);
        }

        private void OilPaintingSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new OilPainting(40);
        }

        private void PixellateSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new Pixellate();
        }

        private void SimplePosterizationSelected(object sender, RoutedEventArgs e)
        {
            currentFilter = new SimplePosterization();
        }
   
        ////////////////////////////////////////////////////////////////////////////

        private void RotateUnselected(object sender, RoutedEventArgs e)
        {
            RotationField.Visibility = Visibility.Collapsed;
        }

        ////////////////////////////////////////////////////////////////////////////

        private void RotationChanged(object sender, TextChangedEventArgs e)
        {
            double value;
            try
            {
                value = double.Parse(RotationValue.Text);
            }
            catch
            {
                value = 45;
            }

            currentFilter = new RotateNearestNeighbor(value, true);
        }
    }
}