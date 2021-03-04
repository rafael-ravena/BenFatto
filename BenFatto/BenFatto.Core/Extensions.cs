using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace BenFatto
{
    public static class Extensions
    {
        public static string FormateTimeZone(this DateTime value, int timeZone, CultureInfo cultureInfo, string format)
        {
            return $"{value.ToString(format, cultureInfo)} {timeZone.ToTimeZoneString()}";
        }

        public static DateTime BeginningOfDay(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, 0, 0, 0);
        }

        public static DateTime BeginningOfHour(this DateTime value)
        {
            return new DateTime(value.Year, value.Month, value.Day, value.Hour, 0, 0);
        }

        public static DateTime NextHour(this DateTime value)
        {
            return value.BeginningOfHour().AddHours(1);
        }
        public static DateTime NextDay(this DateTime value)
        {
            return value.BeginningOfDay().AddDays(1);
        }

        public static string ToTimeZoneString(this int value)
        {
            string strValue = Math.Abs(value).ToString();
            if (0 == value)
                return "±0000";
            return $"{(value < 0 ? "-" : "+")}{strValue.PadLeft(4, '0')}";
        }

        //IMHO this would be a better implementation on 'PadLeft' or 'PadRight' =D
        public static string PadLeft(this string value, int totalLenght) => value.PadLeft(' ', totalLenght);

        public static string PadLeft(this string value, char character, int totalLenght)
        {
            if (value.Length - totalLenght < 0)
                throw new ArgumentOutOfRangeException(nameof(totalLenght));
            return character.Repeat(totalLenght - value.Length) + value;
        }

        public static string PadRight(this string value, int totalLenght) => value.PadRight(' ', totalLenght);

        public static string PadRight(this string value, char character, int totalLenght)
        {
            if (value.Length - totalLenght < 0)
                throw new ArgumentOutOfRangeException(nameof(totalLenght));
            return value + character.Repeat(totalLenght - value.Length);
        }

        public static string Repeat(this char value, int lenght) => value.ToString().Repeat(lenght);

        public static string Repeat(this string value, int lenght)
        {
            if (lenght < 0)
                throw new ArgumentOutOfRangeException(nameof(lenght));
            while (value.Length < lenght)
            {
                value += value;
            }
            return value.Substring(0, lenght);
        }

        public static string GetDescription(this Enum value)
        {
            FieldInfo field = value.GetType().GetField(value.ToString());
            DescriptionAttribute DescAttribute =
                field.GetCustomAttribute<DescriptionAttribute>();
            if (DescAttribute != null)
                return DescAttribute.Description;
            DisplayAttribute DisplayAttribute =
                field.GetCustomAttribute<DisplayAttribute>();
            if (DisplayAttribute != null)
                return DisplayAttribute.Name;
            return value.ToString();
        }

    }
}
