using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Xml.Linq
{
    public static class XElementExtensions
    {
        public static bool HasElement(this XElement element, string name)
        {
            XElement found = element.Element(name);
            if (found != null)
                return true;
            return false; 
        }
    }
}
