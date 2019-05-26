using System.Collections.Generic;
using System.IO;
using System.Net;

namespace lab4.Implementation
{
    public interface IFtpProvider
    {
        List<string> GetFilesInDirectory(string requestUriString, string login, string password);
        FtpWebResponse GetResponse(string requestUriString, string login, string password);
    }
}