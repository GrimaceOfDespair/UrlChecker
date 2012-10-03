using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace UrlChecker
{
  public static class WebRequestExtensions
  {
    public static WebResponse BetterEndGetResponse(this WebRequest request, IAsyncResult asyncResult)
    {
      try
      {
        return request.EndGetResponse(asyncResult);
      }
      catch (WebException wex)
      {
        if (wex.Response != null)
        {
          return wex.Response;
        }
        throw;
      }
    }

    public static WebResponse GetResponse(this WebRequest request, bool doThrow)
    {
      try
      {
        return request.GetResponse();
      }
      catch (WebException wex)
      {
        if (doThrow == false && wex.Response != null)
        {
          return wex.Response;
        }
        throw;
      }
    }
  }
}
