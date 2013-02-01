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
          CheckUrl(Regex.Replace(line, @"\t(.*)$", ""));
        }
        catch (Exception e)
        {
          Console.Error.WriteLine(e);
        }
      }
    }

    private static void CheckUrl(string url)
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
    }
  }
}
