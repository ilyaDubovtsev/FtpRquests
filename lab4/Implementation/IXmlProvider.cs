using System.IO;
using System.Xml;

namespace lab4.Implementation
{
    public interface IXmlProvider
    {
        XmlDocument GetDocumentFromStream(byte[] bytes);
    }
}