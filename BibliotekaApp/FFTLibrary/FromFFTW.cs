// Decompiled with JetBrains decompiler
// Type: BibliotekaApp.FFTLibrary.FromFFTW
// Assembly: BibliotekaApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79E5A823-D5B6-4126-B437-420B506F0180
// Assembly location: D:\E 2023-10-29 16;51;32\VojnaAkademija\DiplomskiRadovi\MojDiplomski\Diplomski7\SolutinoMarkoFinal\Radar512\bin\Debug\BibliotekaApp.dll

using System;
using System.Runtime.InteropServices;

#nullable disable
namespace BibliotekaApp.FFTLibrary
{
  internal class FromFFTW
  {
    private const string Library = "libfftw3-3";

    [DllImport("libfftw3-3", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr fftw_plan_many_dft(
      int rank,
      int[] n,
      int howmany,
      IntPtr input,
      IntPtr inembed,
      int istride,
      int idist,
      IntPtr output,
      IntPtr onembed,
      int ostride,
      int odist,
      Direction kind,
      Options flags);
  }
}
