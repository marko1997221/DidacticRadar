using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Radar512
{
    class PozadinaWindow
    {
        public static RadialGradientBrush PozadinaGlavnogProzora()
        {
            List<GradientStop> listaGradentat = new List<GradientStop>();
            GradientStop g1 = new GradientStop(Color.FromRgb(0, 20, 0), 0);
            GradientStop g3 = new GradientStop(Color.FromRgb(0, 30, 0), 0.1);
            GradientStop g2 = new GradientStop(Color.FromRgb(0, 0, 0), 0.9);

            listaGradentat.Add(g1);
            listaGradentat.Add(g2);
            listaGradentat.Add(g3);

            GradientStopCollection gst = new GradientStopCollection(listaGradentat);
            RadialGradientBrush rgb = new RadialGradientBrush(gst);
            rgb.RadiusX = 0.58;
            rgb.RadiusY = 0.58;
            return rgb;
        }
        public static RadialGradientBrush PozadinaLevoOdPokazivaca()
        {
            List<GradientStop> listaGradentat = new List<GradientStop>();
            GradientStop g1 = new GradientStop(Color.FromRgb(0, 0, 0), 0);
            GradientStop g2 = new GradientStop(Color.FromArgb(1,255, 255, 255), 0.2);

            listaGradentat.Add(g1);
            listaGradentat.Add(g2);

            GradientStopCollection gst = new GradientStopCollection(listaGradentat);
            RadialGradientBrush rgb = new RadialGradientBrush(gst);
            rgb.RadiusX = 5;
            rgb.RadiusY = 2;
            rgb.GradientOrigin = new Point(-0.1, 0);
            return rgb;
        }
        public static RadialGradientBrush PozadinaDesnoOdPokazivaca()
        {
            List<GradientStop> listaGradentat = new List<GradientStop>();
            GradientStop g1 = new GradientStop(Color.FromRgb(0, 0, 0), 0);
            GradientStop g2 = new GradientStop(Color.FromArgb(1, 255, 255, 255), 0.2);

            listaGradentat.Add(g1);
            listaGradentat.Add(g2);

            GradientStopCollection gst = new GradientStopCollection(listaGradentat);
            RadialGradientBrush rgb = new RadialGradientBrush(gst);
            rgb.RadiusX = 5;
            rgb.RadiusY = 2;
            rgb.GradientOrigin = new Point(1.1, 0);
            return rgb;
        }

        public static SolidColorBrush SolidColorBrush()
        {
            SolidColorBrush SCK = new SolidColorBrush(Color.FromArgb(100, 204, 204, 204));
            return SCK;
        }
        public static LinearGradientBrush WrapperGrafika()
        {
            LinearGradientBrush myLinearGradientBrush =
          new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0, 0.5);
            myLinearGradientBrush.EndPoint = new Point(1, 0.5);
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(0, 0, 0), 0.0));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(0, 191, 143), 0.05));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(0, 21, 0), 0.80));
            myLinearGradientBrush.GradientStops.Add(
              new GradientStop(Color.FromRgb(0, 0, 0), 0.95));
            return myLinearGradientBrush;
        }
        public static LinearGradientBrush PozadinaGrafika()
        {
            LinearGradientBrush myLinearGradientBrush =
                new LinearGradientBrush();
            myLinearGradientBrush.StartPoint = new Point(0.5, 0);
            myLinearGradientBrush.EndPoint = new Point(0.5, 1);
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(0, 0, 0), 0.0));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(9, 48, 40), 0.05));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(10, 60, 50), 0.35));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(20, 70, 60), 0.6));
            myLinearGradientBrush.GradientStops.Add(
                new GradientStop(Color.FromRgb(35, 122, 87), 0.90));
            myLinearGradientBrush.GradientStops.Add(
               new GradientStop(Color.FromRgb(0, 0, 0), 1));
            return myLinearGradientBrush;
        }


    }
}
