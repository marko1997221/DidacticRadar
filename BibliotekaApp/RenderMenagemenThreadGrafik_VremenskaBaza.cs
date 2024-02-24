// Decompiled with JetBrains decompiler
// Type: BibliotekaApp.RenderMenagemenThreadGrafik_VremenskaBaza
// Assembly: BibliotekaApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79E5A823-D5B6-4126-B437-420B506F0180
// Assembly location: D:\E 2023-10-29 16;51;32\VojnaAkademija\DiplomskiRadovi\MojDiplomski\Diplomski7\SolutinoMarkoFinal\Radar512\bin\Debug\BibliotekaApp.dll

using System;
using System.Diagnostics;
using System.Security.Permissions;
using System.Threading;

#nullable disable
namespace BibliotekaApp
{
  public class RenderMenagemenThreadGrafik_VremenskaBaza
  {
    private Stopwatch stoperica;

    public Thread Nit { get; set; }

    public int ModRada { get; set; } = 512;

    public ushort AngleValue { get; set; }

    public object[] ObradjeniPodaci { get; set; }

    public object[] ObradjeniPodaci1 { get; set; }

    public int desired_Refresh_Time_Miliseconds { get; private set; }

    public event EventHandler<ushort> HendleTimeBase;

    public event EventHandler<object[]> HendleGrafik;

    public event EventHandler<Stopwatch> RenderingTime;

    public RenderMenagemenThreadGrafik_VremenskaBaza(int _desired_Refresh_Time_Miliseconds)
    {
      this.Nit = (Thread) null;
      this.stoperica = new Stopwatch();
      this.desired_Refresh_Time_Miliseconds = _desired_Refresh_Time_Miliseconds;
      this.AngleValue = (ushort) 0;
      this.ObradjeniPodaci = new object[1]
      {
        (object) new byte[1, 512]
      };
      this.ObradjeniPodaci1 = new object[1]
      {
        (object) new byte[1, 256]
      };
    }

    private void Osvezi(int DesiredTime)
    {
      this.stoperica.Start();
      while (true)
      {
        if (this.stoperica.ElapsedMilliseconds >= (long) DesiredTime)
        {
          EventHandler<ushort> hendleTimeBase = this.HendleTimeBase;
          if (hendleTimeBase != null)
            hendleTimeBase((object) this, this.AngleValue);
          if (this.ModRada == 512)
          {
            EventHandler<object[]> hendleGrafik = this.HendleGrafik;
            if (hendleGrafik != null)
              hendleGrafik((object) this, this.ObradjeniPodaci);
          }
          else
          {
            EventHandler<object[]> hendleGrafik = this.HendleGrafik;
            if (hendleGrafik != null)
              hendleGrafik((object) this, this.ObradjeniPodaci1);
          }
          EventHandler<Stopwatch> renderingTime = this.RenderingTime;
          if (renderingTime != null)
            renderingTime((object) this, this.stoperica);
          this.stoperica.Restart();
        }
      }
    }

    [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
    public void Pokteni_Nit()
    {
      if (this.Nit == null)
      {
        this.Nit = new Thread((ThreadStart) (() => this.Osvezi(this.desired_Refresh_Time_Miliseconds)));
        this.Nit.Priority = ThreadPriority.AboveNormal;
        this.Nit.Name = "Rendering_Thread";
        this.Nit.Start();
      }
      else
      {
        this.Nit.Abort();
        this.Nit = new Thread((ThreadStart) (() => this.Osvezi(this.desired_Refresh_Time_Miliseconds)));
        this.Nit.Priority = ThreadPriority.AboveNormal;
        this.Nit.Name = "Rendering_Thread";
        this.Nit.Start();
      }
    }

    [SecurityPermission(SecurityAction.Demand, ControlThread = true)]
    public void ZaustaviRender() => this.Nit.Abort();
  }
}
