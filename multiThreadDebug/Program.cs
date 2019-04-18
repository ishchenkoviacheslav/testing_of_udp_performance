using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace multiThreadDebug
{
    class MyClass
    {
        public int MyProperty { get; set; }
        public MyClass()
        {
            MyProperty = 4;
        }
    }
    class Program
    {
        private static readonly object loker = new object();
        public static List<int> MyList = new List<int>();
        static UdpClient udp = new UdpClient();

        static void Main(string[] args)
        {
            System.Timers.Timer tim = new System.Timers.Timer() { Interval = 5000, Enabled = true, AutoReset = false };
            tim.Elapsed += (object sender, System.Timers.ElapsedEventArgs e)=> {((System.Timers.Timer)sender).Enabled = false; };
            while (true)
            {
                Thread.Sleep(1000);
                if(!tim.Enabled)
                {
                    Console.WriteLine("Timer worked!");
                    tim.Enabled = true;
                }
            }
            //TimeSpan res = TimeSpan.Zero;
            //DateTime tm;
            //List<MyClass> l = new List<MyClass>();
            //Random r2 = new Random();
            //for (int i = 0; i < 10000; i++)
            //{
            //    l.Add(new MyClass());
            //}
            //tm = DateTime.UtcNow;
            //foreach (MyClass nu in l)
            //{
            //    nu.MyProperty = 4;
            //}
            //res = DateTime.UtcNow.Subtract(tm);
            //Console.WriteLine(res);
            //tm = DateTime.UtcNow;
            //it's faster! On Read about 2 times, on write about 10 - 25%
            //for (int rg = 0; rg < l.Count; rg++)
            //{
            //    l[rg].MyProperty = 4;
            //}
            //res = DateTime.UtcNow.Subtract(tm);
            //Console.WriteLine(res);
            Thread.Sleep(1000000000);
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
            for (int t = 0; t < 50; t++)
            {
                DateTime temp;
                TimeSpan[] result = new TimeSpan[2000];
                IPEndPoint ipAndPort = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                //2000
                temp = DateTime.UtcNow;

                for (int y = 0; y < MyList.Count; y++)
                {
                    //not 2000 but less
                    //if ((y % 10) == 0)
                    //{
                        lock (loker)
                        {
                            byte[] bytes = Encoding.ASCII.GetBytes("325bp945nbpoidyushdp875908mqeyrgiuqp57jvq84ytp8 eygt0-3 745-v89uqemroti nqpty some long long data..........." + MyList[y]);
                            udp.Send(bytes, bytes.Length, ipAndPort);
                        }
                    //}
                    //result[y] = DateTime.UtcNow.Subtract(temp);
                }
                TimeSpan total = DateTime.UtcNow.Subtract(temp);
                //TimeSpan total = TimeSpan.Zero;
                //foreach (TimeSpan time in result)
                //{
                //    total = total + time;
                //}
                Console.WriteLine($"total: {total}");
                //if total time of work less than 10 ms - make sleep
                if (total < new TimeSpan(0,0,0,0,10))
                {
                    Thread.Sleep(10);
                }
            }

            Console.WriteLine("Any key...");
            Console.ReadLine();
        }
    }
}
