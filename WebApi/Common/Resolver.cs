using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApi.Common
{
    public class Resolver
    {

        public string Token { get; set; }
        public string DataStr { get; set; }
        public JObject Data { get; set; }

        public Resolver(JObject obj)
        {
            var token = obj.GetValue("token");
            var data = obj.GetValue("data");

            if (token == null)
            {
                throw new UnauthorizedAccessException("token验证失败");
            }
            else
            {
                var tokenstr = ConfigurationManager.AppSettings["token"];

                var json =token.ToString()!=tokenstr ? string.Empty : tokenstr;

                if (string.Empty== json)
                {
                    throw new UnauthorizedAccessException("token验证失败");
                }
            }

            if (data == null)
            {
                throw new Exception("data不能为空");
            }

            this.Token = token.ToString();
            this.DataStr = data.ToString();
            this.Data = JObject.Parse(this.DataStr);
        }
    }
}