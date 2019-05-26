using System.IO;
using System.Text;
using System.Xml;

namespace lab4.Implementation
{
    public class XmlProvider : IXmlProvider
    {
        public XmlDocument GetDocumentFromBytes(byte[] bytes)
        {
            var xmlDocument = new XmlDocument();
            
            using (var memoryStream = new MemoryStream(bytes))
            {
                xmlDocument.Load(memoryStream);
                return xmlDocument;
            }
        }
    }
}