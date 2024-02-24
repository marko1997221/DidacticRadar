// Decompiled with JetBrains decompiler
// Type: BibliotekaApp.FFTLibrary.KlasaPlanFFT
// Assembly: BibliotekaApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79E5A823-D5B6-4126-B437-420B506F0180
// Assembly location: D:\E 2023-10-29 16;51;32\VojnaAkademija\DiplomskiRadovi\MojDiplomski\Diplomski7\SolutinoMarkoFinal\Radar512\bin\Debug\BibliotekaApp.dll

using SharpFFTW;
using SharpFFTW.Double;
using System;
using System.Threading;

#nullable disable
namespace BibliotekaApp.FFTLibrary
{
  public class KlasaPlanFFT : AbstractPlan<double>
  {
    private static readonly Mutex mutex = new Mutex();

    private KlasaPlanFFT(
      IntPtr handle,
      AbstractArray<double> input,
      AbstractArray<double> output,
      bool ownsArrays)
      : base(handle, input, output, ownsArrays)
    {
    }

    public override void Dispose(bool disposing)
    {
      if (!this.hasDisposed)
      {
        if (this.handle != IntPtr.Zero)
        {
          NativeMethods.destroy_plan(this.handle);
          this.handle = IntPtr.Zero;
        }
        if (this.ownsArrays)
        {
          this.input.Dispose();
          this.output.Dispose();
        }
      }
      this.hasDisposed = disposing;
    }

    public static KlasaPlanFFT fftw_plan_many_dft(
      int rank,
      int[] n,
      int howmany,
      ComplexArray input,
      int istride,
      int idist,
      ComplexArray output,
      int ostride,
      int odist,
      Direction kind,
      Options flags)
    {
      KlasaPlanFFT.mutex.WaitOne();
      IntPtr handle = FromFFTW.fftw_plan_many_dft(rank, n, howmany, input.Handle, IntPtr.Zero, istride, idist, output.Handle, IntPtr.Zero, ostride, odist, kind, flags);
      KlasaPlanFFT.mutex.ReleaseMutex();
      return new KlasaPlanFFT(handle, (AbstractArray<double>) input, (AbstractArray<double>) output, false);
    }

    public override void Execute() => NativeMethods.execute(this.handle);
  }
}
