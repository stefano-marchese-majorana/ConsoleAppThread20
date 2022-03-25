// esempio con tutto quello visto negli esempi precendenti

using System;
using System.Threading;

namespace ConsoleAppThread20
{
    public class MyData
    {
        public static object DataLock = new object();
        public static int DataShared { get; set; }
    }

    public class TH
    {

        // Method1
        public static void Method()
        {
            for (int i = 0; i < 10; i++)
            {
                lock (MyData.DataLock)
                {
                    int temp = MyData.DataShared;
                    Console.WriteLine($"THREAD {Thread.CurrentThread.Name}" +
                        $"({i})({MyData.DataShared})");
                    Thread.Yield();
                    MyData.DataShared = temp + 1;
                    Console.WriteLine($"THREAD {Thread.CurrentThread.Name}" +
                        $"({i})({MyData.DataShared})");
                }
            }
            Console.WriteLine($"THREAD {Thread.CurrentThread.Name} stopping");
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            string[] cars = { "Volvo", "BMW", "Ford", "Mazda" };
            Thread[] ths = new Thread[4];

            int i = 0;
            foreach (string s in cars)
            {
                Console.WriteLine($"Creating and starting thread {s}");
                ths[i] = new Thread(TH.Method);
                ths[i].Name = s;
                ths[i].IsBackground = false;
                ths[i].Start();
                i++;
            }

            foreach (Thread th in ths)
            {
                th.Join();
            }

            Console.WriteLine($"Data shared {MyData.DataShared}");
            Console.WriteLine($"Main thread stopping");
        }
    }
}
