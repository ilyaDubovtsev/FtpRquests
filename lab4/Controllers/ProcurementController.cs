using System.Collections.Generic;
using System.IO;
using lab4.Implementation;
using lab4.Models;
using Microsoft.AspNetCore.Mvc;

namespace lab4.Controllers
{
    [Route("procurement")]    
    public class ProcurementController : Controller
    {
        private readonly IGovernmentHandler governmentHandler;

        public ProcurementController(IGovernmentHandler governmentHandler)
        {
            this.governmentHandler = governmentHandler;
        }
        
        [HttpPost("/get")]
        public IActionResult Get()
        {
            var form = Request.Form;
            var procurements = governmentHandler.FindDocuments(form["ContainedText"]);
            return View(procurements);
        }
    }
}