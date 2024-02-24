// Decompiled with JetBrains decompiler
// Type: BibliotekaApp.RadarControlMessages
// Assembly: BibliotekaApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79E5A823-D5B6-4126-B437-420B506F0180
// Assembly location: D:\E 2023-10-29 16;51;32\VojnaAkademija\DiplomskiRadovi\MojDiplomski\Diplomski7\SolutinoMarkoFinal\Radar512\bin\Debug\BibliotekaApp.dll

using System.Net;
using System.Net.Sockets;

#nullable disable
namespace BibliotekaApp
{
  internal class RadarControlMessages
  {
    private IPEndPoint ipEndplatformControl;
    private IPEndPoint ipEndRadarContol;
    private Socket sck;

    public RadarControlMessages(IPAddress ip, int _portPlatformControl, int _portRadarContorol)
    {
      this.sck = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      this.ipEndplatformControl = new IPEndPoint(ip, _portPlatformControl);
      this.ipEndRadarContol = new IPEndPoint(ip, _portRadarContorol);
    }

    public void Send_To_Platform(byte[] message)
    {
      if (this.sck.Connected)
      {
        this.sck.Disconnect(true);
      }
      else
      {
        this.sck.Connect((EndPoint) this.ipEndplatformControl);
        this.sck.Send(message);
      }
    }

    public void Send_To_Radar(byte[] message)
    {
      if (this.sck.Connected)
      {
        this.sck.Disconnect(true);
      }
      else
      {
        this.sck.Connect((EndPoint) this.ipEndRadarContol);
        this.sck.Send(message);
      }
    }
  }
}
