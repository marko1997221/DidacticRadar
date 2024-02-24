using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Radar256
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {



        public MainWindow()
        {
            InitializeComponent();
        }
        #region  Prvi deo izgleda KOntejner2 on je najveci
        // Zaduzen je za pozicioniranje obelezivaca ugla i takodje ima pozadinu u kojoj stvara okvir
        Canvas Kontejner2 = new Canvas();

        //funkcija za inicijalizaciju Kontejner2

        private void Inicijalizacija_Kontejner2()
        {
            Kontejner2.Width = MestoZaElipsu.ActualWidth + MarginaLevaIvica.ActualWidth + MarginaSredina.ActualWidth;
            Kontejner2.Height = MestoZaElipsu.ActualWidth + MarginaLevaIvica.ActualWidth + MarginaSredina.ActualWidth;
            Grid.Children.Add(Kontejner2);
            Grid.SetColumn(Kontejner2, 0);
            Grid.SetRow(Kontejner2, 0);
            Grid.SetRowSpan(Kontejner2, 8);
            Grid.SetColumnSpan(Kontejner2, 6);
        }
        #endregion

        #region  Ova elipsa sluzi samo za iscrtavanje pozadine okvira Pokazivac 2 Elipsa
        /////////////////////////////////////////////////////////////////////////////////////////////
        //Ova elipsa sluzi samo za iscrtavanje pozadine okvira
        Ellipse Pokazivac2 = new Ellipse();

        //funkcija za inicijalizaciju Pokazivac2

        private void Inicijalizacija_Pokazivac2()
        {
            Pokazivac2.Width = MestoZaElipsu.ActualWidth + MarginaLevaIvica.ActualWidth + MarginaSredina.ActualWidth;
            Pokazivac2.Height = MestoZaElipsu.ActualWidth + MarginaLevaIvica.ActualWidth + MarginaSredina.ActualWidth;
            Pokazivac2.Fill = Brushes.Gray;
            Pokazivac2.Opacity = 0.05;
            Grid.Children.Add(Pokazivac2);
            Grid.SetColumn(Pokazivac2, 0);
            Grid.SetRow(Pokazivac2, 0);
            Grid.SetRowSpan(Pokazivac2, 7);
            Grid.SetColumnSpan(Pokazivac2, 6);
        }
        #endregion

        #region KOntejner Ovo je kontejner koji je zaduzen da za definisanje pozadine  Takodje u sebi sadrzi KontejnerElipse 

        /// </summary>
        //Ovo je kontejner koji je zaduzen da za definisanje pozadine  Takodje u sebi sadrzi KontejnerElipse 
        Canvas Kontejner = new Canvas();

        private void Inicijalizacija_Kontejner()
        {
            Kontejner.Width = MestoZaElipsu.ActualWidth;
            Kontejner.Height = MestoZaElipsu.ActualWidth;
            Kontejner.Background = PozadinaWindow.PozadinaGlavnogProzora();
            Grid.Children.Add(Kontejner);
            Grid.SetRow(Kontejner, 1);
            Grid.SetRowSpan(Kontejner, 4);
            Grid.SetColumn(Kontejner, 1);
            Grid.SetColumnSpan(Kontejner, 4);
        }
        #endregion

        #region Region za inicijalizaciju VremenskeBaze

        private void Inicijalizacija_VremenskeBaze()
        {

            Glavni.Children.Remove(VremenaskaBaza);
            VremenaskaBaza.X1 = 0;
            VremenaskaBaza.X2 = Kontejner.Width / 2;
            VremenaskaBaza.Y1 = Kontejner.Height / 2;
            VremenaskaBaza.Y2 = Kontejner.Height / 2;
            VremenaskaBaza.Stroke = Brushes.White;
            VremenaskaBaza.StrokeThickness = 1;
            VremenaskaBaza.HorizontalAlignment = HorizontalAlignment.Center;
            VremenaskaBaza.VerticalAlignment = VerticalAlignment.Center;
            VremenaskaBaza.RenderTransformOrigin = new Point(0.435, 1);
            Kontejner.Children.Add(VremenaskaBaza);

        }
        #endregion

        #region Region za inicijalizaciju Elipsi i Kontejnera za Obelezivace Daljine
        /////////////////////////////////////////////////////////////////////////////////////////////
        //Ovo je konejner koji je zaduzen za pokazivace daljine sve ih mesta tu
        //odredjuje njihovu posziciju i interakciju
        Canvas KontejnerElipse = new Canvas();

        private void Inicijalizacija_KontejnerElipse()
        {
            KontejnerElipse.Width = MestoZaElipsu.ActualWidth + (MarginaLevaIvica.ActualWidth + MarginaSredina.ActualWidth);
            KontejnerElipse.Height = MestoZaElipsu.ActualWidth + (MarginaLevaIvica.ActualWidth + MarginaSredina.ActualWidth);

            Kontejner.Children.Add(KontejnerElipse);
        }

        ///  Generisanje obelezivaca Ugla
        ///  
        private void ObelezivaciUgla()
        {
            int ugao = 0;
            double inkerement = 1;
            GeometryGroup xaxis_geom = new GeometryGroup();

            for (int i = 1; i < 13; i++)
            {
                TextBlock t = new TextBlock();
                t.Text = ugao.ToString();
                t.FontSize = 12;
                t.Foreground = Brushes.White;
                t.Opacity = 0.7;
                LineGeometry line1 = new LineGeometry(new Point(OkretanjeZa30stepeni((Pokazivac2.Width + Pokazivac.Width) / 4, ugao)[0], OkretanjeZa30stepeni((Pokazivac2.Width + Pokazivac.Width) / 4, ugao)[1]), new Point(OkretanjeZa30stepeni(30, ugao)[0], OkretanjeZa30stepeni(30, ugao)[1]));

                xaxis_geom.Children.Add(line1);
                Kontejner2.Children.Add(t);
                if (i >= 5)
                {
                    Canvas.SetTop(t, OkretanjeZa30stepeniTekst((Pokazivac2.Width + Pokazivac.Width) / 4, -ugao + 90 - inkerement)[1]);
                    Canvas.SetLeft(t, OkretanjeZa30stepeniTekst((Pokazivac2.Width + Pokazivac.Width) / 4, -ugao + 90 - inkerement)[0]);
                    inkerement -= 0.1;
                }
                else
                {
                    Canvas.SetTop(t, OkretanjeZa30stepeniTekst((Pokazivac2.Width + Pokazivac.Width) / 4, -ugao + 90)[1]);
                    Canvas.SetLeft(t, OkretanjeZa30stepeniTekst((Pokazivac2.Width + Pokazivac.Width) / 4, -ugao + 90)[0]);

                }


                ugao += 30;
            }
            System.Windows.Shapes.Path xaxis_path = new System.Windows.Shapes.Path();
            DoubleAnimation animation1 = new DoubleAnimation();
            animation1.To = 0.2;
            animation1.From = 0;
            animation1.Duration = TimeSpan.FromMilliseconds(10000);
            animation1.AutoReverse = true;
            animation1.RepeatBehavior = RepeatBehavior.Forever;
            animation1.EasingFunction = new QuadraticEase();

            Storyboard sb1 = new Storyboard();
            xaxis_path.Opacity = 0;
            xaxis_path.Visibility = Visibility.Visible;
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;
            Kontejner2.Children.Add(xaxis_path);
            Storyboard.SetTarget(animation1, xaxis_path);
            Storyboard.SetTargetProperty(animation1, new PropertyPath(Control.OpacityProperty));
            sb1.Children.Add(animation1);
            sb1.Begin(this);

        }
        //funkcije za generisanje
        private double[] OkretanjeZa30stepeniTekst(double R, double ugao)
        {
            double rezX = R * Math.Sin(2 * Math.PI * (90 + ugao) / 360) + 355;
            double rezY = R * Math.Cos(2 * Math.PI * (90 + ugao) / 360) + 350;
            return new double[] { rezX, rezY };
        }
        private double[] OkretanjeZa30stepeni(double R, double ugao)
        {
            double rezX = R * Math.Sin(2 * Math.PI * (90 + ugao) / 360) + 360;
            double rezY = R * Math.Cos(2 * Math.PI * (90 + ugao) / 360) + 360;
            return new double[] { rezX, rezY };
        }
        ///Inicijjalizacija interfejsa za Obelezivace Daljine
        ///
        private void Interfejs1Km()
        {
            double heigh = Pokazivac.Width;
            double width = Pokazivac.Width;
            double korak = Pokazivac.Width * 1000 / (ModRada * 45);
            KontejnerElipse.Children.Clear();
            GeometryGroup xaxis_geom = new GeometryGroup();
            int ugao = 0;
            int milisekunde = 0;
            LineGeometry line = new LineGeometry(new Point(OkretanjeZa30stepeni((Pokazivac2.Width + Pokazivac.Width) / 4, ugao)[0], OkretanjeZa30stepeni((Pokazivac2.Width + Pokazivac.Width) / 4, ugao)[1]), new Point(OkretanjeZa30stepeni(30, ugao)[0], OkretanjeZa30stepeni(30, ugao)[1]));
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

        private void Interfejs2Km()
        {

            double heigh = Pokazivac.Width;
            double width = Pokazivac.Width;
            double korak = Pokazivac.Width * 2000 / (ModRada * 45);
            KontejnerElipse.Children.Clear();
            GeometryGroup xaxis_geom = new GeometryGroup();
            int ugao = 0;
            int milisiekunde = 0;
            List<Ellipse> ListaElipsi = new List<Ellipse>();
            List<Storyboard> ListaStory = new List<Storyboard>();
            LineGeometry line = new LineGeometry(new Point(OkretanjeZa30stepeni(350, ugao)[0], OkretanjeZa30stepeni(350, ugao)[1]), new Point(OkretanjeZa30stepeni(45, ugao)[0], OkretanjeZa30stepeni(45, ugao)[1]));
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
                animation.Duration = TimeSpan.FromMilliseconds(1000 - milisiekunde);
                animation.AutoReverse = true;
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.EasingFunction = new QuadraticEase();
                el.Opacity = 0;
                Storyboard.SetTarget(animation, el);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
                st2.Children.Add(animation);

                //ovde se zavrsava
                KontejnerElipse.Children.Add(el);
                Canvas.SetLeft(el, i * korak / 2);
                Canvas.SetTop(el, i * korak / 2);
                ///////////////////////////////////////////////////////////
                milisiekunde += 10;
                i++;

            }

        }

        private void Interfejs5Km()
        {

            double heigh = Pokazivac.Width;
            double width = Pokazivac.Width;
            double korak = Pokazivac.Width * 5000 / (ModRada * 45);
            KontejnerElipse.Children.Clear();

            int milisekunde = 0;
            GeometryGroup xaxis_geom = new GeometryGroup();
            int ugao = 0;
            LineGeometry line = new LineGeometry(new Point(OkretanjeZa30stepeni(350, ugao)[0], OkretanjeZa30stepeni(350, ugao)[1]), new Point(OkretanjeZa30stepeni(45, ugao)[0], OkretanjeZa30stepeni(45, ugao)[1]));
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
                animation.Duration = TimeSpan.FromMilliseconds(1000 - milisekunde);
                animation.AutoReverse = true;
                animation.RepeatBehavior = RepeatBehavior.Forever;
                animation.EasingFunction = new QuadraticEase();
                el.Opacity = 0;
                Storyboard.SetTarget(animation, el);
                Storyboard.SetTargetProperty(animation, new PropertyPath(Control.OpacityProperty));
                st3.Children.Add(animation);

                //ovde se zavrsava
                KontejnerElipse.Children.Add(el);
                Canvas.SetLeft(el, i * korak / 2);
                Canvas.SetTop(el, i * korak / 2);
                ///////////////////////////////////////////////////////////
                milisekunde += 50;
                i++;

            }
        }

        ///Generis

        #endregion

        #region Region Za inicijlazicaiju Buttuna Za aktiviranje Obelezivaca Daljine

        /// <summary>
        /// Koriste se za aktivacijju animacija obelezivaca daljine koje sam generisao
        /// pomocu njih se kontrolise animacija
        /// </summary>
        private Storyboard st1 = new Storyboard();
        private Storyboard st2 = new Storyboard();
        private Storyboard st3 = new Storyboard();


        Button button1_sektorDaljina = new Button();  //Obelezivac na 1Km

        private void Button1_sektorDaljina_Click(object sender, RoutedEventArgs e)
        {
            if (button1_sektorDaljina.Tag == null)
            {
                button1_sektorDaljina.Tag = "clicked";
                button2_sektorDaljina.Tag = null;
                button3_sektorDaljina.Tag = null;
                Interfejs1Km();
                st3.Stop(this);
                st2.Stop(this);
                st1.Begin(this, true);
            }
            else
            {
                button1_sektorDaljina.Tag = null;
                st1.Stop(this);
                st2.Stop(this);
                st3.Stop(this);
            }

        }

        Button button2_sektorDaljina = new Button();  //Obelezivac na 2km

        private void Button2_sektorDaljina_Click(object sender, RoutedEventArgs e)
        {
            if (button2_sektorDaljina.Tag == null)
            {
                button2_sektorDaljina.Tag = "clicked";
                button3_sektorDaljina.Tag = null;
                button1_sektorDaljina.Tag = null;
                Interfejs2Km();
                st1.Stop(this);
                st3.Stop(this);
                st2.Begin(this, true);
            }
            else
            {
                button2_sektorDaljina.Tag = null;
                st1.Stop(this);
                st2.Stop(this);
                st3.Stop(this);
            }

        }

        Button button3_sektorDaljina = new Button();  //Obelezivac na 5km

        private void Button3_sektorDaljina_Click(object sender, RoutedEventArgs e)
        {
            if (button3_sektorDaljina.Tag == null)
            {
                button3_sektorDaljina.Tag = "clicked";
                button2_sektorDaljina.Tag = null;
                button1_sektorDaljina.Tag = null;
                Interfejs5Km();
                st1.Stop(this);
                st2.Stop(this);
                st3.Begin(this, true);
            }
            else
            {
                button3_sektorDaljina.Tag = null;
                st1.Stop(this);
                st2.Stop(this);
                st3.Stop(this);
            }


        }

        private void Inicijalizacija_Buttona_Za_Obelezavanje_Daljine()
        {
            //  dodajemo buttone Glavnom gridu
            button1_sektorDaljina.Click += Button1_sektorDaljina_Click;
            button1_sektorDaljina.Content = "1km";
            button1_sektorDaljina.Width = MarginaLevaIvica.ActualWidth;
            button1_sektorDaljina.Height = CetvrtiRed.ActualHeight;
            button1_sektorDaljina.FontSize = 10;
            button1_sektorDaljina.Background = Brushes.White;
            button1_sektorDaljina.Width = 30;
            button1_sektorDaljina.Height = 30;
            button1_sektorDaljina.Margin = new Thickness(0, 0, 0, 0);
            button1_sektorDaljina.VerticalAlignment = VerticalAlignment.Top;
            button1_sektorDaljina.HorizontalAlignment = HorizontalAlignment.Left;
            Glavni.Children.Add(button1_sektorDaljina);
            Grid.SetColumn(button1_sektorDaljina, 0);
            Grid.SetRow(button1_sektorDaljina, 0);
            //    dodajemo  buttone Glavnom gridu
            button2_sektorDaljina.Content = "2km";
            button2_sektorDaljina.Click += Button2_sektorDaljina_Click;
            button2_sektorDaljina.Width = MarginaLevaIvica.ActualWidth;
            button2_sektorDaljina.Height = CetvrtiRed.ActualHeight;
            button2_sektorDaljina.FontSize = 10;
            button2_sektorDaljina.Background = Brushes.White;
            button2_sektorDaljina.Width = 30;
            button2_sektorDaljina.Height = 30;
            button2_sektorDaljina.Margin = new Thickness(50, 0, 0, 0);
            button2_sektorDaljina.VerticalAlignment = VerticalAlignment.Top;
            button2_sektorDaljina.HorizontalAlignment = HorizontalAlignment.Left;
            Glavni.Children.Add(button2_sektorDaljina);

            //  dodajemo  buttone Glavnom gridu
            button3_sektorDaljina.Content = "5km";
            button3_sektorDaljina.Click += Button3_sektorDaljina_Click;
            button3_sektorDaljina.Width = MarginaLevaIvica.ActualWidth;
            button3_sektorDaljina.Height = CetvrtiRed.ActualHeight;
            button3_sektorDaljina.FontSize = 10;
            button3_sektorDaljina.Background = Brushes.White;
            button3_sektorDaljina.Width = 30;
            button3_sektorDaljina.Height = 30;
            button3_sektorDaljina.Margin = new Thickness(100, 0, 0, 0);
            button3_sektorDaljina.VerticalAlignment = VerticalAlignment.Top;
            button3_sektorDaljina.HorizontalAlignment = HorizontalAlignment.Left;
            Glavni.Children.Add(button3_sektorDaljina);
        }

        #endregion

        #region Region za Elipsu za granicu pokazivaca
        //Ova elipsa sluzi samo da nacrta elipsku sa strokom bele boje 
        Ellipse Pokazivac = new Ellipse();

        private void Inicijalizacija_Pokazivac()
        {
            Pokazivac.Width = MestoZaElipsu.ActualWidth;
            Pokazivac.Height = MestoZaElipsu.ActualWidth;
            Pokazivac.StrokeThickness = 0.8;
            Pokazivac.Opacity = 0.3;
            Pokazivac.Stroke = Brushes.White;
            Grid.Children.Add(Pokazivac);
            Grid.SetColumn(Pokazivac, 1);
            Grid.SetRow(Pokazivac, 1);
            Grid.SetRowSpan(Pokazivac, 4);
            Grid.SetColumnSpan(Pokazivac, 4);
        }
        /// </summary>

        #endregion

        #region Funckija za ispis Grafika i njegova inicijalizacija
        public Polyline polyline = new Polyline();
        public Polyline polyline2 = new Polyline();
        public int counter = 0;

        public void Ispis(object[] data)
        {
            object o = data[0] as object;

            double[,] red = o as double[,];
            if (red != null)
            {
                const double margin = 20;
                double xmin = margin;
                double xmax = SirinaKolon2.ActualWidth - margin;
                double ymin = margin;
                double ymax = VisinaReda1.ActualHeight - margin;
                double step = xmax / ModRada;
                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Background, (ThreadStart)delegate ()
                {

                    Brush[] brushes = new Brush[] { Brushes.White, Brushes.Red };
                    PointCollection points = new PointCollection();
                    PointCollection points2 = new PointCollection();
                    int i = 0;
                    double granica = 100;
                    for (double x = xmin; x < xmax; x += step)
                    {
                        double last_y = ((red[0, i])) / 10000;

                        if (last_y < ymin) last_y = (int)ymin;
                        if (last_y > ymax) last_y = (int)ymax;
                        points.Add(new Point(x, ymax - last_y));
                        points2.Add(new Point(x, ymax - granica));
                        i++;
                    }
                    polyline.StrokeThickness = 0.5;
                    polyline.Stroke = brushes[0];
                    polyline.Points = points;
                    polyline2.StrokeThickness = 0.5;
                    polyline2.Stroke = brushes[1];
                    polyline2.Points = points2;

                    if (counter == 0)
                    {


                        canGraph.Children.Add(polyline2);
                        canGraph.Children.Add(polyline);

                    }
                    counter++;
                });
            }


        }

        private void Inicijalizacija_Grafika()
        {
            canGraph.Children.Clear();
            const double margin = 20;
            double xmin = margin;
            double xmax = SirinaKolon2.ActualWidth - margin;
            double ymax = VisinaReda1.ActualHeight - margin;
            double step = xmax / ModRada;
            canGraph.Width = SirinaKolon2.ActualWidth;
            canGraph.Height = VisinaReda1.ActualHeight;
            Rectangle pozadina = new Rectangle();
            pozadina.Width = canGraph.Width;
            pozadina.Height = canGraph.Height;
            pozadina.Fill = PozadinaWindow.WrapperGrafika();
            pozadina.Opacity = 0.5;
            pozadina.RadiusX = 20;
            pozadina.RadiusY = 20;
            canGraph.Children.Add(pozadina);
            Canvas.SetLeft(pozadina, 0);
            Canvas.SetTop(pozadina, 0);
            Rectangle diagonalFillRectangle = new Rectangle();
            diagonalFillRectangle.Width = canGraph.Width;
            diagonalFillRectangle.Height = canGraph.Height; // - 0.1 * canGraph.Height;
            diagonalFillRectangle.HorizontalAlignment = HorizontalAlignment.Center;
            diagonalFillRectangle.Fill = PozadinaWindow.PozadinaGrafika();
            diagonalFillRectangle.Opacity = 0.3;
            diagonalFillRectangle.RadiusY = 20;
            diagonalFillRectangle.RadiusX = 20;
            //




            // Create a diagonal linear gradient with four stops.

            // Use the brush to paint the rectangle.

            canGraph.Children.Add(diagonalFillRectangle);
            //Pravljenje X- ose
            // Make the X axis.
            GeometryGroup xaxis_geom = new GeometryGroup();
            xaxis_geom.Children.Add(new LineGeometry(
                new Point(0, ymax), new Point(canGraph.Width, ymax)));
            for (double x = xmin + step;
                x <= canGraph.Width - step; x += step)
            {
                xaxis_geom.Children.Add(new LineGeometry(
                    new Point(x, ymax - 4 / 2),
                    new Point(x, ymax + 4 / 2)));
            }

            System.Windows.Shapes.Path xaxis_path = new System.Windows.Shapes.Path();
            xaxis_path.StrokeThickness = 1;
            xaxis_path.Stroke = Brushes.Black;
            xaxis_path.Data = xaxis_geom;

            canGraph.Children.Add(xaxis_path);
            // pravimo Y osu
            // Make the Y ayis.
            GeometryGroup yaxis_geom = new GeometryGroup();
            yaxis_geom.Children.Add(new LineGeometry(
                new Point(xmin, 0), new Point(xmin, canGraph.Height)));
            for (double y = step; y <= canGraph.Height - step; y += step)
            {
                yaxis_geom.Children.Add(new LineGeometry(
                    new Point(xmin - 4 / 2, y),
                    new Point(xmin + 4 / 2, y)));
            }

            System.Windows.Shapes.Path yaxis_path = new System.Windows.Shapes.Path();
            yaxis_path.StrokeThickness = 1;
            yaxis_path.Stroke = Brushes.Black;
            yaxis_path.Data = yaxis_geom;

            canGraph.Children.Add(yaxis_path);
        }
        /// <summary>
        #endregion

        #region Definisanje DependecyPropertija za Animazciju promene Ugla Vremeske Baze
        ///Animacija kretanja Vremenske baze na pokazivacu
        public double Ugao
        {
            get { return (double)GetValue(AngleProperty); }
            set { SetValue(AngleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Angle.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty AngleProperty = DependencyProperty.Register("Angle", typeof(double), typeof(MainWindow), new UIPropertyMetadata(0.0, new PropertyChangedCallback(AngleChanged)));

        private static void AngleChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            MainWindow control = (MainWindow)sender;
            control.PerformAnimation((double)e.OldValue, (double)e.NewValue);
        }

        private void PerformAnimation(double oldValue, double newValue)
        {
            Storyboard s = new Storyboard();

            DoubleAnimation animation = new DoubleAnimation();
            animation.From = oldValue;
            animation.To = newValue;
            animation.Duration = new Duration(TimeSpan.FromMilliseconds(animacijaUgaoneBrzineUMS));
            s.Children.Add(animation);
            Storyboard.SetTarget(animation, VremenaskaBaza);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Line.RenderTransform).(RotateTransform.Angle)"));
            s.Begin();
        }
        #endregion

        #region Dogadjaji za Zumiranje
        // pomocne promenljive
        Point? lastCenterPositionOnTarget;
        Point? lastMousePositionOnTarget;
        Point? lastDragPoint;
        /// <summary>
        /// ///////////////////////////////////////////////////////////////// Deogadjaji za zumiranje///////////////////////////////
        /// </summary>
        void OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            scrollViewer.Cursor = Cursors.Arrow;
            scrollViewer.ReleaseMouseCapture();
            lastDragPoint = null;
        }
        void OnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            lastMousePositionOnTarget = Mouse.GetPosition(Grid);

            if (e.Delta > 0)
            {
                slider.Value += 1;
            }
            if (e.Delta < 0)
            {
                slider.Value -= 1;
            }

            e.Handled = true;
        }
        void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var mousePos = e.GetPosition(scrollViewer);
            if (mousePos.X <= scrollViewer.ViewportWidth && mousePos.Y <
                scrollViewer.ViewportHeight) //make sure we still can use the scrollbars
            {
                scrollViewer.Cursor = Cursors.SizeAll;
                lastDragPoint = mousePos;
                Mouse.Capture(scrollViewer);
            }
        }
        void OnMouseMove(object sender, MouseEventArgs e)
        {
            if (lastDragPoint.HasValue)
            {
                Point posNow = e.GetPosition(scrollViewer);

                double dX = posNow.X - lastDragPoint.Value.X;
                double dY = posNow.Y - lastDragPoint.Value.Y;

                lastDragPoint = posNow;

                scrollViewer.ScrollToHorizontalOffset(scrollViewer.HorizontalOffset - dX);
                scrollViewer.ScrollToVerticalOffset(scrollViewer.VerticalOffset - dY);
            }
        }
        void OnSliderValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            scaleTransform.ScaleX = e.NewValue;
            scaleTransform.ScaleY = e.NewValue;

            var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                             scrollViewer.ViewportHeight / 2);
            lastCenterPositionOnTarget = scrollViewer.TranslatePoint(centerOfViewport, Grid);
        }
        void OnScrollViewerScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            if (e.ExtentHeightChange != 0 || e.ExtentWidthChange != 0)
            {
                Point? targetBefore = null;
                Point? targetNow = null;

                if (!lastMousePositionOnTarget.HasValue)
                {
                    if (lastCenterPositionOnTarget.HasValue)
                    {
                        var centerOfViewport = new Point(scrollViewer.ViewportWidth / 2,
                                                         scrollViewer.ViewportHeight / 2);
                        Point centerOfTargetNow =
                              scrollViewer.TranslatePoint(centerOfViewport, Grid);

                        targetBefore = lastCenterPositionOnTarget;
                        targetNow = centerOfTargetNow;
                    }
                }
                else
                {
                    targetBefore = lastMousePositionOnTarget;
                    targetNow = Mouse.GetPosition(Grid);

                    lastMousePositionOnTarget = null;
                }

                if (targetBefore.HasValue)
                {
                    double dXInTargetPixels = targetNow.Value.X - targetBefore.Value.X;
                    double dYInTargetPixels = targetNow.Value.Y - targetBefore.Value.Y;

                    double multiplicatorX = e.ExtentWidth / Grid.Width;
                    double multiplicatorY = e.ExtentHeight / Grid.Height;

                    double newOffsetX = scrollViewer.HorizontalOffset -
                                        dXInTargetPixels * multiplicatorX;
                    double newOffsetY = scrollViewer.VerticalOffset -
                                        dYInTargetPixels * multiplicatorY;

                    if (double.IsNaN(newOffsetX) || double.IsNaN(newOffsetY))
                    {
                        return;
                    }

                    scrollViewer.ScrollToHorizontalOffset(newOffsetX);
                    scrollViewer.ScrollToVerticalOffset(newOffsetY);
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        #endregion

        #region Definisanje i generisanje poruke koja je potrebna da se posalje PLAFORMI radara
        public int Vertikala { get; set; }
        public int Horizontala { get; set; }


        private void SlajderZaBrzElevacije_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {

            TextBlockEle.Text = SlajderZaBrzElevacije.Value.ToString();
            fp.BrzinaB = (int)SlajderZaBrzElevacije.Value;
            fp.Kreni();
        }
        int vrednostSlajdera = 0;
        int animacijaUgaoneBrzineUMS = 0;
        private void SlajderZaBrzAzimuta_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            TextBlockAz.Text = SlajderZaBrzAzimuta.Value.ToString();
            fp.BrzinaA = (int)SlajderZaBrzAzimuta.Value;
            if (SlajderZaBrzAzimuta.Value==90)
            {
                animacijaUgaoneBrzineUMS = 600;
            }
            fp.Kreni();
        }

        private void SlajedrVertikala_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Vertikala = (int)SlajedrVertikala.Value;
            switch (SlajedrVertikala.Value)
            {
                case 1:
                    TextBoxSmerVetikala.Text = "Gore";
                    fp.SmerB = 0X18;
                    fp.Kreni();
                    break;
                case -1:
                    TextBoxSmerVetikala.Text = "Dole";
                    fp.SmerB = 0X10;
                    fp.Kreni();
                    break;
                default:
                    TextBoxSmerVetikala.Text = "Sredina";
                    fp.SmerB = 0X0;
                    fp.Kreni();
                    break;
            }
        }

        private void SlajderHorizontala_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Horizontala = (int)SlajderHorizontala.Value;
            switch (SlajderHorizontala.Value)
            {
                case 1:
                    TextBoxSmerHorizontala.Text = "Desno";
                    fp.SmerA = 0x2;
                    fp.Kreni();
                    break;
                case -1:
                    TextBoxSmerHorizontala.Text = "Levo";
                    fp.SmerA = 0x4;
                    fp.Kreni();
                    break;
                default:
                    TextBoxSmerHorizontala.Text = "Sredina";
                    fp.SmerA = 0x0;
                    fp.Kreni();
                    break;
            }
        }
        #endregion

        #region Funkcije za iscrtavanje Detektovanih ciljeva

        Canvas KontejnerZaDetekciju = new Canvas();
        private void Inicijalizacija_KontejnerZaDetekciju()
        {
            KontejnerZaDetekciju.Width = MestoZaElipsu.ActualWidth;
            KontejnerZaDetekciju.Height = MestoZaElipsu.ActualWidth;
            Kontejner.Children.Add(KontejnerZaDetekciju);

        }
        public double[] AbsolutneKoordinate(double angle, int rezCelija)
        {
            //ovde je potrebno da se ugao podeli sa ugaonom brzinom
            double konstanta = Kontejner.Width / (2 * ModRada);
            double x = rezCelija * konstanta * Math.Cos((-angle + 180 + 90) / 57.5) + (KontejnerZaDetekciju.Width) / 2;
            double y = rezCelija * konstanta * Math.Sin((-angle + 180 + 90) / 57.5) + (KontejnerZaDetekciju.Width) / 2;
            return new double[] { x, y };

        }
        //public double[] AbsolutneKoordinate1024(double angle, int rezCelija)
        //{
        //    const double konstanta = 0.68359375;
        //    double x = rezCelija * konstanta * Math.Cos((-angle + 180 + 90) / 57.5) + MestoZaElipsu.ActualWidth / 2;
        //    double y = rezCelija * konstanta * Math.Sin((-angle + 180 + 90) / 57.5) + MestoZaElipsu.ActualWidth / 2;
        //    return new double[] { x, y };

        //}

        private async void Receiving_ProcesDetekcija(object sender, object[] e)
        {
            var i = (int)e[0];
            var ugao = (ushort)e[1];
            await Task.Run(() => {


                Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Render, (ThreadStart)delegate () {
                    Ellipse rec = new Ellipse();

                    //Animacija za nestajnje sa pokazivaca
                    DoubleAnimation animation = new DoubleAnimation();
                    animation.To = 0;
                    //animation.From = 1;
                    animation.Duration = TimeSpan.FromMilliseconds(30000);
                    animation.EasingFunction = new QuadraticEase();

                    Storyboard sb = new Storyboard();
                    sb.Children.Add(animation);

                    rec.Opacity = 1;
                    rec.Visibility = Visibility.Visible;

                    Storyboard.SetTarget(sb, rec);
                    Storyboard.SetTargetProperty(sb, new PropertyPath(Control.OpacityProperty));
                    sb.Completed += new EventHandler((s, e1) => {
                        rec.Opacity = 0;
                        rec.Visibility = Visibility.Collapsed;
                        KontejnerZaDetekciju.Children.Remove(rec);
                        rec = null;
                    });
                    sb.Begin();
                    //ovde se zavrsava

                    rec.Width = Kontejner.Width / (2 * ModRada);
                    rec.Height = Kontejner.Width / (2 * ModRada);
                    rec.Fill = Brushes.Yellow;
                    KontejnerZaDetekciju.Children.Add(rec);
                    Canvas.SetLeft(rec, AbsolutneKoordinate(ugao * 0.01, i)[1]);
                    Canvas.SetTop(rec, AbsolutneKoordinate(ugao * 0.01, i)[0]);
                });



            });

        }


        #endregion

        ReceivingThread receiving = new ReceivingThread(21566);
        public int ModRada { get; set; } = 256; //Glavni proprety za odredjivanje moda rada
        FunkcijeZaPlatformu fp = new FunkcijeZaPlatformu();
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            fp.Platforma.Connect(fp.ip);
            //   prvo dodajemo gridu   Kontejner
            Inicijalizacija_Kontejner();
            //  dodajemo gridu   KontejnerElipse
            Inicijalizacija_KontejnerElipse();
            //  dodajemo gridu   Kontejner2
            Inicijalizacija_Kontejner2();
            //   dodajemo gridu  Pokazivac2
            Inicijalizacija_Pokazivac2();
            //     dodajemo gridu  Pokazivac
            Inicijalizacija_Pokazivac();
            // dodajemo belezivace Ugla gridu
            ObelezivaciUgla();
            //dodajemo Buttone Za animacije
            Inicijalizacija_Buttona_Za_Obelezavanje_Daljine();
            //dodajemo radio buttone za izbor moda
            //Inicijalizacija_RadioButtona();
            //dodajemo grafik
            Inicijalizacija_Grafika();
            //inicijlaziacija vremenske baze
            Inicijalizacija_VremenskeBaze();
            Inicijalizacija_KontejnerZaDetekciju();
            statusBar.Width = Glavni.Width;
            // registovanje Listenera za zumiranje
            scrollViewer.ScrollChanged += OnScrollViewerScrollChanged;
            scrollViewer.MouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseLeftButtonUp += OnMouseLeftButtonUp;
            scrollViewer.PreviewMouseWheel += OnPreviewMouseWheel;
            scrollViewer.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
            scrollViewer.MouseMove += OnMouseMove;
            slider.ValueChanged += OnSliderValueChanged;

            receiving.RecivingThreadNotifcationTime += Receiving_RecivingThreadNotifcationTime;
            receiving.RecivingThreadNotificationMessage += Receiving_RecivingThreadNotificationMessage;
            receiving._Osvezavanje.HendleGrafik += RenderMenagemenThread_HendleGrafik;
            receiving._Osvezavanje.HendleTimeBase += RenderMenagemenThread_HendleTimeBase;
            receiving._Osvezavanje.RenderingTime += RenderMenagemenThread_RenderingTime;
            receiving.ProcesDetekcija += Receiving_ProcesDetekcija;
            receiving._Osvezavanje.ModRada = ModRada;
            receiving._Osvezavanje.Pokteni_Nit();
            receiving.PokreniRezim2048(true);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            receiving._cts.Cancel();
            receiving._cts.Dispose();
            receiving._cts = new CancellationTokenSource();
            receiving.ZaustaviRezim2048();
            receiving._Osvezavanje.ZaustaviRender();
            string root = Directory.GetCurrentDirectory();
            DirectoryInfo OneAbove = Directory.GetParent(root);
            DirectoryInfo OneAbove1 = Directory.GetParent(OneAbove.FullName);
            DirectoryInfo OneAbove2 = Directory.GetParent(OneAbove1.FullName);
            Process.Start(OneAbove2.FullName + @"\StartUpWindow\bin\Debug\StartUpWindow.exe");
            Process[] procesi = Process.GetProcessesByName("Radar256");
            foreach (var item in procesi)
            {
                item.Kill();
            }
            //Application.Current.Shutdown();
        }


        private void RenderMenagemenThread_RenderingTime(object sender, System.Diagnostics.Stopwatch e)
        {
            Dispatcher.Invoke(() => { textZaVremeGrafik.Text = "Vreme za osvezavanje grafika:" + e.ElapsedMilliseconds.ToString() + " ms"; });
        }
        int k = 0;
        private void RenderMenagemenThread_HendleTimeBase(object sender, UInt16 e)
        {
            var prethodni = (double)0;
            Dispatcher.Invoke(() => {  prethodni = Ugao; });
            var sledeci = e * 0.01 + 90+2*k*180;
            var razlika = prethodni - sledeci;
            if (razlika>0)
            {
                k++;
            }
           
            Dispatcher.Invoke(() => { Ugao = e * 0.01+90 +2*k*180; });
        }


        private void RenderMenagemenThread_HendleGrafik(object sender, object[] e)
        {
            Ispis(e);
        }

        private void Receiving_RecivingThreadNotificationMessage(object sender, string e)
        {
            Dispatcher.Invoke(() =>
            {
                statusText.Text = e;
            });
        }

        private void Receiving_RecivingThreadNotifcationTime(object sender, System.Diagnostics.Stopwatch e)
        {
            Dispatcher.Invoke(() =>
            {
                textblockStoperica.Text = "Vreme obrade Buffera:" + e.ElapsedMilliseconds.ToString() + "milisekunde";
            });
        }
    }
}
