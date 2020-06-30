using System;
using System.Collections.Generic;
using System.Text;

namespace DatEx.ApsTender.Helpers
{
    public static class Ext_String
    {
        public const Int32 IndentWidth = 3;

        private static Dictionary<Int32, String> Indents = new Dictionary<Int32, String>();

        public static String GetIndent(Int32 indentLevel)
        {
            String indent;
            if(!Indents.TryGetValue(indentLevel, out indent))
                indent = new String(' ', indentLevel * Ext_String.IndentWidth);
            return indent;
        }
    }
}
