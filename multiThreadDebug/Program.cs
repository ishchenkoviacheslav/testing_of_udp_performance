using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace multiThreadDebug
{
    class Program
    {
        private static readonly object loker = new object();
        public static List<int> MyList = new List<int>();

        static UdpClient udp = new UdpClient();
        static void Main(string[] args)
        {
            Random r = new Random();
            for (int i = 0; i < 2000; i++)
            {
                MyList.Add(r.Next(0, 10000));
            }
            //for (int z = 0; z < 200; z++)
            //{
            //    new Thread(() =>
            //    {
            //        Thread.CurrentThread.IsBackground = true;
            //        Random ra = new Random();

            //        while (true)
            //        {
            //            int newValue = ra.Next(0, 1999);
            //            Thread.Sleep(10);
            //            lock (loker)
            //            {
            //                //Thread.Sleep(10);
            //                MyList[newValue] = newValue;
            //            }
            //        }
            //    }).Start();
            //}
            for (int t = 0; t < 10; t++)
            {
                DateTime temp;
                TimeSpan[] result = new TimeSpan[2000];
                IPEndPoint ipAndPort = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                for (int y = 0; y < 2000; y++)
                {
                    temp = DateTime.UtcNow;
                    //if ((y % 10) == 0)
                   // {
                        lock (loker)
                        {
                            byte[] bytes = Encoding.UTF8.GetBytes("325bp945nbpoidyushdp875908mqeyrgiuqp57jvq84ytp8 eygt0-3 745-v89uqemroti nqpty some long long data...........");
                            udp.Send(bytes, bytes.Length, ipAndPort);
                        }
                  //  }
                    result[y] = DateTime.UtcNow.Subtract(temp);
                }
                //for (int u = 0; u < result.Length; u++)
                //{
                //    Console.WriteLine(result[u]);
                //}
                TimeSpan total = TimeSpan.Zero;
                foreach (TimeSpan time in result)
                {
                    total = total + time;
                }
                Console.WriteLine($"total: {total}");
            }
            //List<int> tempList = new List<int>();
            //Console.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.ffff"));
            //for (int i = 0; i < MyList.Count; i++)
            //{
            //    if (MyList[i] > 4000 && MyList[i] < 6000)
            //    {
            //        tempList.Add(MyList[i]);
            //    }
            //}
            //Console.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.ffff"));

            //foreach (int item in MyList)
            //{
            //    if (item > 4000 && item < 6000)
            //    {
            //        tempList.Add(item);
            //    }
            //}
            //Console.WriteLine(DateTime.UtcNow.ToString("HH:mm:ss.ffff"));
            //Console.WriteLine(tempList.Count);
            //Task.Run(() =>
            //{
            //    lock (loker)
            //    {
            //        //foreach (int item in MyList)
            //        for (int i = 0; i < 10; i++)
            //        {
            //            Thread.Sleep(200);
            //            Console.WriteLine($"2222! - {(MyList[i] = i)}");

            //        }
            //    }
            //});

            //Task.Run(() =>
            //{
            //    lock (loker)
            //    {
            //        //foreach (int item in MyList)
            //        for (int i = 0; i < 10; i++)
            //        {
            //            Thread.Sleep(200);
            //            Console.WriteLine($"111111111! - {MyList[i]}");
            //        }
            //    }
            //});

            Console.WriteLine("Any key...");
            Console.ReadLine();
        }
    }
}
