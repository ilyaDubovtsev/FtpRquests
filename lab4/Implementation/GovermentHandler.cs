using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using lab4.Models;
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

        public List<Procurement> FindDocuments(string substring)
        {
            var zipNames = GetZipNames();

            var procurements = new List<Procurement>();

            foreach (var zipName in zipNames)
            {
                var documents = GetAllDocumentsFromZip(zipName);

                foreach (var pair in documents)
                {
                    var name = pair.Key;
                    if (pair.Value.Contains(substring))
                    {
                        var procurement = new Procurement()
                        {
                            Name = name,
                            ArchiveName = zipName,
                            Url = $"{uri}{zipName}",
                            Value = pair.Value
                        };

                        procurements.Add(procurement);
                    }
                }
            }

            return procurements;
        }

        private List<string> GetZipNames()
        {
            return ftpProvider.GetFilesInDirectory(uri, login, password);
        }

        private Dictionary<string, string> GetAllDocumentsFromZip(string zipName)
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

            return decompressedFiles
                .Where(x => x.Key.EndsWith(".xml"))
                .ToDictionary(
                    k => k.Key,
                    v => Encoding.Default.GetString(v.Value)
                );
//            var xmlDocuments = new Dictionary<string, XmlDocument>(); //new List<XmlDocument>();
//
//            foreach (var pair in decompressedFiles)
//            {
//                if (pair.Key.EndsWith(".xml"))
//                {
//                    var xmlDocument = xmlProvider.GetDocumentFromBytes(pair.Value);
//                    xmlDocuments[pair.Key] = xmlDocument;
//                }
//            }
//
//            return xmlDocuments;
        }
    }
}