namespace Dxw.Throttling.OwinSelfHostedDemo
{
    using System;
    using Microsoft.Owin.Hosting;
    using System.Net.Http;

    class Program
    {
        static void Main(string[] args)
        {
            var baseAddr = "http://localhost:8001/";

            using (WebApp.Start<Startup>(baseAddr))
            {
                var client = new HttpClient();

                client.GetAsync(baseAddr + "api/values").ContinueWith(r =>
                {
                    var response = r.Result;
                    Console.WriteLine(response);
                    Console.WriteLine(response.Content.ReadAsStringAsync().Result);
                });

                Console.WriteLine("started at " + baseAddr); Console.ReadKey();
            }
        }
    }
}
