using System;
using System.Collections.Generic;
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
using System.IO;
using System.Diagnostics;
using System.Windows.Media.Animation;

namespace StartUpWindow
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StringBuilder sb = new StringBuilder();
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Mod256_Checked(object sender, RoutedEventArgs e)
        {
            sb.Clear();
            string root = Directory.GetCurrentDirectory();
            DirectoryInfo OneAbove = Directory.GetParent(root);
            DirectoryInfo OneAbove1 = Directory.GetParent(OneAbove.FullName);
            DirectoryInfo OneAbove2 = Directory.GetParent(OneAbove1.FullName);
            sb.Append(OneAbove2.FullName + @"\Radar256\bin\Debug\Radar256.exe");
           
        }

        private void Mod512_Checked(object sender, RoutedEventArgs e)
        {
            sb.Clear();
            string root = Directory.GetCurrentDirectory();
            DirectoryInfo OneAbove = Directory.GetParent(root);
            DirectoryInfo OneAbove1 = Directory.GetParent(OneAbove.FullName);
            DirectoryInfo OneAbove2 = Directory.GetParent(OneAbove1.FullName);
            sb.Append(OneAbove2.FullName + @"\Radar512\bin\Debug\Radar512.exe");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Process.Start(sb.ToString());
            Application.Current.Shutdown();
        }
       
        Canvas KontejnerElipse = new Canvas();
       

        private double[] OkretanjeZa30stepeni(double R, double ugao)
        {
            double rezX = R * Math.Sin(2 * Math.PI * (90 + ugao) / 360) + 360;
            double rezY = R * Math.Cos(2 * Math.PI * (90 + ugao) / 360) + 360;
            return new double[] { rezX, rezY };
        }
        private Storyboard st1 = new Storyboard();
        private void Interfejs1Km()
        {
            double heigh = KontejnerElipse.Width;
            double width = KontejnerElipse.Width;
            double korak = KontejnerElipse.Width * 1000 / (512 * 45);
            KontejnerElipse.Children.Clear();
            GeometryGroup xaxis_geom = new GeometryGroup();
            int ugao = 0;
            int milisekunde = 0;
            LineGeometry line = new LineGeometry(new Point(OkretanjeZa30stepeni((KontejnerElipse.Width + KontejnerElipse.Width) / 4, ugao)[0], OkretanjeZa30stepeni((KontejnerElipse.Width + KontejnerElipse.Width) / 4, ugao)[1]), new Point(OkretanjeZa30stepeni(30, ugao)[0], OkretanjeZa30stepeni(30, ugao)[1]));
            xaxis_geom.Children.Add(line);
            ugao += 30;
            int i = 1;
            while (korak * i < width)
            {
                if (korak * i > width)
                {
                    return;
                }
                ////////////////////////ELipse//////////////////////////////
                Ellipse el = new Ellipse();
                
                el.Height = heigh - korak * i;
                el.Width = width - korak * i;
                el.Stroke = Brushes.Black;
                el.StrokeThickness = 1;
                //Animacija za nestajnje sa pokazivaca
                DoubleAnimation animation = new DoubleAnimation();
                animation.To = 0.5;
                animation.From = 0;
                animation.Duration = TimeSpan.FromMilliseconds(700 - milisekunde);
                animation.AutoReverse = true;
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.EasingFunction = new QuadraticEase();
                el.Opacity = 0;
                el.Visibility = Visibility.Visible;
                Storyboard.SetTarget(animation, el);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
                st1.Children.Add(animation);
                //ovde se zavrsava
                KontejnerElipse.Children.Add(el);
                //Kontejner.Children.Add(KontejnerElipse);
                Canvas.SetLeft(el, i * korak / 2);
                Canvas.SetTop(el, i * korak / 2);
                ///////////////////////////////////////////////////////////
                milisekunde += 1;
                i++;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            GlavniProzor.Background = Pozadina.PozadinaGlavnogProzora();
            GlavniGrid.Children.Add(KontejnerElipse);
            KontejnerElipse.Opacity = 0.5;
            Grid.SetRow(KontejnerElipse, 1);
            KontejnerElipse.HorizontalAlignment = HorizontalAlignment.Center;
            KontejnerElipse.VerticalAlignment = VerticalAlignment.Center;
            KontejnerElipse.Width = 1300;
            KontejnerElipse.Height = 1300;
            Interfejs1Km();
            st1.Begin(this, true);
            
        }
    }
}
