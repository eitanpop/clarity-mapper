using System;
using System.Collections.Generic;
using System.Text;
using HL7.Dotnetcore;

namespace ClarityMapper.V2.Extensions
{
    public static class MessageExtensions
    {
        public static string GetValueOrNull(this Message message, string segmentLocation, bool formatWithEncodingCharacters = true)
        {
            try
            {
                string value = message.GetValue(segmentLocation);
                if (formatWithEncodingCharacters)
                    value = GetFormattedString(value, message.GetValue("MSH.2"));
                return value;
            }
            catch
            {
                return null;
            }
        }

        private static string GetFormattedString(string value, string encodingChars)
        {
            return value.Replace(encodingChars[0],' ');
        }
    }
}
