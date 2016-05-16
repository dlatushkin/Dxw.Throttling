using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Dxw.Throttling.Core.Processors;
using Dxw.Throttling.Core.Rules;
using Dxw.Throttling.Core.Storages;
using Dxw.Throttling.Redis.Storages;

namespace Dxw.Throttling.ConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //var config = System.Configuration.ConfigurationManager.GetSection("Throttling");

            //var config1 = System.Configuration.ConfigurationManager.GetSection("throttling");

            var configSection = System.Configuration.ConfigurationManager.GetSection("throttling");

            //var storage = new RedisStorage();

            //storage.Upsert("1", null, null, 
            //    (context, storage1, oldValue, rule) => new ProcessEventResult
            //                                                {
            //                                                    NewState = new SimpleStorageValue { Value = "2" },
            //                                                    Result = ApplyResult.Ok()
            //                                                });

            Console.WriteLine("done"); Console.ReadKey();
        }

        [Serializable]
        class SimpleStorageValue : IStorageValue
        {
            public object Value { get; set; }

            public bool IsExpired(DateTime utcNow)
            {
                return false;
            }
        }
    }
}
