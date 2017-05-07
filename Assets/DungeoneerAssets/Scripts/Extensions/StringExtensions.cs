using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        public static bool StartsWithVowel(this string source)
        {
            if(source != null && source.Length > 0)
            {
                char firstLetter = source.ToLower()[0]; 
                return firstLetter == 'A' || firstLetter == 'E' || firstLetter == 'I' || firstLetter == 'O' || firstLetter == 'U';
            }
            return false; 
        }
    }
}
