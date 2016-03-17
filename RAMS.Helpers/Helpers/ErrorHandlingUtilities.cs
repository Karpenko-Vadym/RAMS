using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace RAMS.Helpers
{
    /// <summary>
    /// ErrorHandlingUtilities class implements error hendling helpers
    /// </summary>
    public static class ErrorHandlingUtilities
    {
        /// <summary>
        /// LogException method logs an exception details to the file
        /// </summary>
        /// <param name="exception">Exception details in string format (It is recommended to use GetExceptionDetails() method to convert exception into the string)</param>
        /// <param name="controller">Controller in which an exception have been thrown</param>
        /// <param name="action">Action method in which an exception have been thrown</param>
        /// <param name="other">Additional information that can be appended to the log file</param>
        public static void LogException(string exception, string controller = null, string action = null, string other = null)
        {
            var filePath = HttpContext.Current.Server.MapPath("~/App_Data/ExceptionLog.log");

            var stringBuilder = new StringBuilder();

            if (String.IsNullOrEmpty(controller) || String.IsNullOrEmpty(action))
            {
                stringBuilder.Append("Logged on: " + DateTime.UtcNow + Environment.NewLine);
            }
            else
            {
                stringBuilder.Append("Logged on: " + DateTime.UtcNow + Environment.NewLine + "Controller: " + controller + Environment.NewLine + "Action: " + action + Environment.NewLine);
            }

            stringBuilder.Append(exception + Environment.NewLine);

            if (!String.IsNullOrEmpty(other))
            {
                stringBuilder.Append(other + Environment.NewLine);
            }

            stringBuilder.Append(Environment.NewLine);

            var streamWriter = new StreamWriter(filePath, true);

            streamWriter.Write(stringBuilder);

            streamWriter.Close();
        }

        /// <summary>
        /// GetExceptionDetails method converts an exception into the string appending stack trace and inner exception details 
        /// </summary>
        /// <param name="ex">An exception to be converted into the string</param>
        /// <returns>String representation of the exception</returns>
        public static string GetExceptionDetails(this Exception ex)
        {
            if(ex == null)
            {
                return String.Empty;
            }

            var result = new StringBuilder();

            result.Append(String.Format("Exception of type '{0}' has been caught." + Environment.NewLine + "Message: {1}" + Environment.NewLine, ex.GetType().Name, ex.Message));

            if (!String.IsNullOrEmpty(ex.StackTrace))
            {
                result.Append("********************* Stack Trace **********************" + Environment.NewLine);
                result.Append(ex.StackTrace + Environment.NewLine);
                result.Append("****************** End of Stack Trace ******************" + Environment.NewLine);
            }

            if(ex.InnerException != null)
            {
                result.Append("******************* Inner Exception ********************" + Environment.NewLine);
                result.Append(ex.InnerException.GetExceptionDetails());
                result.Append("**************** End of Inner Exception ****************" + Environment.NewLine);
            }

            return result.ToString();
        }
    }
}
