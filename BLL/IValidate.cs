using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    interface IValidate<T> where T:class,new()
    {
        T Validate(JObject obj);
    }
}
