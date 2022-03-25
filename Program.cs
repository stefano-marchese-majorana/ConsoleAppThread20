// esempio con tutto quello visto negli esempi precendenti

using System;
using System.Collections.Generic;
using System.Threading;

namespace ConsoleAppThread20
{
    public class MyData
    {
        public static int MaxLoopNumber = 5;
        public static bool IsBackground = false;

        public static object DataLock = new object();
        public static int DataShared { get; set; }
    }

    public class TH
    {

        // Method1
        public static void Method(Object slp)
        {
            int sleep = Convert.ToInt32(slp);

            string name = Thread.CurrentThread.Name;
            ThreadPriority priority = Thread.CurrentThread.Priority;
            Type type = Thread.CurrentThread.GetType();
            int tid = Thread.CurrentThread.ManagedThreadId;

            Console.Write($"Thread({name}) ");
            Console.Write($"sleep({slp}) ");
            Console.Write($"tid({tid}) ");
            Console.Write($"prioprity({priority}) ");
            Console.WriteLine($"type({type})");

            for (int i = 0; i < MyData.MaxLoopNumber; i++)
            {
                lock (MyData.DataLock)
                {
                    int temp = MyData.DataShared;
                    Console.WriteLine($"Thread({name}) " +
                        $"Loop({i}) " +
                        $"DataShared({MyData.DataShared})");
                    Thread.Yield();
                    MyData.DataShared = temp + 1;
                    Console.WriteLine($"Thread({name}) " +
                        $"Loop({i}) " +
                        $"DataShared({MyData.DataShared})");
                }

                Console.WriteLine($"Sleeping Thread({name}) Sleep({sleep})");
                Thread.Sleep(sleep);
            }

            Console.WriteLine($"Stopping Thread({name})");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };
            List<Thread> ths = new List<Thread>();

            foreach (string s in cars)
            {
                Console.WriteLine($"Creating thread {s}");
                Thread th = new Thread(TH.Method);
                th.Name = s;
                th.IsBackground = MyData.IsBackground;
                ths.Add(th);
            }

            foreach (Thread th in ths)
            {
                Random rd = new Random();
                int sleep = rd.Next(0, 6) * 1000;
                th.Start(sleep);
            }

            foreach (Thread th in ths)
            {
                Console.WriteLine($"Main tests {th.Name} is alive: {th.IsAlive}");
            }

            if (!MyData.IsBackground)
            {
                foreach (Thread th in ths)
                {
                    th.Join();
                }
            }

            foreach (Thread th in ths)
            {
                Console.WriteLine($"Main tests {th.Name} is alive: {th.IsAlive}");
            }

            Console.WriteLine($"Data shared {MyData.DataShared}");
            Console.WriteLine($"Stopping Main thread");
        }
    }
}
