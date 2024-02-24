// Decompiled with JetBrains decompiler
// Type: BibliotekaApp.FFTLibrary.Options
// Assembly: BibliotekaApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79E5A823-D5B6-4126-B437-420B506F0180
// Assembly location: D:\E 2023-10-29 16;51;32\VojnaAkademija\DiplomskiRadovi\MojDiplomski\Diplomski7\SolutinoMarkoFinal\Radar512\bin\Debug\BibliotekaApp.dll

using System;

#nullable disable
namespace BibliotekaApp.FFTLibrary
{
  [Flags]
  public enum Options : uint
  {
    Measure = 0,
    DestroyInput = 1,
    Unaligned = 2,
    ConserveMemory = 4,
    Exhaustive = 8,
    PreserveInput = 16, // 0x00000010
    Patient = 32, // 0x00000020
    Estimate = 64, // 0x00000040
  }
}
