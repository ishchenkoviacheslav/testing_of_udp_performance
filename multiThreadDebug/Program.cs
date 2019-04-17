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
            //initialization
            Random r = new Random();
            for (int i = 0; i < 2000; i++)
            {
                MyList.Add(r.Next(0, 10000));
            }
            //create 2000 "clients"
            for (int z = 0; z < 2000; z++)
            {
                new Thread(() =>
                {
                    Thread.CurrentThread.IsBackground = true;
                    Random ra = new Random();
                    //every client update his data
                    while (true)
                    {
                        //some new client data
                        int newValue = ra.Next(0, 1999);
                        //frequency of client request + network delay
                        Thread.Sleep(10);
                        lock (loker)
                        {
                            //Thread.Sleep(3);
                            //new client data(coordinate)
                            MyList[newValue] = newValue;
                        }
                    }
                }).Start();
            }
            Thread.Sleep(3000);
            for (int t = 0; t < 50; t++)
            {
                Thread.Sleep(10);
                DateTime temp;
                TimeSpan[] result = new TimeSpan[2000];
                IPEndPoint ipAndPort = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                //2000
                for (int y = 0; y < MyList.Count; y++)
                {
                    temp = DateTime.UtcNow;
                    //if ((y % 10) == 0)
                    // {
                    lock (loker)
                    {
                        byte[] bytes = Encoding.ASCII.GetBytes("325bp945nbpoidyushdp875908mqeyrgiuqp57jvq84ytp8 eygt0-3 745-v89uqemroti nqpty some long long data..........." + MyList[y]);
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

            Console.WriteLine("Any key...");
            Console.ReadLine();
        }
    }
}
