using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace BenFatto
{
    public static class Extensions
    {
        public static string FormateTimeZone(this DateTime value, int timeZone) => value.FormateTimeZone(timeZone, "dd/MMM/yyyy:HH:ss:mm");

        public static string FormateTimeZone(this DateTime value, int timeZone, string format)
        {
            return $"{value.ToString(format, CultureInfo.GetCultureInfo("en-US"))} {timeZone.ToTimeZoneString()}";
        }

        public static string ToTimeZoneString(this int value)
        {
            string strValue = Math.Abs(value).ToString();
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
