using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MDS.Azure.DevOps.Web2.Extensions
{
    public static class ExceptionExtensions
    {
        public static string GetMessageChain(this Exception inputEx, string message = "", int innerExLevel = 0, bool innerExNewLine = true)
        {
            message += (string.IsNullOrEmpty(message) ? string.Empty : "; " + (innerExNewLine ? Environment.NewLine : string.Empty)) +
                (innerExLevel > 0 ? string.Format("INNER EXCEPTION {0}: ", innerExLevel) : string.Empty) +
                inputEx.Message;

            if (inputEx.InnerException != null)
            {
                innerExLevel++;
                message = inputEx.InnerException.GetMessageChain(message, innerExLevel, innerExNewLine);
            }

            return message;
        }
    }
}