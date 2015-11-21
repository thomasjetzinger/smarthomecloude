using Microsoft.Azure.Devices;
using SmartHomeCloudAPI.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace SmartHomeCloudAPI.Controllers
{
    public class TestController : ApiController
    {

         ServiceClient serviceClient;
         
        [HttpGet]
        public  IEnumerable<Contact> Get()
        {
            
            return new Contact[]{
                new Contact { Id = 1, EmailAddress = "barney@contoso.com", Name = "Barney Poland"},
                new Contact { Id = 2, EmailAddress = "lacy@contoso.com", Name = "Lacy Barrera"},
                new Contact { Id = 3, EmailAddress = "lora@microsoft.com", Name = "Lora Riggs"}
            };
        }





    }
}
