using System.IO;
using lab4.Implementation;
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
        
        [HttpGet("/get")]
        public string Get()
        {
            return governmentHandler.FindDocument("abc");
        }
    }
}