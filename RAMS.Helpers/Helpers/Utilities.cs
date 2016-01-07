﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

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
    }
}