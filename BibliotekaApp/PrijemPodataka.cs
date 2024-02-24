// Decompiled with JetBrains decompiler
// Type: BibliotekaApp.PrijemPodataka
// Assembly: BibliotekaApp, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 79E5A823-D5B6-4126-B437-420B506F0180
// Assembly location: D:\E 2023-10-29 16;51;32\VojnaAkademija\DiplomskiRadovi\MojDiplomski\Diplomski7\SolutinoMarkoFinal\Radar512\bin\Debug\BibliotekaApp.dll

using System.Net;
using System.Net.Sockets;

#nullable disable
namespace BibliotekaApp
{
  public class PrijemPodataka
  {
    public byte[] Dgram = new byte[1040];
    public byte[][] Buffer1 = new byte[2048][];
    public byte[][] Buffer2 = new byte[2048][];

    public Socket Socket { get; set; }

    public PrijemPodataka(int port)
    {
      IPEndPoint localEP = new IPEndPoint(IPAddress.Any, port);
      Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
      socket.Bind((EndPoint) localEP);
      this.Socket = socket;
    }
  }
}
