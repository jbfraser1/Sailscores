﻿using System;
using System.Linq;

namespace SailScores.Core.Services
{
    public static class UrlUtility
    {

        public static string GetUrlName(string rawName)
        {
            if (String.IsNullOrWhiteSpace(rawName))
            {
                return null;
            }

            return RemoveWhitespace(RemoveDisallowedCharacters(rawName));
        }

        public static string RemoveWhitespace(string input)
        {
            return new string(input.ToCharArray()
                .Where(c => !Char.IsWhiteSpace(c))
                .ToArray());
        }

        public static string RemoveDisallowedCharacters(string str)
        {
            var charsToRemove = new string[] { ":", "/", "?", "#", "[", "]", "@", "!", "$", "&", "'", "(", ")", "*", "+", ",", ";", "=" };
            foreach (var c in charsToRemove)
            {
                str = str.Replace(c, string.Empty);
            }
            return str;
        }
    }
}
