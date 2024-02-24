using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using SharpFFTW.Double;

namespace Radar512.MathLabFuntions
{
    class MathLabFunckije
    {
        public MLApp.MLApp ml = new MLApp.MLApp();

        public object Prosta_detekcija_pikova(ComplexArray data1, CancellationToken cancellationToken)
        {
            object data;
            cancellationToken.ThrowIfCancellationRequested();
            ml.Execute(@" cd G:\MathlabFunkcije\Obrada256");
            cancellationToken.ThrowIfCancellationRequested();
            ml.Feval("obrada", 1, out data, data1.ToArray());
            return data;
        }
        public object Prosta_detekcija_pikova1(ComplexArray data1, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            object data;
            cancellationToken.ThrowIfCancellationRequested();
            ml.Execute(@" cd G:\MathlabFunkcije\Obrada256");
            cancellationToken.ThrowIfCancellationRequested();
            ml.Feval("obrada", 1, out data, data1.ToArray());
            return data;
        }
        public object Prosta_detekcija_pikova1024(ComplexArray data1, CancellationToken cancellationToken)
        {
            try
            {
                cancellationToken.ThrowIfCancellationRequested();
                object data;
                cancellationToken.ThrowIfCancellationRequested();
                ml.Execute(@" cd C:\Users\Lenovo\Desktop\MojDiplomski\Diplomski7\MathlabFunkcije\Obrada1024");
                cancellationToken.ThrowIfCancellationRequested();
                ml.Feval("obrada1024", 1, out data, data1.ToArray());
                return data;
            }
            catch (COMException)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return null;
            }
            
        }

    }
}
