// Testable re-implementation of ExternMethods for unit testing.
// Mirrors the production ExternMethods class without WinForms Form dependency.

using System;
using System.Globalization;

namespace SchoolManagementApplciation
{
    /// <summary>
    /// Testable copy of ExternMethods extension class.
    /// </summary>
    public static class ExternMethods
    {
        public static Type? IsNumber(this object value)
        {
            if (value is sbyte)   return typeof(sbyte);
            if (value is byte)    return typeof(byte);
            if (value is short)   return typeof(short);
            if (value is ushort)  return typeof(ushort);
            if (value is int)     return typeof(int);
            if (value is uint)    return typeof(uint);
            if (value is long)    return typeof(long);
            if (value is ulong)   return typeof(ulong);
            if (value is float)   return typeof(float);
            if (value is double)  return typeof(double);
            if (value is decimal) return typeof(decimal);
            return null;
        }

        public static string ToMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(dateTime.Month);
        }

        public static string ToShortMonthName(this DateTime dateTime)
        {
            return CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(dateTime.Month);
        }
    }
}
