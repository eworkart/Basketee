using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Basketee.API.Services
{
    /// <summary>
    /// Extension class for string manipulation methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Case insignificant comparison between the given string and current instance
        /// </summary>
        /// <param name="instance">Current instance</param>
        /// <param name="valueToCompare">Value to be compared with the current instance</param>
        /// <returns>Whether the string matches</returns>
        public static bool CompareIgnoreCase(this string instance, string valueToCompare)
        {
             return string.Equals(instance, valueToCompare, StringComparison.OrdinalIgnoreCase);
        }

        public static string ConvertToPascalCase(this string str)
        {
            //if nothing is proivided throw a null argument exception
            if (str == null) throw new ArgumentNullException("str", "Null text cannot be converted!");

            if (str.Length == 0) return str;

            //split the provided string into an array of words
            str = str.ToLower();
            string[] words = str.Split(' ');

            //loop through each word in the array
            for (int i = 0; i < words.Length; i++)
            {
                //if the current word is greater than 1 character long
                if (words[i].Length > 0)
                {
                    //grab the current word
                    string word = words[i];

                    //convert the first letter in the word to uppercase
                    char firstLetter = char.ToUpper(word[0]);

                    //concantenate the uppercase letter to the rest of the word
                    words[i] = firstLetter + word.Substring(1);
                }
            }

            //return the converted text
            return string.Join(" ", words);
        }

        public static string Join(this List<string> str, string delimiter = "|")
        {
            return str != null ? String.Join(delimiter, str.ToArray()) : string.Empty;
        }


        public static string ToCleanString(this object str)
        {
            return str != null ? Regex.Replace(Convert.ToString(str), @"\t\n\r", "").Trim() : "";
        }

        public static string ToCleanString(this string str)
        {
            return str != null ? Regex.Replace(str, @"\t\n\r", "").Trim() : "";
        }
    }
}
