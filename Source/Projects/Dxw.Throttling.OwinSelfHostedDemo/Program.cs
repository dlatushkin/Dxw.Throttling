namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using System;
    using Microsoft.Owin.Hosting;
    using System.Net.Http;
    using System.Threading.Tasks;

    class Program
    {
        private const string baseAddr = "http://localhost:8001/";

        static void Main(string[] args)
        {
            Task.Run(Run);
            Console.ReadKey();
        }

        static async Task Run()
        {
            using (WebApp.Start<Startup>(baseAddr))
            {
                Console.WriteLine("started at " + baseAddr);

                var client = new HttpClient();

                Console.WriteLine(await CallAsync("api/first"));
                Console.WriteLine(await CallAsync("api/second"));

                Console.WriteLine("test complete");
                Console.ReadKey();
            }
        }

        private static async Task<string> CallAsync(string url)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(baseAddr + url);
                var contentStr = await response.Content.ReadAsStringAsync();
                return contentStr;
            }
        }
}
}
