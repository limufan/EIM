using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM
{
    public static class Enumerable
    {
        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            PropertyInfo property = typeof(TSource).GetProperty(propertyName);
            if (ReflectionHelper.Is<IOrderable>(property.PropertyType))
            {
                return source.OrderBy(o =>(IOrderable)property.GetValue(o, null));
            }
            else if (ReflectionHelper.IsIList<IOrderable>(property.PropertyType))
            {
                return source.OrderBy(o =>(IList)property.GetValue(o, null));
            }
            return source.OrderBy(o => property.GetValue(o, null));
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, string propertyName)
        {
            if (propertyName == null)
            {
                throw new ArgumentNullException("propertyName");
            }
            PropertyInfo property = typeof(TSource).GetProperty(propertyName);
            if (ReflectionHelper.Is<IOrderable>(property.PropertyType))
            {
                return source.OrderByDescending(o =>(IOrderable)property.GetValue(o, null));
            }
            else if (ReflectionHelper.IsIList<IOrderable>(property.PropertyType))
            {
                return source.OrderByDescending(o =>(IList)property.GetValue(o, null));
            }
            return source.OrderByDescending(o => property.GetValue(o, null));
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Func<TSource, IOrderable> keySelector)
        {
            return source.OrderBy(o => 
                {
                    IOrderable orderable  = keySelector(o);
                    if (orderable == null)
                    {
                        return null;
                    }
                    return orderable.GetOrderValue();
                });
        }

        public static IOrderedEnumerable<TSource> OrderBy<TSource>(this IEnumerable<TSource> source, Func<TSource, IList> keySelector)
        {
            return source.OrderBy(o => GetOrderValue(keySelector(o)));
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, Func<TSource, IOrderable> keySelector)
        {
            return source.OrderByDescending(o =>
                {
                    IOrderable orderable = keySelector(o);
                    if (orderable == null)
                    {
                        return null;
                    }
                    return orderable.GetOrderValue();
                });
        }

        public static IOrderedEnumerable<TSource> OrderByDescending<TSource>(this IEnumerable<TSource> source, Func<TSource, IList> keySelector)
        {
            return source.OrderByDescending(o => GetOrderValue(keySelector(o)));
        }

        private static string GetOrderValue(IList list)
        {
            if(list == null)
            {
                return null;
            }
            string orderValue = "";
            foreach (object obj in list)
            {
                orderValue += (obj as IOrderable).GetOrderValue();
            }
            return orderValue;
        }
    }
}
