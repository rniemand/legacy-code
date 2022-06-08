using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Rn.WebManLib
{
    [ServiceContract]
    public class WmAuthApi
    {
        public string UserName { get; set; }
        public string ApiKey { get; set; }
    }
}
