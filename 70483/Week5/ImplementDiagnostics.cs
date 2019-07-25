#define BOB
using System;
using System.Diagnostics;

namespace DotNet.E70483.DebugAndSecurity
{
    public class ImplementDiagnostics
    {
        public void DoSomeDiag()
        {
#if BOB
            Console.WriteLine("Bob is here!");
#endif

#if DEBUG
            Console.WriteLine("We are debugging");
#endif

        }

    }
    public class CustomListener : TraceListener
    {

        
        public  void Writex(string message, [System.Runtime.CompilerServices.CallerMemberName] string memberName = "",
        [System.Runtime.CompilerServices.CallerFilePath] string sourceFilePath = "",
        [System.Runtime.CompilerServices.CallerLineNumber] int sourceLineNumber = 0)
        {
            throw new NotImplementedException();
        }

        public override void Write(string message) 
        {
            throw new NotImplementedException();
        }

        public override void WriteLine(string message)
        {
            throw new NotImplementedException();
        }
    }
}
