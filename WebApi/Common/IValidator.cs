using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.Common
{
    interface IValidator<T> where T:class,new()
    {
        bool Validate(JObject obj, out T entity);
    }
}
