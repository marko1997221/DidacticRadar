using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Radar512
{
    class FunkcijeZaPlatformu
    {
        public int SmerA { get; set; } 
        public int SmerB { get; set; } 
        public int BrzinaA { get; set; } 
        public int BrzinaB { get; set; }
        public int IzvorSignala { get; set; } = 1;
        public int Blok_rx { get; set; } = 1;
        public int Pri { get; set; } = 1;
        public int DuzSekvence { get; set; }
        public int Amplituda { get; set; } = 0;
        public int Dopler { get; set; } = 0;
        public int Makro { get; set; } = 0;
        //public int Radi { get; set; } 

        public IPEndPoint ip = new IPEndPoint(IPAddress.Parse("192.168.19.32"), 2411);
        public IPEndPoint ip2 = new IPEndPoint(IPAddress.Parse("192.168.19.32"), 2410);
        public Socket Platforma = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        public byte[] NizZaPlatformu = new byte[18] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 2 };
        public byte[] NizZaPlatformu2 = new byte[18] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 };
        //public byte[] NizZaPlatformu1 = new byte[18]/* { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 4, 9, 0, 14, 1 };*/
        //                                             { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1,  1,   83,    0,    0 ,  84 ,   2, };

        public void Kreni()
        {
            Platforma.Connect(ip);
            var smer = SmerA + SmerB;
            int[] Pniz = new int[] { 1, 0, smer, BrzinaA, BrzinaB };
            int y=0;
            for (int i = 0; i < Pniz.Length; i++)
            {
                y += Pniz[i];
            }
            y = y % 256;
            if (y>127)
            {
                y = y - 256;
            }
                NizZaPlatformu[13] = (byte)smer;
                NizZaPlatformu[14] = (byte)BrzinaA;
                NizZaPlatformu[15] = (byte)BrzinaB;
                NizZaPlatformu[16] = (byte)y;
                Platforma.Send(NizZaPlatformu);
           // Platforma.Disconnect(true);
                //Platforma.Disconnect(false);
        }
        public void Radara()
        {
            Platforma.Connect(ip2);
            var kontrolni1 = IzvorSignala * 128 + Blok_rx * 64 + Pri * 32 + DuzSekvence;
            var kontrolni2 = Makro;
            if (kontrolni1>127)
            {
                kontrolni1 = kontrolni1 - 127;
            }
            if (kontrolni2 > 127)
            {
                kontrolni2 = kontrolni2 - 127;
            }
            NizZaPlatformu2[0] = (byte)kontrolni1;
            NizZaPlatformu2[1] = (byte)kontrolni2;
            NizZaPlatformu2[2] = (byte)Amplituda;
            NizZaPlatformu2[3] = (byte)Dopler;
            Platforma.Send(NizZaPlatformu2);
            //Platforma.Disconnect(true);

        }
    
    }
}
