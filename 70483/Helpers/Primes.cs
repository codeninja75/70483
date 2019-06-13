using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DotNet.E70483.Helpers
{
    public class Primes
    {
        public static IEnumerable<int> GetPrimeFactors(int value, Eratosthenes eratosthenes)
        {
            List<int> factors = new List<int>();

            foreach (int prime in eratosthenes)
            {
                while (value % prime == 0)
                {
                    value /= prime;
                    factors.Add(prime);
                }

                if (value == 1)
                {
                    break;
                }
            }

            return factors;
        }
        public static bool isPrime(int number)
        {

            if (number == 1) return false;
            if (number == 2) return true;

            var limit = Math.Ceiling(Math.Sqrt(number)); //hoisting the loop limit

            for (int i = 2; i <= limit; ++i)
            {
                if (number % i == 0) return false;
            }

            return true;

        }
    }
    public class Eratosthenes : IEnumerable<int>
    {
        private List<int> _primes = new List<int>();
        private int _lastChecked;

        public Eratosthenes()
        {
            _primes.Add(2);
            _lastChecked = 2;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public IEnumerator<int> GetEnumerator()
        {
            foreach (int prime in _primes)
            {
                yield return prime;
            }

            while (_lastChecked < int.MaxValue)
            {
                _lastChecked++;

                if (IsPrime(_lastChecked))
                {
                    _primes.Add(_lastChecked);
                    yield return _lastChecked;
                }
            }
        }
        private bool IsPrime(int checkValue)
        {
            bool isPrime = true;

            foreach (int prime in _primes)
            {
                if ((checkValue % prime) == 0 && prime <= Math.Sqrt(checkValue))
                {
                    isPrime = false;
                    break;
                }
            }

            return isPrime;
        }
    }
}
