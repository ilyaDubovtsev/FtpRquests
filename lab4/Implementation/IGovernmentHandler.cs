using System.Collections.Generic;
using System.Xml;
using lab4.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab4.Implementation
{
    public interface IGovernmentHandler
    {
        List<Procurement> FindDocuments(string substring); 
    }
}