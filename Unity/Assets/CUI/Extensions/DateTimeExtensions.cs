using System;

namespace CUI.Extensions
{
    public static class DateTimeExtensions
    {
        public static long ToUnixTimeSeconds(this DateTime dateTime)
        {
            return (long)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
        }

        public static string ToDateString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        public static string ToLocalDateString(this DateTime date)
        {
            return date.ToLocalTime().ToString("yyyy-MM-dd");
        }

        public static string ToLocalDateTimeString(this DateTime date)
        {
            return date.ToLocalTime().ToString("yyyy-MM-dd HH:mm");
        }
    }
}