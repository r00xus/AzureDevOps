using MDS.Azure.DevOps.Web2.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDS.Azure.DevOps.Web2.ActionResults
{
    public class ExceptionResult : JsonNetResult
    {
        public ExceptionResult(Exception e)
        {
            Data = new
            {
                success = false,
                exception = e.GetMessageChain()
            };
        }
    }
}