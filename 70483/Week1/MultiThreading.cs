namespace DotNet.E70483.ProgramFlow
{
    using DotNet.E70483.Helpers;
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class MultiThreading
    {
        
        static void Task1()
        {
            using (Benchmark b = new Benchmark("Task 1 prime factor find"))
            {
                Eratosthenes eratosthenes = new Eratosthenes(); 
                Console.WriteLine($"Task 1 starting {DateTime.Now.ToShortTimeString()}");
                Console.WriteLine(Primes.GetPrimeFactors(41724259, eratosthenes).PrettyPrint());
                Console.WriteLine($"Task 1 ending {DateTime.Now.ToShortTimeString()}");
            }
        }
        static void Task2()
        {
            using (Benchmark b = new Benchmark("Task 2 prime factor find"))
            {
                Eratosthenes eratosthenes = new Eratosthenes();
                Console.WriteLine($"Task 2 starting {DateTime.Now.ToShortTimeString()}");
                Console.WriteLine(Primes.GetPrimeFactors(13187259, eratosthenes).PrettyPrint());
                Console.WriteLine($"Task 2 ending {DateTime.Now.ToShortTimeString()}");
            }
        }
        public static void Non_Parallel_Invoke()
        {
            using (Benchmark b = new Benchmark("Non parallel task run "))
            {

                Task1();
                Task2();
            }
        }
        public static void Parallel_Invoke_Foreach()
        {
            using (Benchmark b = new Benchmark("Parallel Task Run"))
            {

                Parallel.Invoke(() => Task1(), () => Task2());
            }
            using (Benchmark b = new Benchmark("Parallel Foreach Run"))
            {

                List<Action> list = new List<Action>();
                list.Add(Task1);
                list.Add(Task2);
                Parallel.ForEach(list, t => t());
            }
        }
       
        public static void TaskUse()
        {
            using (Benchmark b = new Benchmark("Using Tasks "))
            {

                Task.Run( () => Task1());
                Task.Run( () => Task2());
            }
        }
        public static void TaskReturnValues()
        {
            using (Benchmark b = new Benchmark("Returning values from tasks"))
            {

                
                Task<string> task = Task.Run(() =>
                {
                    Eratosthenes eratosthenes = new Eratosthenes();
                   return Primes.GetPrimeFactors(13187259, eratosthenes).PrettyPrint();
                    });
                
                Task<string> task2 = Task.Run(() =>
                {
                    Eratosthenes eratosthenes = new Eratosthenes();
                    return Primes.GetPrimeFactors(41724259, eratosthenes).PrettyPrint();
                    });
                Console.WriteLine(task.Result);
                Console.WriteLine(task2.Result);
            }
        }
       
        
        public static void RawThread()
        {
            using (Benchmark b = new Benchmark("Using raw threads"))
            {

                Thread t1 = new Thread(Task1);
                Thread t2 = new Thread(Task2);
                t1.Start();
                t2.Start();
                t1.Join();
                t2.Join();
            }
        }
        public static void ThreadAndLambda()
        {
            using (Benchmark b = new Benchmark("Using threads via lambdas"))
            {

                Thread t1 = new Thread(() =>
                {
                    Eratosthenes eratosthenes = new Eratosthenes();
                    Console.WriteLine($"Task 1 starting {DateTime.Now.ToShortTimeString()}");
                    Console.WriteLine(Primes.GetPrimeFactors(13187259, eratosthenes).PrettyPrint());
                    Console.WriteLine($"Task 1 ending {DateTime.Now.ToShortTimeString()}");
                }
                );
                Thread t2 = new Thread(()=>
                {
                    Eratosthenes eratosthenes = new Eratosthenes();
                    Console.WriteLine($"Task 2 starting {DateTime.Now.ToShortTimeString()}");
                    Console.WriteLine(Primes.GetPrimeFactors(41724259, eratosthenes).PrettyPrint());
                    Console.WriteLine($"Task 2 ending {DateTime.Now.ToShortTimeString()}");
                }
                );
                t1.Start();
                t2.Start();
                t1.Join();
                t2.Join();
            }
        }
       

    }
}
