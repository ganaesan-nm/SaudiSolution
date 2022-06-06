using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaudiA.WebPortal.Foundation.ServiceClient.Models
{
    public class ODataApiResponse<T> where T : new()
    {
        public T ResponseData { get; set; }       

        public ODataApiResponse()
        { 
        }
    }
}
