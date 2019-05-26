using System.Xml;
using Microsoft.AspNetCore.Mvc;

namespace lab4.Implementation
{
    public interface IGovernmentHandler
    {
        string FindDocument(string substring); 
    }
}