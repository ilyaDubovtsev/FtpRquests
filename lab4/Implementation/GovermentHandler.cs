using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace lab4.Implementation
{
    public class GovernmentHandler : IGovernmentHandler
    {
        private readonly IFtpProvider ftpProvider;
        private readonly IXmlProvider xmlProvider;
        private static string uri = @"ftp://ftp.zakupki.gov.ru/fcs_regions/Udmurtskaja_Resp/protocols/currMonth/";
        private static string login = "free";
        private static string password = "free";

        public GovernmentHandler(
            IFtpProvider ftpProvider,
            IXmlProvider xmlProvider)
        {
            this.ftpProvider = ftpProvider;
            this.xmlProvider = xmlProvider;
        }

        public string FindDocument(string substring)
        {
            var zipNames = GetZipNames();
            var stringBuilder = new StringBuilder();

            foreach (var zipName in zipNames)
            {
                stringBuilder.Append($"Архив {zipName}\n");
                var documents = GetAllDocumentsFromZip(zipName);

                foreach (var pair in documents)
                {
                    stringBuilder.Append($"\t- {pair.Key}\n");
                }
            }

            return stringBuilder.ToString();
        }

        private List<string> GetZipNames()
        {
            return ftpProvider.GetFilesInDirectory(uri, login, password);
        }

        private Dictionary<string, XmlDocument> GetAllDocumentsFromZip(string zipName)
        {
            var response = ftpProvider.GetResponse($"{uri}{zipName}", login, password);

            var allBytes = new List<byte>();

            using (var responseStream = response.GetResponseStream())
            using (var reader = new BinaryReader(responseStream))
            {
                var bufferSize = 2048;

                while (responseStream.CanRead)
                {
                    byte[] buffer = reader.ReadBytes(bufferSize);
                    allBytes.AddRange(buffer);
                }
            }

            var decompressedFiles = ZipHelper.Unzip(allBytes.ToArray());

            var xmlDocuments = new Dictionary<string, XmlDocument>(); //new List<XmlDocument>();

            foreach (var pair in decompressedFiles)
            {
                if (pair.Key.EndsWith(".xml"))
                {
                    var xmlDocument = xmlProvider.GetDocumentFromStream(pair.Value);
                    xmlDocuments[pair.Key] = xmlDocument;
                }
            }

            return xmlDocuments;
        }
    }
}