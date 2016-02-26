using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace RAMS.Helpers
{
    /// <summary>
    /// Utilities class implements various helpers that can be used throught application
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// IsEmpty method checks whether IEnumerable contains any elements and returns true if so, false otherwise
        /// </summary>
        /// <typeparam name="T">IEnumerable of T type is passed to IsEmpty method to determine whether it contains any elements</typeparam>
        /// <param name="data">Data of type T (Actual variable)</param>
        /// <returns>True if IEnumerable contains any elements, false otherwise</returns>
        public static bool IsEmpty<T>(this IEnumerable<T> data)
        {
            if (data != null)
            {
                return (data.Any()) ? false : true;
            }

            return true;
        }

        /// <summary>
        /// RegexMatch method checks whether the string (data) matches a regular expression (regexString) and returns true if so, false otherwise
        /// </summary>
        /// <param name="regexString">String representation of a regular expression</param>
        /// <param name="data">String to be matched with regular expression</param>
        /// <returns>True if string matches a regular expression, false otherwise</returns>
        public static bool RegexMatch(string regexString, string data)
        {
            Regex regex = new Regex(@regexString);

            Match match = regex.Match(data);

            if(match.Success)
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// GetProfilePictureUrl returns string representation of the path to user's profile picture, and if profile picture does not exist, returns empty string
        /// </summary>
        /// <param name="userName">User name of the user whos profile picture's url is being fetched</param>
        /// <returns>String representation of the path to user's profile picture, and if profile picture does not exist, returns empty string</returns>
        public static string GetProfilePictureUrl(this HtmlHelper htmlHelper, string userName)
        {
            var urlHelper = new UrlHelper(htmlHelper.ViewContext.RequestContext);

            if(File.Exists(htmlHelper.ViewContext.HttpContext.Server.MapPath(String.Format("~/Content/ProfilePictures/{0}.jpg", userName))))
            {
                return urlHelper.Content(String.Format("~/Content/ProfilePictures/{0}.jpg", userName));
            }
            else if (File.Exists(htmlHelper.ViewContext.HttpContext.Server.MapPath(String.Format("~/Content/ProfilePictures/{0}.png", userName))))
            {
                return urlHelper.Content(String.Format("~/Content/ProfilePictures/{0}.png", userName));
            }
            else if (File.Exists(htmlHelper.ViewContext.HttpContext.Server.MapPath(String.Format("~/Content/ProfilePictures/{0}.gif", userName))))
            {
                return urlHelper.Content(String.Format("~/Content/ProfilePictures/{0}.gif", userName));
            }

            return String.Empty;
        }

        /// <summary>
        /// ConvertIntToBase64String moethod converts int to encoded string
        /// </summary>
        /// <param name="data">Integer to be converted</param>
        /// <returns>Encoded string</returns>
        public static string ConvertIntToBase64String(int data)
        {
            return Convert.ToBase64String(BitConverter.GetBytes(data));
        }

        /// <summary>
        /// ConvertBase64StringToInt converts encoded string to int
        /// </summary>
        /// <param name="data">String to be converted</param>
        /// <returns>Decoded int</returns>
        public static int ConvertBase64StringToInt(string data)
        {
            return BitConverter.ToInt32(Convert.FromBase64String(data), 0);
        }
    }
}
