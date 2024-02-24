using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using BibliotekaApp;
using System.IO;
using System.Diagnostics;
using BibliotekaApp.FFTLibrary;
using SharpFFTW.Double;
using System.Numerics;
using Radar256.MathLabFuntions;
using System.Security.Permissions;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Radar256
{
    public class ReceivingThread   :IDisposable
    {
        public CancellationTokenSource _cts = new CancellationTokenSource();
        public bool Exit { get; set; } = false;
        public Thread receivingThread_1024 { get; private set; }
        public Thread receivingThread_2048 { get; private set; }
        private static Task<object[]> Task1 = null;
        private static Task<object[]> Task2 = null;
        private static LinkedList<Task<object[]>> listaTakova1 = new LinkedList<Task<object[]>>();
        private static LinkedList<Task<object[]>> listaTakova2 = new LinkedList<Task<object[]>>();
        public event EventHandler<Stopwatch> RecivingThreadNotifcationTime;
        public event EventHandler<string> RecivingThreadNotificationMessage;
        public event EventHandler<object[]> ProcesDetekcija;
        private Stopwatch stoperica;
        private byte[] dgramIzmenjen;
        private static MathLabFunckije ml;
        private PrijemPodataka pr = new PrijemPodataka(21566);
        public  RenderMenagemenThreadGrafik_VremenskaBaza _Osvezavanje;
        private object[] PodaciDetekcija = new object[2];

        public ReceivingThread(int _port)
        {
            _cts = new CancellationTokenSource();
            ml = new MathLabFunckije();
            stoperica = new Stopwatch();
            dgramIzmenjen = new byte[1040];

            _Osvezavanje = new RenderMenagemenThreadGrafik_VremenskaBaza(100);


           
            receivingThread_2048 = new Thread((ThreadStart)delegate ()
            {
               // RenderMenagemenThreadGrafik_VremenskaBaza _Osvezavanje = new RenderMenagemenThreadGrafik_VremenskaBaza(100);
                _Osvezavanje.Pokteni_Nit();
                int x = 0;
                int y = 0;
                Task1 = new Task<object[]>(() => { return new object[1]; });
                Task2 = new Task<object[]>(() => { return new object[1]; });
                listaTakova1.AddFirst(Task1);
                listaTakova2.AddFirst(Task2);
                Task1.Start();
                Task2.Start();
                stoperica.Start();
                while (Exit!=true)
                {
                    pr.Socket.Receive(pr.Dgram);
                    //if (x == 1302 || y == 1302)
                    //{
                    // EventZaVremenskuBazu2?.Invoke(this, pr);
                    //}
                    MemoryStream ms = new MemoryStream(pr.Dgram);
                    if (pr.Buffer1[0] == null)
                    {
                            pr.Buffer1[x] = ms.ToArray();
                        x++;
                    }
                    else if (x != 0 && y == 0)
                    {
                        
                            pr.Buffer1[x] = ms.ToArray();
                        x++;

                    }
                    else if (x == 0 && y != 0)
                    {
                       
                            pr.Buffer2[y] = ms.ToArray();
                        y++;
                    }
                    if (x == 2048)
                    {
                        try
                        {
                            var stariUgao = _Osvezavanje.AngleValue;
                            _Osvezavanje.AngleValue = BitConverter.ToUInt16(new byte[] { pr.Dgram[9], pr.Dgram[10] }, 0);
                            var razlika = Math.Abs(stariUgao - _Osvezavanje.AngleValue);
                            if (razlika >= 20)
                            {
                                _Osvezavanje.AngleValue = stariUgao;
                            }

                            if (listaTakova1.Count != 0)
                            {



                                if ((listaTakova1.First.Value.Status == TaskStatus.WaitingForActivation))
                                {
                                    listaTakova1.First.Value.Start();
                                }
                                else if (listaTakova1.First.Value.Status == TaskStatus.RanToCompletion)
                                {

                                    //stoperica_Za_Grafik.Restart();
                                    //Ispis(listaTakova1.First.Value.Result);
                                    _Osvezavanje.ObradjeniPodaci = listaTakova1.First.Value.Result[0] as object[];
                                    //_Osvezavanje.AngleValue = (double)listaTakova1.First.Value.Result[1];
                                    //inreefejsT.ObradjeniPodaci =
                                    stoperica.Stop();

                                    RecivingThreadNotifcationTime?.Invoke(this, stoperica);
                                    //statusText.Dispatcher.Invoke(() => { statusText.Text = "Stoperica 1:" + stoperica.ElapsedMilliseconds.ToString() + "miliseknude potrebno za obradu"; });
                                    //Console.WriteLine("Stoperica 1:" + stoperica.ElapsedMilliseconds.ToString());
                                    Thread.Sleep(250);
                                    stoperica.Restart();
                                    listaTakova1.First.Value.Dispose();
                                    listaTakova1.RemoveFirst();
                                    //brojTaskova1++;
                                }
                                else if (listaTakova1.First.Value.Status == TaskStatus.Running)
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Zagusenje u pozadini Buffrer 1");
                                    //statusText.Dispatcher.Invoke(() => { statusText.Text = "Zagusenje u pozadini Buffrer 1"; });
                                    //await listaTakova1.First.Value;
                                    Task.WaitAll(listaTakova1.ToArray());
                                }
                                else
                                {
                                    // statusText.Dispatcher.Invoke(() => { statusText.Text = "Nesto cudno se desava"; });
                                    RecivingThreadNotificationMessage?.Invoke(this, "Nesto cudno se desava");
                                    listaTakova1.First.Value.Dispose();
                                    listaTakova1.RemoveFirst();
                                    //continue;

                                }

                            }
                        }
                        catch (Exception ER)
                        {
                            RecivingThreadNotificationMessage?.Invoke(this, "Doskolo je do greske pri obradi pozadinksih taskova! Sistemska poruka:" + ER.Message);
                            // statusText.Dispatcher.Invoke(() => { statusText.Text = "Doskolo je do greske pri obradi pozadinksih taskova " + ER.Message; });
                        }
                            pr.Buffer2[y] = ms.ToArray();
                        y++;
                        listaTakova1.AddLast(VratiKonverovatno2048(pr.Buffer1));
                        x = 0;
                    }
                    if (y == 2048)
                    {
                        var stariUgao = _Osvezavanje.AngleValue;
                        _Osvezavanje.AngleValue = BitConverter.ToUInt16(new byte[] { pr.Dgram[9], pr.Dgram[10] }, 0);
                        var razlika = Math.Abs(stariUgao - _Osvezavanje.AngleValue);
                        if (razlika >= 20)
                        {
                            _Osvezavanje.AngleValue = stariUgao;
                        }

                        try
                        {
                            if (listaTakova1.Count != 0)
                            {



                                if ((listaTakova2.First.Value.Status == TaskStatus.WaitingForActivation))
                                {
                                    listaTakova2.First.Value.Start();
                                }
                                else if (listaTakova2.First.Value.Status == TaskStatus.RanToCompletion)
                                {
                                    stoperica.Stop();
                                    _Osvezavanje.ObradjeniPodaci = listaTakova2.First.Value.Result[0] as object[];
                                    //_Osvezavanje.AngleValue = (double)listaTakova2.First.Value.Result[1];
                                    RecivingThreadNotifcationTime?.Invoke(this, stoperica);

                                    Thread.Sleep(250);
                                    stoperica.Restart();
                                    listaTakova2.First.Value.Dispose();
                                    listaTakova2.RemoveFirst();

                                }
                                else if (listaTakova2.First.Value.Status == TaskStatus.Running)
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Zagusenje u pozadini Buffrer 2");
                                    Task.WaitAll(listaTakova2.ToArray());
                                }
                                else
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Nesto cudno se desava");
                                    listaTakova2.First.Value.Dispose();
                                    listaTakova2.RemoveFirst();

                                }

                            }
                        }
                        catch (Exception Er)
                        {

                            RecivingThreadNotificationMessage?.Invoke(this, "Doskolo je do greske pri obradi pozadinksih taskova! Sistemska poruka:" + Er.Message);
                            //Console.WriteLine("Dolazi do greske sa radom Taskova!");
                        }
                       
                            pr.Buffer1[x] = ms.ToArray();
                        
                        x++;
                        listaTakova2.AddLast(VratiKonverovatno2048(pr.Buffer2));
                        y = 0;
                        //op2.Buff = pr.Buffer1;
                        //EventZaBuffer2?.Invoke(this, op2);
                    }
                    ms.Dispose();
                    ms.Close();

                }
            });
        }

        
        public void PokreniRezim1024(bool ex)
        {

            Exit = false;
            receivingThread_1024 = new Thread((ThreadStart)delegate ()
            {
               // _Osvezavanje = new RenderMenagemenThreadGrafik_VremenskaBaza(100);
                // _Osvezavanje.Pokteni_Nit();
                int x = 0;
                int y = 0;
                pr.Buffer1 = new byte[2048][];
                pr.Buffer2 = new byte[2048][];
                Task1 = new Task<object[]>(() => { return new object[1]; },_cts.Token);
                Task2 = new Task<object[]>(() => { return new object[1]; }, _cts.Token);
                listaTakova1.AddFirst(Task1);
                listaTakova2.AddFirst(Task2);
                Task1.Start();
                Task2.Start();
                stoperica.Start();
                while (Exit != true)
                {
                    pr.Socket.Receive(pr.Dgram);
                    //if (x == 1302 || y == 1302)
                    //{
                    // EventZaVremenskuBazu2?.Invoke(this, pr);
                    //}
                    MemoryStream ms = new MemoryStream(pr.Dgram);
                    if (pr.Buffer1[0] == null)
                    {
                        if (x % 2 == 0 && BitConverter.ToUInt32(pr.Dgram, 17) == 0)
                        {
                            pr.Buffer1[x] = ms.ToArray();
                        }
                        else if (x % 2 == 1 && BitConverter.ToUInt32(pr.Dgram, 1019) == 0)
                        {
                            pr.Buffer1[x] = ms.ToArray();
                        }
                        else
                        {
                            MemoryStream drugi = new MemoryStream(dgramIzmenjen);
                            pr.Buffer1[x] = drugi.ToArray();
                            x++;
                            pr.Buffer1[x] = ms.ToArray();
                            //drugi.Dispose();
                        }
                        x++;
                    }
                    else if (x != 0 && y == 0 && x<2048)
                    {
                        if (x % 2 == 0 && BitConverter.ToUInt32(pr.Dgram, 17) == 0)
                        {
                            pr.Buffer1[x] = ms.ToArray();
                        }
                        else if (x % 2 == 1 && BitConverter.ToUInt32(pr.Dgram, 1019) == 0)
                        {
                            pr.Buffer1[x] = ms.ToArray();
                        }
                        else
                        {
                            MemoryStream drugi = new MemoryStream(dgramIzmenjen);
                            pr.Buffer1[x] = drugi.ToArray();
                            x++;
                            pr.Buffer1[x] = ms.ToArray();
                           // drugi.Dispose();
                        }
                        x++;

                    }
                    else if (x == 0 && y != 0 && y < 2048)
                    {
                        if (y % 2 == 0 && BitConverter.ToUInt32(pr.Dgram, 17) == 0)
                        {
                            pr.Buffer2[y] = ms.ToArray();
                        }
                        else if (y % 2 == 1 && BitConverter.ToUInt32(pr.Dgram, 1019) == 0)
                        {
                            pr.Buffer2[y] = ms.ToArray();
                        }
                        else
                        {
                            MemoryStream drugi = new MemoryStream(dgramIzmenjen);
                            pr.Buffer2[y] = drugi.ToArray();
                            y++;
                            pr.Buffer2[y] = ms.ToArray();
                           // drugi.Dispose();
                        }

                        y++;
                    }
                    if (x == 2048)
                    {
                        if (pr.Dgram[11]==89)
                        {
                            _Osvezavanje.AngleValue = BitConverter.ToUInt16(new byte[] { pr.Dgram[9], pr.Dgram[10] }, 0);
                        }

                        try
                        {
                            if (listaTakova1.Count != 0)
                            {



                                if ((listaTakova1.First.Value.Status == TaskStatus.WaitingForActivation))
                                {
                                    listaTakova1.First.Value.Start();
                                }
                                else if (listaTakova1.First.Value.Status == TaskStatus.RanToCompletion)
                                {
                                    if (listaTakova1.First.Value.Result[0] as object != null)
                                    {
                                        //stoperica_Za_Grafik.Restart();
                                        //Ispis(listaTakova1.First.Value.Result);
                                        _Osvezavanje.ObradjeniPodaci = listaTakova1.First.Value.Result[0] as object[];
                                        Detekcija(listaTakova1.First.Value.Result[0] as object[]);
                                       // _Osvezavanje.AngleValue = (double)listaTakova1.First.Value.Result[1];
                                        //inreefejsT.ObradjeniPodaci =
                                        stoperica.Stop();
                                    }
                                    RecivingThreadNotificationMessage?.Invoke(this, "Obradjujem...");
                                    RecivingThreadNotifcationTime?.Invoke(this, stoperica);
                                    //statusText.Dispatcher.Invoke(() => { statusText.Text = "Stoperica 1:" + stoperica.ElapsedMilliseconds.ToString() + "miliseknude potrebno za obradu"; });
                                    //Console.WriteLine("Stoperica 1:" + stoperica.ElapsedMilliseconds.ToString());
                                    Thread.Sleep(250);
                                    stoperica.Restart();
                                   // listaTakova1.First.Value.Dispose();
                                    listaTakova1.RemoveFirst();
                                    //brojTaskova1++;
                                }
                                else if (listaTakova1.First.Value.Status == TaskStatus.Running)
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Zagusenje u pozadini Buffrer 1");
                                    //statusText.Dispatcher.Invoke(() => { statusText.Text = "Zagusenje u pozadini Buffrer 1"; });
                                    //await listaTakova1.First.Value;
                                    Task.WaitAll(listaTakova1.ToArray());
                                }
                                else
                                {
                                    // statusText.Dispatcher.Invoke(() => { statusText.Text = "Nesto cudno se desava"; });
                                    RecivingThreadNotificationMessage?.Invoke(this, "Nesto cudno se desava");
                                   // listaTakova1.First.Value.Dispose();
                                    listaTakova1.RemoveFirst();
                                    //continue;

                                }

                            }
                        }
                        catch (Exception ER)
                        {
                            RecivingThreadNotificationMessage?.Invoke(this, "Doskolo je do greske pri obradi pozadinksih taskova! Sistemska poruka:" + ER.Message);
                            // statusText.Dispatcher.Invoke(() => { statusText.Text = "Doskolo je do greske pri obradi pozadinksih taskova " + ER.Message; });
                        }
                        if (y % 2 == 0 && BitConverter.ToUInt32(pr.Dgram, 17) == 0)
                        {
                            pr.Buffer2[y] = ms.ToArray();
                        }
                        else if (y % 2 == 1 && BitConverter.ToUInt32(pr.Dgram, 1019) == 0)
                        {
                            pr.Buffer2[y] = ms.ToArray();
                        }
                        else
                        {
                            MemoryStream drugi = new MemoryStream(dgramIzmenjen);
                            pr.Buffer2[y] = drugi.ToArray();
                            y++;
                            pr.Buffer2[y] = ms.ToArray();
                           // drugi.Dispose();
                        }
                        y++;

                        listaTakova1.AddLast(VratiKonverovatno(pr.Buffer1, _cts.Token));
                        x = 0;

                    }
                    if (y == 2048)
                    {
                        if (pr.Dgram[11]==89)
                        {
                            _Osvezavanje.AngleValue = BitConverter.ToUInt16(new byte[] { pr.Dgram[9], pr.Dgram[10] }, 0);
                        }
                
                        try
                        {
                            if (listaTakova2.Count != 0)
                            {
                                if ((listaTakova2.First.Value.Status == TaskStatus.WaitingForActivation))
                                {
                                    listaTakova2.First.Value.Start();
                                }
                                else if (listaTakova2.First.Value.Status == TaskStatus.RanToCompletion)
                                {
                                    if (listaTakova2.First.Value.Result[0] as object != null)
                                    {
                                        stoperica.Stop();
                                        _Osvezavanje.ObradjeniPodaci = listaTakova2.First.Value.Result[0] as object[];
                                        Detekcija(listaTakova2.First.Value.Result[0] as object[]);
                                        //_Osvezavanje.AngleValue = (double)listaTakova2.First.Value.Result[1];
                                    }
                                    RecivingThreadNotifcationTime?.Invoke(this, stoperica);
                                    RecivingThreadNotificationMessage?.Invoke(this, "Obradjujem...");
                                    Thread.Sleep(250);
                                    stoperica.Restart();
                                    listaTakova2.First.Value.Dispose();
                                    listaTakova2.RemoveFirst();

                                }
                                else if (listaTakova2.First.Value.Status == TaskStatus.Running)
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Zagusenje u pozadini Buffrer 2");
                                    Task.WaitAll(listaTakova2.ToArray());
                                }
                                else
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Nesto cudno se desava");
                                    listaTakova2.First.Value.Dispose();
                                    listaTakova2.RemoveFirst();

                                }

                            }
                        }
                        catch (Exception Er)
                        {

                            RecivingThreadNotificationMessage?.Invoke(this, "Doskolo je do greske pri obradi pozadinksih taskova! Sistemska poruka:" + Er.Message);
                            //Console.WriteLine("Dolazi do greske sa radom Taskova!");
                        }
                        if (x % 2 == 0 && BitConverter.ToUInt32(pr.Dgram, 17) == 0)
                        {
                            pr.Buffer1[x] = ms.ToArray();
                        }
                        else if (x % 2 == 1 && BitConverter.ToUInt32(pr.Dgram, 1019) == 0)
                        {
                            pr.Buffer1[x] = ms.ToArray();
                        }
                        else
                        {
                            MemoryStream drugi = new MemoryStream(dgramIzmenjen);
                            pr.Buffer1[x] = drugi.ToArray();
                            x++;
                            pr.Buffer1[x] = ms.ToArray();
                           // drugi.Dispose();
                        }

                        x++;
                        listaTakova2.AddLast(VratiKonverovatno(pr.Buffer2, _cts.Token));
                        y = 0;
                        //op2.Buff = pr.Buffer1;
                        //EventZaBuffer2?.Invoke(this, op2);
                    }
                    ms.Dispose();
                    ms.Close();

                }
            });
            //Task.WaitAll(listaTakova1.ToArray());
            //Task.WaitAll(listaTakova2.ToArray());
            //listaTakova1.Clear();
            //listaTakova2.Clear();
            receivingThread_1024.Start();
            
            receivingThread_1024.Priority = ThreadPriority.Normal;
        }

        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public  void ZaustaviRezim1024()
        {
            //Task.WaitAll(listaTakova1.ToArray());
            //Task.WaitAll(listaTakova2.ToArray());
            if (receivingThread_2048.ThreadState == System.Threading.ThreadState.Running)
            {
                receivingThread_2048.Abort();
                //foreach (var item in listaTakova1)
                //{
                //    await item;
                //}
                //foreach (var item in listaTakova2)
                //{
                //    await item;
                //}
                
            }
            else if (receivingThread_1024.ThreadState == System.Threading.ThreadState.Running)
            {
                receivingThread_1024.Abort();
                //foreach (var item in listaTakova1)
                //{
                //    await item;
                //}
                //foreach (var item in listaTakova2)
                //{
                //    await item;
                //}
            }
     
            listaTakova1.Clear();
            listaTakova2.Clear();
        }
        [SecurityPermissionAttribute(SecurityAction.Demand, ControlThread = true)]
        public  void ZaustaviRezim2048()
        {
            //Task.WaitAll(listaTakova1.ToArray());
            //Task.WaitAll(listaTakova2.ToArray());
           
            if (receivingThread_2048.ThreadState == System.Threading.ThreadState.Running)
            {
                receivingThread_2048.Abort();
                //foreach (var item in listaTakova1)
                //{
                //    await item;
                //}
                //foreach (var item in listaTakova2)
                //{
                //    await item;
                //}
            }
            else if (receivingThread_1024.ThreadState == System.Threading.ThreadState.Running)
            {
                receivingThread_1024.Abort();
                //foreach (var item in listaTakova1)
                //{
                //    await item;
                //}
                //foreach (var item in listaTakova2)
                //{
                //    await item;
                //}
            }
            listaTakova1.Clear();
            listaTakova2.Clear();

        }

       
        public void PokreniRezim2048(bool ex)
        {
            Exit = false;
            receivingThread_2048 = new Thread((ThreadStart)delegate ()
            {
                Thread.Sleep(100);
                //_Osvezavanje = new RenderMenagemenThreadGrafik_VremenskaBaza(100);
                // RenderMenagemenThreadGrafik_VremenskaBaza _Osvezavanje = new RenderMenagemenThreadGrafik_VremenskaBaza(100);
                //_Osvezavanje.Pokteni_Nit();
                int x = 0;
                int y = 0;
                pr.Buffer1 = new byte[2048][];
                pr.Buffer2 = new byte[2048][];
                Task1 = new Task<object[]>(() => { return new object[1]; },_cts.Token);
                Task2 = new Task<object[]>(() => { return new object[1]; }, _cts.Token);
                listaTakova1.AddFirst(Task1);
                listaTakova2.AddFirst(Task2);
                Task1.Start();
                Task2.Start();
                stoperica.Start();
                while (Exit != true)
                {
                    pr.Socket.Receive(pr.Dgram);
                    //if (x == 1302 || y == 1302)
                    //{
                    // EventZaVremenskuBazu2?.Invoke(this, pr);
                    //}
                    MemoryStream ms = new MemoryStream(pr.Dgram);
                    if (pr.Buffer1[0] == null)
                    {
                        pr.Buffer1[x] = ms.ToArray();
                        x++;
                    }
                    else if (x != 0 && y == 0 && x < 2048)
                    {

                        pr.Buffer1[x] = ms.ToArray();
                        x++;

                    }
                    else if (x == 0 && y != 0 && y<2048)
                    {

                        pr.Buffer2[y] = ms.ToArray();
                        y++;
                    }
                    if (x == 2048)
                    {

                        if (pr.Dgram[11]==89)
                        {
                            _Osvezavanje.AngleValue = BitConverter.ToUInt16(new byte[] { pr.Dgram[9], pr.Dgram[10] }, 0);
                        }
                      
                        try
                        {
                            if (listaTakova1.Count != 0)
                            {



                                if ((listaTakova1.First.Value.Status == TaskStatus.WaitingForActivation))
                                {
                                    listaTakova1.First.Value.Start();
                                }
                                else if (listaTakova1.First.Value.Status == TaskStatus.RanToCompletion)
                                {
                                    if (listaTakova1.First.Value.Result[0] as object!=null)
                                    {
                                        //stoperica_Za_Grafik.Restart();
                                        //Ispis(listaTakova1.First.Value.Result);
                                        
                                        _Osvezavanje.ObradjeniPodaci1 = listaTakova1.First.Value.Result[0] as object[];
                                        Detekcija(listaTakova1.First.Value.Result[0] as object[]);
                                        //var privremena = listaTakova1.First.Value.Result;
                                        //_Osvezavanje.AngleValue = (double)listaTakova1.First.Value.Result[1];
                                        //inreefejsT.ObradjeniPodaci =
                                        RecivingThreadNotificationMessage?.Invoke(this, "Obradjujem...");

                                    }
                                        stoperica.Stop();

                                        RecivingThreadNotifcationTime?.Invoke(this, stoperica);
                                        //statusText.Dispatcher.Invoke(() => { statusText.Text = "Stoperica 1:" + stoperica.ElapsedMilliseconds.ToString() + "miliseknude potrebno za obradu"; });
                                        //Console.WriteLine("Stoperica 1:" + stoperica.ElapsedMilliseconds.ToString());
                                        Thread.Sleep(250);
                                        stoperica.Restart();
                                        //listaTakova1.First.Value.Dispose();
                                        listaTakova1.RemoveFirst();
                                        //brojTaskova1++;
                                   

                                }
                                else if (listaTakova1.First.Value.Status == TaskStatus.Running)
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Zagusenje u pozadini Buffrer 1");
                                    //statusText.Dispatcher.Invoke(() => { statusText.Text = "Zagusenje u pozadini Buffrer 1"; });
                                    //await listaTakova1.First.Value;
                                    Task.WaitAll(listaTakova1.ToArray());
                                }
                                else
                                {
                                    // statusText.Dispatcher.Invoke(() => { statusText.Text = "Nesto cudno se desava"; });
                                    RecivingThreadNotificationMessage?.Invoke(this, "Nesto cudno se desava");
                                    //listaTakova1.First.Value.Dispose();
                                    listaTakova1.RemoveFirst();
                                    //continue;

                                }

                            }
                        }
                        catch (Exception ER)
                        {
                            RecivingThreadNotificationMessage?.Invoke(this, "Doskolo je do greske pri obradi pozadinksih taskova! Sistemska poruka:" + ER.Message);
                            // statusText.Dispatcher.Invoke(() => { statusText.Text = "Doskolo je do greske pri obradi pozadinksih taskova " + ER.Message; });
                        }
                        pr.Buffer2[y] = ms.ToArray();
                        y++;
                        listaTakova1.AddLast(VratiKonverovatno2048(pr.Buffer1, _cts.Token));
                        x = 0;
                    }
                    if (y == 2048)
                    {

                        if (pr.Dgram[11] == 89)
                        {
                           _Osvezavanje.AngleValue = BitConverter.ToUInt16(new byte[] { pr.Dgram[9], pr.Dgram[10] }, 0);
                        }
                        try
                        {
                            if (listaTakova2.Count != 0)
                            {



                                if ((listaTakova2.First.Value.Status == TaskStatus.WaitingForActivation))
                                {
                                    listaTakova2.First.Value.Start();
                                }
                                else if (listaTakova2.First.Value.Status == TaskStatus.RanToCompletion)
                                {
                                    if (listaTakova2.First.Value.Result[0] as object != null)
                                    {
                                        stoperica.Stop();
                                        
                                            _Osvezavanje.ObradjeniPodaci1 = listaTakova2.First.Value.Result[0] as object[];
                                        Detekcija(listaTakova2.First.Value.Result[0] as object[]);
                                        //_Osvezavanje.AngleValue = (double)listaTakova2.First.Value.Result[1];

                                        RecivingThreadNotificationMessage?.Invoke(this, "Obradjujem...");
                                        RecivingThreadNotifcationTime?.Invoke(this, stoperica);
                                    }
                                        Thread.Sleep(250);
                                        stoperica.Restart();
                                        //listaTakova2.First.Value.Dispose();
                                        listaTakova2.RemoveFirst();
                                   
                                }
                                else if (listaTakova2.First.Value.Status == TaskStatus.Running)
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Zagusenje u pozadini Buffrer 2");
                                    Task.WaitAll(listaTakova2.ToArray());
                                }
                                else
                                {
                                    RecivingThreadNotificationMessage?.Invoke(this, "Nesto cudno se desava");
                                    //listaTakova2.First.Value.Dispose();
                                    listaTakova2.RemoveFirst();

                                }

                            }
                        }
                        catch (Exception Er)
                        {

                            RecivingThreadNotificationMessage?.Invoke(this, "Doskolo je do greske pri obradi pozadinksih taskova! Sistemska poruka:" + Er.Message);
                            //Console.WriteLine("Dolazi do greske sa radom Taskova!");
                        }

                        pr.Buffer1[x] = ms.ToArray();

                        x++;
                        listaTakova2.AddLast(VratiKonverovatno2048(pr.Buffer2, _cts.Token));
                        y = 0;
                        //op2.Buff = pr.Buffer1;
                        //EventZaBuffer2?.Invoke(this, op2);
                    }
                    ms.Dispose();
                    ms.Close();

                }
            });
            receivingThread_2048.Start();
            receivingThread_2048.Priority = ThreadPriority.Normal;
        }


        private static Task<object[]> VratiKonverovatno(byte[][] _buff, CancellationToken cancellationToken= default)
        {
            //Task<byte[][,]> t = new Task<byte[][,]>(() => { return Konverzije.KonverzijaU16U1024(_buff); });
            //t.Start();
            //var a = t.GetAwaiter();
            //return  a.GetResult();]
            cancellationToken.ThrowIfCancellationRequested();
            if (_buff==new byte[2048][])
            {
                return null;
            }
            cancellationToken.ThrowIfCancellationRequested();
            lock (_buff)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.Run<object[]>(() =>
                {
                   
                        cancellationToken.ThrowIfCancellationRequested();
                        var a = Konverzije.KonverzijaU16U1024(_buff);
                        cancellationToken.ThrowIfCancellationRequested();
                        var b = new ComplexArray(Konverzije.Konverzija256Kompleksno(a[1]));
                        cancellationToken.ThrowIfCancellationRequested();
                        var plan1 = KlasaPlanFFT.fftw_plan_many_dft(1, new int[] { 1024 }, 512, b, 512, 1, b, 512, 1, Direction.Forward, Options.Measure);
                        cancellationToken.ThrowIfCancellationRequested();
                        plan1.Execute();
                        cancellationToken.ThrowIfCancellationRequested();
                        var c = ml.Prosta_detekcija_pikova1024(b, cancellationToken) as object[];
                        cancellationToken.ThrowIfCancellationRequested();
                        return new object[] { c, BitConverter.ToUInt16(new byte[] { a[1][2047, 9], a[1][2047, 10] }, 0) * 0.0001 };
                    
                });
            }

        }


        private static Task<object[]> VratiKonverovatno2048(byte[][] _buff, CancellationToken cancellationToken = default)
        {
            //Task<byte[][,]> t = new Task<byte[][,]>(() => { return Konverzije.KonverzijaU16U1024(_buff); });
            //t.Start();
            //var a = t.GetAwaiter();
            //return  a.GetResult();
            cancellationToken.ThrowIfCancellationRequested();
            if (_buff== new byte[2048][])
            {
                return null;
            }
            cancellationToken.ThrowIfCancellationRequested();
            lock (_buff)
            {
                cancellationToken.ThrowIfCancellationRequested();
                return Task.Run<object[]>(() =>
                {
                    
                        cancellationToken.ThrowIfCancellationRequested();
                        var a = Konverzije.KonverzijaU16U1024(_buff);
                        cancellationToken.ThrowIfCancellationRequested();
                        var b = new ComplexArray(Konverzije.Konverzija256Kompleksno(a[1]));
                        cancellationToken.ThrowIfCancellationRequested();
                        var plan1 = KlasaPlanFFT.fftw_plan_many_dft(1, new int[] { 2048 }, 256, b, 256, 1, b, 256, 1, Direction.Forward, Options.Measure);
                        cancellationToken.ThrowIfCancellationRequested();
                        plan1.Execute();
                        cancellationToken.ThrowIfCancellationRequested();
                        var c = ml.Prosta_detekcija_pikova(b, cancellationToken) as object[];
                        cancellationToken.ThrowIfCancellationRequested();
                        return new object[] { c, BitConverter.ToUInt16(new byte[] { a[1][2047, 9], a[1][2047, 10] }, 0) * 0.0001 };
                    
                    
                   
                });
            }

        }

        public void Detekcija(object[] data)
        {
            object o = data[0];
            double[,] red = o as double[,];
            double granica = 100;
            if (_Osvezavanje.ModRada == 256)
            {
                for (int i = 0; i < 256; i++)
                {
                    var a = red[0, i] / 10000;
                    if (a > granica)
                    {
                       
                        PodaciDetekcija[0] = i;
                        PodaciDetekcija[1] = _Osvezavanje.AngleValue;
                        ProcesDetekcija?.Invoke(this, PodaciDetekcija);
                    }
                    
                }
            }
            else
            {
                for (int i = 0; i < 512; i++)
                {
                    var a = red[0, i] / 10000;
                    if (a > granica)
                    {
                        PodaciDetekcija[0] = i;
                        PodaciDetekcija[1] = _Osvezavanje.AngleValue;
                        ProcesDetekcija?.Invoke(this, PodaciDetekcija);
                    }
                  
                }
            }

        }

        public void Dispose()
        {
            if (_cts!=null)
            {
                _cts.Dispose();
                _cts = null;
            }
        }
    }
}
