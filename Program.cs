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

          var profilingMatch = Regex.Match(command, @"(?<times>\d+)\s(?<url>.+)*");
          int times;
          if (profilingMatch.Success && int.TryParse(profilingMatch.Groups["times"].Value, out times))
          {
            var url = profilingMatch.Groups["url"].Value;
            var elapsedMilliseconds = Profile(times, () => CheckUrl(url));

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

    private static long Profile(int times, Func<long> action)
    {
      long totalTime = 0;
      for (var time = 0; time < times; time++)
      {
        totalTime += action();
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
