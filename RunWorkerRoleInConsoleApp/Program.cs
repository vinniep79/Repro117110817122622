using System;
using System.Diagnostics;
using WorkerRole1;

namespace RunWorkerRoleInConsoleApp
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
            Console.ReadKey();
        }
    }
}
