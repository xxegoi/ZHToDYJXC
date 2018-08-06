using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebApi.Models;

namespace WebApi.Common
{
    public class FGH_CloseCheckAttribute: ActionFilterAttribute
    {
        EMTDBContainer db = new EMTDBContainer();

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            ///访问前先检查运转卡是否关闭，如果关闭直接返回错误
            var datastr = actionContext.ActionArguments.First().Value.ToString();

            var data = JObject.Parse(JObject.Parse(datastr).GetValue("data").ToString());

            var fgh= data.GetValue("fgh")==null?string.Empty:data.GetValue("fgh").ToString();

            if (fgh != string.Empty)
            {
                if (db.t_DY_YZK.Count(p => p.FGH == fgh && p.FClosed) > 0)
                {
                    var ex = new Exception("操作失败，此缸号已关闭!");

                    var result = new FailResult(datastr, ex.Message);

                    actionContext.Response= actionContext.Request.CreateResponse(System.Net.HttpStatusCode.OK, result);
                }
            }

        }
    }
}