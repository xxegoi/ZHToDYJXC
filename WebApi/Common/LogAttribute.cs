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

            var controller = actionExecutedContext.ActionContext.ControllerContext.ControllerDescriptor.ControllerName;
            var action = actionExecutedContext.ActionContext.ActionDescriptor.ActionName;

            string message;

            if (state.ToLower()=="fail")
            {
                message =string.Format("{0}-{1}-请求处理失败: {2}" ,controller,action, exception);
                LogHandler.Error(message);
            }
            else
            {
                message = string.Format("{0}-{1}-请求处理成功: {2}", controller,action,data);
                LogHandler.Info(message);
            }


        }
    }
}