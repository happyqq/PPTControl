using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace CommonHelper
{
   public static class HttpHelper
    {
        public static string GetHTTPInfo(string urlPath, string eCode, int millisecond)
        {
            string str = null;
            HttpWebResponse response = null;
            StreamReader reader = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlPath);
                request.Timeout = millisecond;
                response = (HttpWebResponse)request.GetResponse();
                reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.GetEncoding(eCode));
                str = reader.ReadToEnd();
            }
            catch
            {
                str = "";
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                    reader.Dispose();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return str;
        }
    }
}
