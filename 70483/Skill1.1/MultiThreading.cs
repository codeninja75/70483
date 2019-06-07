﻿namespace DotNet.E70483.ProgramFlow
{
    using DotNet.E70483.Helpers;
    using System;
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
        public static void Parallel_Invoke()
        {
            using (Benchmark b = new Benchmark("Parallel Task Run"))
            {

                Parallel.Invoke(() => Task1(), () => Task2());
            }
        }
    }
}