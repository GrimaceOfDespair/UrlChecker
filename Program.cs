using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace UrlChecker
{
  class Program
  {
    static void Main(string[] args)
    {
      string line;
      while (string.IsNullOrEmpty(line = Console.ReadLine()) == false)
      {
        try
        {
          //Console.WriteLine("Checking " + line);
          var command = Regex.Replace(line, @"\t(.*)$", "");

          // Get number of times to request url for profiling
          var profilingMatch = Regex.Match(command, @"((?<warmup>\d+)\s+)?(?<times>\d+)\s+(?<url>.+)*");
          int times;
          if (profilingMatch.Success && int.TryParse(profilingMatch.Groups["times"].Value, out times))
          {
            // Get number of times to request url for warmup
            int warmup;
            int.TryParse(profilingMatch.Groups["warmup"].Value, out warmup);

            // Get url to request
            var url = profilingMatch.Groups["url"].Value;
            var elapsedMilliseconds = Profile(warmup, times, () => CheckUrl(url));

            Console.WriteLine(url + "\tAVG\t" + url + "\t" + Math.Floor(elapsedMilliseconds / (float)times));
          }
          else
          {
            CheckUrl(command);
          }
        }
        catch (Exception e)
        {
          Console.Error.WriteLine(e);
        }
      }
    }

    private static long Profile(int warmup, int times, Func<long> action)
    {
      long totalTime = 0;
      for (var time = -warmup; time < times; time++)
      {
        var elapsedMilleSeconds = action();

        if (time >= 0)
        {
          totalTime += elapsedMilleSeconds;
        }
      }
      return totalTime;
    }

    private static long CheckUrl(string url)
    {
      var request = (HttpWebRequest)WebRequest.Create(url);
      request.AllowAutoRedirect = true;
      request.MaximumAutomaticRedirections = 4;

      var timer = new Stopwatch();
      timer.Start();

      HttpStatusCode statusCode;
      Uri responseUri;
      using (var response = (HttpWebResponse)request.GetResponse(false))
      {
        statusCode = response.StatusCode;
        responseUri = response.ResponseUri;
      }

      timer.Stop();

      var responseInfo = url + "\t" + statusCode + "\t" + responseUri + "\t" + timer.ElapsedMilliseconds;

      if (statusCode == HttpStatusCode.OK)
      {
        Console.WriteLine(responseInfo);
      }
      else
      {
        Console.Error.WriteLine(responseInfo);
      }

      return timer.ElapsedMilliseconds;
    }
  }
}
