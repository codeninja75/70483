using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DotNet.E70483.Helpers;
namespace DotNet.E70483.ProgramFlow
{
    public class ManageMultiThreading
    {
        public static void Slicing()
        {
            using (Benchmark b = new Benchmark("Dumb way to attack"))
            {
                Console.WriteLine("This is dumb");
                int[] items = Enumerable.Range(0, 5000000).ToArray();
                List<int> primes = new List<int>();
                foreach (var v in items)
                {
                    if (Primes.isPrime(v))
                        primes.Add(v);
                }
                //Console.WriteLine(primes.PrettyPrint());
                Console.WriteLine("Count is :" + primes.ToList().Count.ToString());
            }
            Console.WriteLine("Press anykey to continue...");
            Console.ReadLine();
            
            using (Benchmark b = new Benchmark("Also Dumb way to attack"))
            {
                List<int> primes = new List<int>();
                try
                {
                    Console.WriteLine("This is even dumber");
                    int[] items = Enumerable.Range(0, 5000000).ToArray();

                    Parallel.ForEach(items, v =>
                    {
                        if (Primes.isPrime(v))
                            primes.Add(v);

                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                //Console.WriteLine(primes.PrettyPrint());
                Console.WriteLine("Count is :" + primes.ToList().Count.ToString());
            }
            Console.WriteLine("Press anykey to continue...");
            Console.ReadLine();
            
            using (Benchmark b = new Benchmark("Behold the power of parallel and a concurrent data structure"))
            {
                ConcurrentBag<int> primes = new ConcurrentBag<int>();
                try
                {
                    Console.WriteLine("This is smarter");
                    int[] items = Enumerable.Range(0, 5000000).ToArray();

                    Parallel.ForEach(items, v =>
                    {
                        if (Primes.isPrime(v))
                            primes.Add(v);

                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                //Console.WriteLine(primes.ToList().PrettyPrint());
                Console.WriteLine("Count is :" + primes.ToList().Count.ToString());
            }
            Console.WriteLine("Press anykey to continue...");
            Console.ReadLine();
            

            using (Benchmark b = new Benchmark("Behold the power of parallel and a non-concurrent data structure"))
            {
                int primeCount = 0;
                try
                {
                    Console.WriteLine("Bad counting!");
                    int[] items = Enumerable.Range(0, 5000000).ToArray();

                    Parallel.ForEach(items, v =>
                    {
                        if (Primes.isPrime(v))
                            primeCount++;

                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine("Count is :" + primeCount.ToString());
            }
            Console.WriteLine("Press anykey to continue...");
            Console.ReadLine();
            

            using (Benchmark b = new Benchmark("Behold the power of parallel and a non-concurrent data structure with Interlock"))
            {
                int primeCount = 0;
                try
                {
                    Console.WriteLine("This is smarter");
                    int[] items = Enumerable.Range(0, 5000000).ToArray();

                    Parallel.ForEach(items, v =>
                    {
                        if (Primes.isPrime(v))
                            Interlocked.Increment(ref primeCount);

                    });
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                Console.WriteLine("Count is :" + primeCount.ToString());
            }
            Console.WriteLine("Press anykey to continue...");
            Console.ReadLine();
            

        }
        public static void GoodAndBadSums()
        {
            BadLocker.Reset();
            using (Benchmark b = new Benchmark("Wow, this is bad"))
            {
                Console.WriteLine("Starting bad idea");
                BadLocker.BadAddRange(45890, 345989);
                Console.WriteLine(BadLocker.SharedTotal);
            }
            BadLocker.Reset();
            using (Benchmark b = new Benchmark("Much better"))
            {
                Console.WriteLine("Starting better idea");
                BadLocker.GoodAddRange(45890, 345989);
                Console.WriteLine(BadLocker.SharedTotal);
            }

        }

        public static class BadLocker
        {
            static int[] items = Enumerable.Range(0, 5000000).ToArray();
            static object SharedTotalLock = new object();
            public static long SharedTotal { get; set; }
            public static void BadAddRange(int start, int end)
            {
                while(start <end)
                {
                    lock(SharedTotalLock)
                    {
                        SharedTotal = SharedTotal + items[start];
                    }
                    start++;
                }
            }
            public static void Reset()
            {
                lock (SharedTotalLock)
                {
                    SharedTotal = 0;
                }

            }
            public static void GoodAddRange(int start, int end)
            {
                long subTotal = 0;
                while (start < end)
                {
                    subTotal= subTotal+ items[start];

                    start++;
                }
                lock(SharedTotalLock)
                {
                    SharedTotal = subTotal + SharedTotal;
                }
            }
        }
    }
}
