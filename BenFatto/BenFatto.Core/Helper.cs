using System;
using System.Collections.Generic;
using System.Text;

namespace BenFatto
{
    public static class Helper
    {
        public static string GetChunk(ref string value, char searchValue)
        {
            int searchPos = value.IndexOf(searchValue);
            if (searchPos < 0)
                return string.Empty;
            string returnValue = value.Substring(0, searchPos);
            value = value.Substring(searchPos + 1);
            return returnValue;
        }

    }
}
