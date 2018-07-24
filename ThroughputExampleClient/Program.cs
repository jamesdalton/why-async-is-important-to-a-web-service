/// <summary>
/// Copyright (c) 2018 James Dalton. All rights reserved.
/// This work is licensed under the terms of the MIT license.
/// </summary>

using System;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ThroughputExampleClient
{
    class Program
    {
        static HttpClient client;

        static async Task<bool> MakeRequestAsync(string url)
        {
            try
            {
                using (var response = await client.GetAsync(url))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Console.WriteLine($"{response.StatusCode}: {response.ReasonPhrase}");
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }

        static async Task RunTestAsync(string url, int count)
        {
            List<Task<bool>> gets = new List<Task<bool>>();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (var i = 0; i < count; i++)
            {
                gets.Add(MakeRequestAsync(url));
            }
            var results = await Task.WhenAll(gets);
            var total = results.Count();
            var success = results.Where(x => x).Count();
            var error = results.Where(x => !x).Count();
            Console.WriteLine("Time\tTotal\tSuccess\tError");
            Console.WriteLine($"{stopWatch.ElapsedMilliseconds/1000.0} s\t{total}\t{success}\t{error}");
        }

        static async Task Main(string[] args)
        {
            var count = Int32.Parse(args[0]);
            using (client = new HttpClient())
            {
                Console.WriteLine("Sync");
                await RunTestAsync("http://localhost:5000/api/sync", count);
                Console.WriteLine("Async");
                await RunTestAsync("http://localhost:5000/api/async", count);
            }
        }
    }
}
