using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Filters;

namespace WebApi.Common
{
    public class LogAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            var resultStr = actionExecutedContext.Response.Content.ReadAsStringAsync().Result;

            var result = JObject.Parse(resultStr);

            var exception = result.GetValue("ExceptionMessage").ToString();
            var state = result.GetValue("State").ToString();
            var data = result.GetValue("Data");

            string message;

            if (state.ToLower()=="fail")
            {
                message = exception;

                LogHandler.Error("{0}请求处理失败: "  + message);
            }
            else
            {
                message = string.Format("{0}请求执行成功", data);
                LogHandler.Info(message);
            }


        }
    }
}