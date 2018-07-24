/// <summary>
/// Copyright (c) 2018 James Dalton. All rights reserved.
/// This work is licensed under the terms of the MIT license.
/// </summary>

using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace basic_example
{
    class Program
    {
        /// <summary>
        /// Asynchronously gets the Google home page.
        /// </summary>
        /// <param name="client">HttpClient</param>
        /// <returns>A task that will return a string.</returns>
        /// <remarks>
        /// This could easily be done in three lines, but breaking it up like
        /// allows for console output to highlight execution flow.
        /// </remarks>
        static async Task<string> MakeRequest(HttpClient client)
        {
            Console.WriteLine("Making get request.");
            var responseTask = client.GetAsync("http://google.com");
            Console.WriteLine("Request made waiting for result.");
            var response = await responseTask;
            Console.WriteLine("Have response, getting Content.");
            var resultTask = response.Content.ReadAsStringAsync();
            Console.WriteLine("Waiting for body.");
            var result = await resultTask;
            Console.WriteLine("Returning result.");
            return result;
        } 
        /// <summary>
        /// An async main is new in C# 7.1. See the csproj file for how to enable.
        /// </summary>
        /// <param name="args">Ignored</param>
        /// <returns>Task</returns>
        static async Task Main(string[] args)
        {
            Console.WriteLine("Enter main.");
            using (var client = new HttpClient())
            {
                Console.WriteLine("Calling MakeRequest.");
                var makeRequestTask = Program.MakeRequest(client);
                Console.WriteLine("Getting result.");
                var result = await makeRequestTask;
                Console.WriteLine("Have result");
                Console.WriteLine(result.Length);
            }
            Console.WriteLine("Exiting main.");
        }
    }
}
