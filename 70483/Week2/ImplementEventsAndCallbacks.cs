using System;

namespace DotNet.E70483.ProgramFlow
{
    public class ImplementEventsAndCallbacks
    {
        public static void CantStopTheSignal()
        {
            Signal s = new Signal();
            s.OnBasicSignaled += TestSignal1;
            s.OnBasicSignaled += TestSignal2;
            s.SignalThisDumb();
            s.OnBasicSignaled -= TestSignal1;
            s.SignalThisDumb();
            s.OnBetterSignaled += S_OnBetterSignaled;
            s.SignalThisBetter();
            s.OnBasicEvent += S_OnBasicEvent;
            s.BasicEvent();
            s.OnGenericEvent += S_OnGenericEvent;
            s.GenericEvent(new SignalEventArgs("Woot!"));

            Signal.IntOperation intOp;
            intOp = (x,y) =>  x * y;
            Console.WriteLine($"Op(4,5) = {intOp(4, 5).ToString()}");
            int z = 9;
            intOp = (x, y) => { return x + y - z; };
            Console.WriteLine($"(secret closure!) Op(4,5) = {intOp(4, 5).ToString()}");
        }

        private static void S_OnGenericEvent(object sender, SignalEventArgs e)
        {
            Console.WriteLine($"TestSignal5 : This is an event. {e.Message}");
        }

        private static void S_OnBasicEvent(object sender, EventArgs e)
        {
            Console.WriteLine("TestSignal4 : This is an event.");
        }

        private static void S_OnBetterSignaled()
        {
            Console.WriteLine("TestSignal3 : This is still not so smart.");
        }

        private static void TestSignal2()
        {
            Console.WriteLine("TestSignal2 : This is not so smart.");
        }

        private static void TestSignal1()
        {
            Console.WriteLine("TestSignal1 : This is not so smart.");
        }
    }

    public class Signal
    {
        public delegate int IntOperation(int a, int b);
        public Action OnBasicSignaled { get; set; }

        
        public void SignalThisDumb()
        {
            Console.WriteLine("I aim to misbehave");
            if (OnBasicSignaled != null)
            {
                OnBasicSignaled();
            }
        }

        public event Action OnBetterSignaled = delegate { };
        public void SignalThisBetter()
        {
            Console.WriteLine("Try and stop this signal");
            OnBetterSignaled();
        }
        public event EventHandler OnBasicEvent = delegate { };
        public void BasicEvent()
        {
            OnBasicEvent(this, EventArgs.Empty);
        }
        public event EventHandler<SignalEventArgs> OnGenericEvent = delegate { };
        public void GenericEvent(SignalEventArgs args)
        {
            OnGenericEvent(this, args);
        }
    }
    public class SignalEventArgs : EventArgs
    {
        public string Message { get; set; }
        public SignalEventArgs(string message)
        {
            Message = message;
        }
    }
}
