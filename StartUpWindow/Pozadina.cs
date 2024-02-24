using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace StartUpWindow
{
    class Pozadina
    {
        public static RadialGradientBrush PozadinaGlavnogProzora()
        {
            List<GradientStop> listaGradentat = new List<GradientStop>();
            GradientStop g1 = new GradientStop(Color.FromRgb(153, 255, 153), 0);
            GradientStop g3 = new GradientStop(Color.FromRgb(51, 255, 153), 0.4);
            GradientStop g2 = new GradientStop(Color.FromRgb(0, 102, 51), 0.89);

            listaGradentat.Add(g1);
            listaGradentat.Add(g2);
            listaGradentat.Add(g3);

            GradientStopCollection gst = new GradientStopCollection(listaGradentat);
            RadialGradientBrush rgb = new RadialGradientBrush(gst);
            rgb.RadiusX = 0.5;
            rgb.RadiusY = 0.9;
            return rgb;
        }
        public static LinearGradientBrush Pozadina2()
        {
            List<GradientStop> listaGradentat = new List<GradientStop>();
            GradientStop g1 = new GradientStop(Color.FromRgb(83, 188, 83), 0);
            GradientStop g3 = new GradientStop(Color.FromRgb(83, 255, 22), 0.39);
            GradientStop g2 = new GradientStop(Color.FromRgb(0, 0, 0), 0.59);

            listaGradentat.Add(g1);
            listaGradentat.Add(g2);
            listaGradentat.Add(g3);

            GradientStopCollection lgt = new GradientStopCollection(listaGradentat);
            LinearGradientBrush lgb = new LinearGradientBrush(lgt);


            
            return lgb;
        }
    }
}
