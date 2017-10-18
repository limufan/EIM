using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM
{
    public class EnumHelper
    {
        public static List<T> GetValues<T>()
        {
            Type enumType = typeof(T);
            if (!enumType.IsEnum)
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");

            List<T> values = new List<T>();

            var fields = enumType.GetFields().Where(f => f.IsLiteral);

            foreach (FieldInfo field in fields)
            {
                T value = (T)field.GetValue(enumType);
                values.Add(value);
            }

            return values;
        }

        public static List<object> GetValues(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");

            List<object> values = new List<object>();

            var fields = enumType.GetFields().Where(f => f.IsLiteral);

            foreach (FieldInfo field in fields)
            {
                object value = field.GetValue(enumType);
                values.Add(value);
            }

            return values;
        }

        public static IList<string> GetNames(Type enumType)
        {
            if (!enumType.IsEnum)
                throw new ArgumentException("Type '" + enumType.Name + "' is not an enum.");

            List<string> values = new List<string>();

            var fields = enumType.GetFields().Where(f => f.IsLiteral);

            foreach (FieldInfo field in fields)
            {
                values.Add(field.Name);
            }

            return values;
        }
    }
}
