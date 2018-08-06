using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApi.Common
{
    public class JObjectToEntity
    {
        public static T Convert<T>(JObject obj) where T : class, new()
        {
            var t = typeof(T);
            var pis = t.GetProperties();

            var pers = from p in  obj.Properties()
                       select p.Name;

            T result = new T();

            foreach(var p in pis)
            {
                var propertyName = p.Name.ToLower();
                if (!pers.Contains(propertyName))
                {
                    throw new Exception("缺少值：" + propertyName);
                }
                else
                {
                    var pt = p.PropertyType;

                    var value = obj[propertyName].ToString();
                    if (p.PropertyType.Name.ToLower() == "int32")
                    {
                        p.SetValue(result,Int32.Parse(value));
                    }
                    else if (p.PropertyType.Name.ToLower() == "decimal")
                    {
                        p.SetValue(result, decimal.Parse(value));
                    }
                    else
                    {
                        p.SetValue(result, value);
                    }

                   
                }
            }

            return result;
        }
    }
}