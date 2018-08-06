using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public abstract class Result
    {
        public string ResTime { get; set; }
        public string ExceptionMessage { get; set; }
        public string State { get; set; }
        public string Data { get; set; }
    }

    public class SucessResult:Result
    {
        public SucessResult(string data)
        {
            this.State = "sucess";
            this.Data = data;
            this.ResTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }

    public class FailResult : Result
    {
        public FailResult(string data,string exception)
        {
            this.Data = data;
            this.ExceptionMessage = exception;
            this.State = "Fail";
            this.ResTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");
        }
    }
}
