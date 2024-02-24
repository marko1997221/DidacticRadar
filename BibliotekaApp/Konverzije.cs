// Decompiled with JetBrains decompiler
// Type: BibliotekaApp.Konverzije
// Assembly: BibliotekaApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79E5A823-D5B6-4126-B437-420B506F0180
// Assembly location: D:\E 2023-10-29 16;51;32\VojnaAkademija\DiplomskiRadovi\MojDiplomski\Diplomski7\SolutinoMarkoFinal\Radar512\bin\Debug\BibliotekaApp.dll

using System;
using System.Numerics;

#nullable disable
namespace BibliotekaApp
{
  public static class Konverzije
  {
    private static byte[,] veca = new byte[2048, 1024];
    private static byte[,] manja = new byte[2048, 16];
    private static short[,] matrix = new short[2048, 512];
    public static Complex[] kompleksna = new Complex[524288];

    public static byte[][,] KonverzijaU16U1024(byte[][] data)
    {
      try
      {
        for (int index1 = 0; index1 < 2048; ++index1)
        {
          for (int index2 = 0; index2 < 1024; ++index2)
          {
            if (index2 < 16)
            {
              if (data[index1] != null)
              {
                Konverzije.manja[index1, index2] = data[index1][index2];
                Konverzije.veca[index1, index2] = data[index1][index2 + 16];
              }
              else
                break;
            }
            else if (data[index1] != null)
              Konverzije.veca[index1, index2] = data[index1][index2 + 16];
            else
              break;
          }
        }
      }
      catch (Exception ex)
      {
        throw;
      }
      return new byte[2][,]
      {
        Konverzije.manja,
        Konverzije.veca
      };
    }

    public static Complex[] Konverzija256Kompleksno(byte[,] data)
    {
      lock (data)
      {
        int index1 = 0;
        int index2 = 0;
        for (int index3 = 0; index3 < 2048; ++index3)
        {
          for (int index4 = 0; index4 < 512; ++index4)
          {
            Konverzije.matrix[index3, index4] = BitConverter.ToInt16(new byte[2]
            {
              data[index3, index1 + 1],
              data[index3, index1]
            }, 0);
            index1 += 2;
          }
          int index5 = 0;
          for (int index6 = 0; index6 < 256; ++index6)
          {
            Konverzije.kompleksna[index2] = new Complex((double) Konverzije.matrix[index3, index5], (double) Konverzije.matrix[index3, index5 + 1]);
            index5 += 2;
            ++index2;
          }
          index1 = 0;
        }
        return Konverzije.kompleksna;
      }
    }
  }
}
