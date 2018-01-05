using System;
using System.Diagnostics;
using WorkerRole1;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            Trace.WriteLine("Instantiating WorkerRole...");
            var role = new WorkerRole();

            Trace.WriteLine("Invoking role.OnStart()...");

            role.OnStart();

            Trace.WriteLine("Invoking role.Run()...");
            role.Run();


            Trace.WriteLine("Hit any key to exit.");
            Console.ReadKey();
        }
    }
}
