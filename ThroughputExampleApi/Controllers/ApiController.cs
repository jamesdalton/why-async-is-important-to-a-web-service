/// <summary>
/// Copyright (c) 2018 James Dalton. All rights reserved.
/// This work is licensed under the terms of the MIT license.
/// </summary>
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace ThroughputExampleApi.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        static RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
        const int minRange = 500;
        const int maxRange = 2000;

        private int SleepTime()
        {
            var bytes = new byte[4];
            random.GetBytes(bytes);
            var scale = BitConverter.ToUInt32(bytes, 0);
            return (int)(minRange + (maxRange - minRange) * (scale / ((double)uint.MaxValue)));
        }

        /// <summary>
        /// Returns after a random period of time using Thread.Sleep
        /// </summary>
        /// <returns>string</returns>
        [HttpGet]
        [Route("sync")]
        public string GetSync()
        {
            Thread.Sleep(SleepTime());
            return "OK";
        }

        /// <summary>
        /// Returns after a random period of time using Task.Delay
        /// </summary>
        /// <returns>string</returns>
        [HttpGet]
        [Route("async")]
        public async Task<ActionResult<string>> GetAsync(int id)
        {
            await Task.Delay(SleepTime());
            return "OK";
        }
    }
}
