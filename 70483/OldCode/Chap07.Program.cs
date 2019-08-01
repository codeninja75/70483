using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Globalization;
namespace Chap7
{
    class Program
    {
        static Temp z;
        static void Main(string[] args)
        {
            int res = 0;
            while (res >= 0)
            {
                
                Console.WriteLine("For ThreadTest1 press  1");
                Console.WriteLine("For ThreadTest2 press  2");
                Console.WriteLine("For ThreadTest3 press  3");
                Console.WriteLine("For ThreadTest4 press  4");
                Console.WriteLine("For ThreadTest5 press  5");
                Console.WriteLine("For ExecContext press  6");
                Console.WriteLine("For Interloc    press  7");
                Console.WriteLine("For LockTest    press  8");
                Console.WriteLine("For MonitorTest press  9");
                Console.WriteLine("For RWLockTest  press 10");
                Console.WriteLine("For MutextTest  press 11");
                Console.WriteLine("For Semaphore   press 12");
                Console.WriteLine("For EventTest   press 13");
                Console.WriteLine("For APMTest     press 14");
                Console.WriteLine("For ThreadPool  press 15");
                Console.WriteLine("For IAsync..    press 16");
                string t = Console.ReadLine();
                if (t.ToUpper() != "Q")
                {
                    if (int.TryParse(t, out res))
                    {
                        switch (res)
                        {
                            case 1: ThreadTest1();
                                break;
                            case 2: ThreadTest2();
                                break;
                            case 3: ThreadTest3();
                                break;
                            case 4: ThreadTest4();
                                break;
                            case 5: ThreadTest5();
                                break;
                            case 6: ExecContext();
                                break;
                            case 7: InterLockTest();
                                break;
                            case 8: LockTest();
                                break;
                            case 9: MonitorTest();
                                break;
                            case 10: RWLockTest();
                                break;
                            case 11: MutexTest();
                                break;
                            case 12: SemaphoreTest();
                                break;
                            case 13: EventTest();
                                break;
                            case 14: APMTest();
                                break;
                            case 15: ThreadPoolTest();
                                break;
                            case 16: IAsyncResultTest();
                                break;



                        }
                        Console.WriteLine("Hit return to clear and see the menu again.");
                        Console.ReadLine();
                        Console.Clear();
                    }
                }
                else
                    res = -1;
            }
            
        }

        static void ThreadTest1()
        {
            Console.WriteLine("Creating new threads");
            List<Thread> lt = new List<Thread>();
            ThreadStart t = new ThreadStart(SimpleWork);
            for (int i = 0; i < 5; i++)
            {
                lt.Add(new Thread(t));
                lt[i].Start();
            }
            
        }
        static void SimpleWork()
        {
            Console.WriteLine("Thread:{0}", Thread.CurrentThread.ManagedThreadId);
        }
        static void ThreadTest2()
        {
            Console.WriteLine("Threads of a different priority");
            List<Thread> lt = new List<Thread>();
            ThreadStart t = new ThreadStart(SimpleWork1);
            for (int i = 0; i < 5; i++)
            {
                lt.Add(new Thread(t));
                switch (i)
                {
                    case 0: lt[i].Priority = ThreadPriority.Lowest;
                        break;
                    case 1: lt[i].Priority = ThreadPriority.Highest;
                        break;
                    case 2: lt[i].Priority = ThreadPriority.Normal;
                        break;
                    case 3: lt[i].Priority = ThreadPriority.AboveNormal;
                        break;
                    case 4: lt[i].Priority = ThreadPriority.BelowNormal;
                        break;
                    default:
                        break;
                }
                lt[i].Start();
            }
            Console.WriteLine("Waiting!");
            foreach (Thread t1 in lt)
            {
                t1.Join();
            }
            Console.WriteLine("Done!");
            
        }
        static void SimpleWork1()
        {
            for (int x = 0; x < 10; x++)
            {
                Console.WriteLine("Thread:{0} {1} , {2}", Thread.CurrentThread.ManagedThreadId, x, Thread.CurrentThread.Priority);
                Thread.Sleep(x*10);
            }
        }
        static void ThreadTest3()
        {
            Temp tz = new Temp();
            Console.WriteLine("Bad threading.");
            List<Thread> lt = new List<Thread>();
            //ThreadStart t = new ThreadStart(SimpleWork1)
            ParameterizedThreadStart pt = new ParameterizedThreadStart(NotSoSimpleWork);
            for (int i = 0; i < 10; i++)
            {
                lt.Add(new Thread(pt));
                lt[i].Start(tz);
            }
            Console.WriteLine("Waiting!");
            foreach (Thread t1 in lt)
            {
                t1.Join();
            }
            Console.WriteLine("tz.iCount = {0} (Should be 100000)", tz.iCount);
            Console.WriteLine("Done!");
            
        }
        static void NotSoSimpleWork(object o)
        {
            if (o.GetType() == typeof(Temp))
            {
                Temp t = (Temp)o;
                for (int i = 0; i < 10000; i++)
                {
                    int z = t.iCount;
                    double f = 5.55;
                    f = f * 2;
                    f = f * f;
                    f = i * f;
                    z = t.iCount - 1;
                    Thread.Sleep(1);
                    t.iCount = z + 2;


                }
                //Console.WriteLine("Thread:{0}  iCount={1}", Thread.CurrentThread.ManagedThreadId, t.iCount);

            }

        }
        static void NotSoSimpleWork2(object o)
        {
            Console.WriteLine("In thread {0}", Thread.CurrentThread.ManagedThreadId);
            if (o.GetType() == typeof(Temp))
            {
                Temp t = (Temp)o;
                for (int i = 0; i < 10000; i++)
                {
                    int z = t.iCount;
                    double f = 5.55;
                    f = f * 2;
                    f = f * f;
                    f = i * f;
                    z = t.iCount - 1;
                    Thread.Sleep(1);
                    t.iCount = z + 2;


                }
                //Console.WriteLine("Thread:{0}  iCount={1}", Thread.CurrentThread.ManagedThreadId, t.iCount);

            }

        }     
        /*
        static void NotSoSimpleWork2(object o)
        {
            if (o.GetType() == typeof(Temp))
            {
                Temp t = (Temp)o;
                for (int i = 0; i < 50; i++)
                {
                    t.iCount++;
                    Console.WriteLine("Thread:{0} {1} iCount={2}", Thread.CurrentThread.ManagedThreadId, i, t.iCount);

                }

            }


        }
         */
        static void ThreadTest4()
        {
            Console.WriteLine("Thread without Critial section");
            Thread newt = new Thread(new ThreadStart(AbortWork));
            //z = new Temp();
            newt.Start();
            Thread.Sleep(11);
            newt.Abort();
            Console.WriteLine("z.isComplete = {0}", z.isComplete);
            Console.WriteLine("z.isValid = {0}", z.isValid);
            
        }
        static void AbortWork()
        {
            Console.WriteLine("AbortWork");
            z = new Temp();
            z.isValid = true;
            Thread.Sleep(100);
            z.isComplete = true;
            Console.WriteLine(z.ToString());
        }
        static void ThreadTest5()
        {
            Console.WriteLine("Thread With Critical Section");
            Thread newt = new Thread(new ThreadStart(CriticalAbort));
            //z = new Temp();
            newt.Start();
            Thread.Sleep(40);
            newt.Abort();
            Console.WriteLine("z.isComplete = {0}", z.isComplete);
            Console.WriteLine("z.isValid = {0}", z.isValid);
            
        }
        static void CriticalAbort()
        {
            Console.WriteLine("CritialAbort");
            Thread.BeginCriticalRegion();
            z = new Temp();
            z.isValid = true;
            Thread.SpinWait(50);
            z.isComplete = true;
            Thread.EndCriticalRegion();
            Console.WriteLine(z.ToString());
        }
        static void ExecContext()
        {
            Console.WriteLine("Exec Context");
            Thread.CurrentThread.CurrentCulture = new CultureInfo("DE-de");
            Console.WriteLine("Current Culture:" + Thread.CurrentThread.CurrentCulture.ToString());
            AsyncFlowControl flow = ExecutionContext.SuppressFlow();
            Thread t = new Thread(new ThreadStart(ExecWork));
            t.Start();
            t.Join();
            ExecutionContext.RestoreFlow();
            ExecutionContext exc = ExecutionContext.Capture();
            ExecutionContext.Run(exc, new ContextCallback(ExecWork1), null);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(1033);
            
        }
        static void ExecWork()
        {
            Console.WriteLine("Current Culture:" + Thread.CurrentThread.CurrentCulture.ToString());
        }
        static void ExecWork1(object o)
        {
            Console.WriteLine("Current Culture:" + Thread.CurrentThread.CurrentCulture.ToString());
        }
        static void InterLockTest()
        {
            Temp tz = new Temp();
            Console.WriteLine("Interlock bad threading.");
            List<Thread> lt = new List<Thread>();
            //ThreadStart t = new ThreadStart(SimpleWork1)
            ParameterizedThreadStart pt = new ParameterizedThreadStart(InterlockWork);
            for (int i = 0; i < 10; i++)
            {
                lt.Add(new Thread(pt));
                lt[i].Start(tz);
            }
            Console.WriteLine("Waiting!");
            foreach (Thread t1 in lt)
            {
                t1.Join();
            }
            Console.WriteLine("tz.iCount = {0} (Should be 100000)", tz.iCount);
            Console.WriteLine("Done!");
        }
        static void InterlockWork(object o)
        {
            if (o.GetType() == typeof(Temp))
            {
                Temp t = (Temp)o;
                for (int i = 0; i < 10000; i++)
                {
                    int z = t.iCount;
                    double f = 5.55;
                    f = f * 2;
                    f = f * f;
                    f = i * f;
                    z = t.iCount - 1;
                    Thread.Sleep(1);
                    //t.iCount = z + 2;
                    Interlocked.Add(ref t.iCount, 1);


                }
                //Console.WriteLine("Thread:{0}  iCount={1}", Thread.CurrentThread.ManagedThreadId, t.iCount);

            }
        }
        static void LockTest()
        {
            Counter c = new Counter();
            Console.WriteLine("Lock Testing");
            List<Thread> lt = new List<Thread>();
            //ThreadStart t = new ThreadStart(SimpleWork1)
            ParameterizedThreadStart pt = new ParameterizedThreadStart(LockWork1);
            for (int i = 0; i < 10; i++)
            {
                lt.Add(new Thread(pt));
                lt[i].Start(c);
            }
            Console.WriteLine("Waiting!");
            foreach (Thread t1 in lt)
            {
                t1.Join();
            }
            Console.WriteLine("Count = {0} Even Count = {1} (Should be 100000 & 50000)", c.Count , c.EvenCount);
            Console.WriteLine("Done!");
            c = new Counter();
            lt.Clear();
            pt = new ParameterizedThreadStart(LockWork2);
            for (int i = 0; i < 10; i++)
            {
                lt.Add(new Thread(pt));
                lt[i].Start(c);
            }
            Console.WriteLine("Waiting!");
            foreach (Thread t1 in lt)
            {
                t1.Join();
            }
            Console.WriteLine("Count = {0} Even Count = {1} (Should be 100000 & 50000)", c.Count, c.EvenCount);
            Console.WriteLine("Done!");


        }
        static void LockWork1(object o)
        {
            Counter c;
            if (o.GetType() == typeof(Counter))
            {
                c = (Counter)o;
                for (int x = 0; x < 10000; x++)
                {
                    c.BadUpdateCount();
                }
            }
        }
        static void LockWork2(object o)
        {
            Counter c;
            if (o.GetType() == typeof(Counter))
            {
                c = (Counter)o;
                for (int x = 0; x < 10000; x++)
                {
                    c.GoodUpdateCount();
                }
            }

        }
        static void MonitorTest()
        {
            Counter c = new Counter();
            Console.WriteLine("Lock Testing");
            List<Thread> lt = new List<Thread>();
            //ThreadStart t = new ThreadStart(SimpleWork1)
            ParameterizedThreadStart pt = new ParameterizedThreadStart(MonitorWork);
            for (int i = 0; i < 10; i++)
            {
                lt.Add(new Thread(pt));
                lt[i].Start(c);
            }
            Console.WriteLine("Waiting!");
            foreach (Thread t1 in lt)
            {
                t1.Join();
            }
            Console.WriteLine("Count = {0} Even Count = {1} (Should be 100000 & 50000)", c.Count, c.EvenCount);
            Console.WriteLine("Done!");
        }
        static void MonitorWork(object o)
        {
            Counter c;
            if (o.GetType() == typeof(Counter))
            {
                c = (Counter)o;
                for (int x = 0; x < 10000; x++)
                {
                    c.MonitorUpdateCount();
                }
            }
        }
        static void RWLockTest()
        {
            Counter c = new Counter();
            Console.WriteLine("Lock Testing");
            List<Thread> lt = new List<Thread>();
            //ThreadStart t = new ThreadStart(SimpleWork1)
            ParameterizedThreadStart pt = new ParameterizedThreadStart(RWWork);
            for (int i = 0; i < 10; i++)
            {
                lt.Add(new Thread(pt));
                lt[i].Start(c);
            }
            Console.WriteLine("Waiting!");
            foreach (Thread t1 in lt)
            {
                t1.Join();
            }
            Console.WriteLine("Count = {0} Even Count = {1} (Should be 100000 & 50000)", c.Count, c.EvenCount);
            Console.WriteLine("Done!");
        }
        static void RWWork(object o)
        {
            Counter c;
            if (o.GetType() == typeof(Counter))
            {
                c = (Counter)o;
                for (int x = 0; x < 10000; x++)
                {
                    c.RWLockUpdateCount();
                }
            }
 
        }
        static void MutexTest()
        {
        }
        static void SemaphoreTest()
        { }
        static void EventTest()
        { }
        static void APMTest()
        { }
        static void ThreadPoolTest()
        {
            Temp t = new Temp();
            WaitCallback TPWork = new WaitCallback(NotSoSimpleWork2);
            if (!ThreadPool.QueueUserWorkItem(TPWork, t))
            {
                Console.WriteLine("Could not queue item!");
            }
            int threads, completionPorts;
            ThreadPool.GetMaxThreads(out threads, out completionPorts);
            ThreadPool.SetMaxThreads(threads + 10, completionPorts + 100);
            ThreadPool.GetMinThreads(out threads, out completionPorts);
            ThreadPool.SetMinThreads(threads + 10, completionPorts + 100);
            Mutex mymutex = new Mutex(true);
            ThreadPool.RegisterWaitForSingleObject(mymutex , new WaitOrTimerCallback(MutexHasFired), null, Timeout.Infinite, true);
            mymutex.ReleaseMutex();
        }

        static void MutexHasFired(object state, bool timeOut)
        {
            if (timeOut)
            {
                Console.WriteLine("Mutex Timed out!");
            }
            else
            {
                Console.WriteLine("Mutex was signaled!");
            }
        }

        static void IAsyncResultTest()
        { }

    }

    class Temp 
    {
        public int iCount;
        public bool isValid;
        public bool isComplete;

    }
    class Counter
    {
        private Mutex _mu;
        private Semaphore _sp;
        private ReaderWriterLock _rwl;
        public Counter()
        {
            _rwl = new ReaderWriterLock();
            try
            {
               _mu = Mutex.OpenExisting("CWMCounter");
            }
            catch (WaitHandleCannotBeOpenedException)
            { }
            if (_mu == null)
            {
                _mu = new Mutex(false , "CWMCounter");
            }
            _sp = new Semaphore(0, 5);
        }
        private int _count = 0;
        public int Count
        {
            get { return _count; }
        }
        private int _evenCount;
        public int EvenCount
        {
            get { return _evenCount; }
        }

        public void BadUpdateCount()
        {
            _count = Interlocked.Increment(ref _count);
            Thread.Sleep(5);
            if (_count % 2 == 0)
            {
                _evenCount = Interlocked.Increment(ref _evenCount);
            }
        }

        public void GoodUpdateCount()
        {
            lock (this)
            {
                _count++;
                if (_count % 2 == 0)
                {
                    Thread.Sleep(1);
                    _evenCount++;
                }
            }
        }

        public void MonitorUpdateCount()
        {
            Monitor.Enter(this);
            try
            {
                _count++;
                if (_count % 2 == 0)
                    _evenCount++;
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        public void RWLockUpdateCount()
        {
            try
            {

                _rwl.AcquireReaderLock(1000);
                LockCookie lc = _rwl.UpgradeToWriterLock(1000);
                _count++;
                if (_count % 2 == 0)
                    _evenCount++;
                _rwl.DowngradeFromWriterLock(ref lc);
                _rwl.ReleaseLock();
            }
            catch (Exception)
            {
                Console.WriteLine("Had Exception!");
            }
        }

        public void MutexCount()
        {
            if (_mu.WaitOne(-1, false))
            {
                try
                {
                    _count++;
                    if (_count % 2 == 0)
                        _evenCount++;
                }
                finally
                {
                    _mu.ReleaseMutex();
                }
            }
        }

        public void SemaphoreCount()
        {

        }

    }
}
