using System;
using System.Collections.Generic;
using System.IO;
using System.Net;

namespace lab4.Implementation
{
    public class FtpProvider : IFtpProvider
    {
        public List<string> GetFilesInDirectory(string requestUriString, string login, string password)
        {
            var response = MakeRequest(requestUriString, login, login, WebRequestMethods.Ftp.ListDirectory);

            var files = new List<string>();

            using (var responseStream = response.GetResponseStream())
            using (var reader = new StreamReader(responseStream))
            {
                while (!reader.EndOfStream)
                {
                    files.Add(reader.ReadLine());
                } 
            }

            return files;
        }

        public FtpWebResponse GetResponse(string requestUriString, string login, string password)
        {
            return MakeRequest(requestUriString, login, login, WebRequestMethods.Ftp.DownloadFile);
        }

        private FtpWebResponse MakeRequest(string requestUriString, string login, string password, string webRequestMethod)
        {
            var request = (FtpWebRequest) WebRequest.Create(requestUriString);
            request.Credentials = new NetworkCredential(login, password);
            request.Method = webRequestMethod;
            request.UseBinary = true;

            return (FtpWebResponse)request.GetResponse();
        }
    }
}