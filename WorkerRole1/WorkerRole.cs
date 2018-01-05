using ContentStripeApi.FeedAccess;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerRole1
{
    public class WorkerRole : RoleEntryPoint
    {
        private readonly CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
        private readonly ManualResetEvent runCompleteEvent = new ManualResetEvent(false);

        public override void Run()
        {
            Trace.TraceInformation("WorkerRole1 is running");

            Trace.WriteLine("Inside WorkerRole.RunAsync()");
            // See app.config & https://docs.postman-echo.com/ for details
            ApiResource resource = new ApiResource(new Config());

            Trace.WriteLine("Executing HTTP GET");

            var result = resource.GetAsync<PostManResponse>("https://postman-echo.com/basic-auth").Result;

            Trace.WriteLine($"HTTP GET responded with authenticated={result.authenticated}");

            // VP - I'm just going to let this run once and then exit.

        }

        public override bool OnStart()
        {
            // Set the maximum number of concurrent connections
            ServicePointManager.DefaultConnectionLimit = 12;

            // For information on handling configuration changes
            // see the MSDN topic at https://go.microsoft.com/fwlink/?LinkId=166357.

            bool result = base.OnStart();

            Trace.TraceInformation("WorkerRole1 has been started");

            return result;
        }

        public override void OnStop()
        {
            Trace.TraceInformation("WorkerRole1 is stopping");

            this.cancellationTokenSource.Cancel();
            this.runCompleteEvent.WaitOne();

            base.OnStop();

            Trace.TraceInformation("WorkerRole1 has stopped");
        }
    }
}
