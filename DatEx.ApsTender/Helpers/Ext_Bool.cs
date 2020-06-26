namespace DatEx.ApsTender.Helpers
{
    using System;



    public static class Ext_Bool
    {
        public static String AsString(this Boolean? value, String whenTrue, String whenFalse, String whenNull)
        {
            if(value is null) return whenNull;
            return value == false ? whenFalse : whenTrue;
        }

        public static String AsString(this Boolean value, String whenTrue, String whenFalse)
        {
            return value == false ? whenFalse : whenTrue;
        }
    }
}
