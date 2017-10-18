using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EIM.Tests
{
    public class RandomDataFiller
    {
        public RandomDataFiller()
        {
            Random = new Random();
        }

        public Random Random { set; get; }

        public virtual void Fill(object obj)
        {
            Type type = obj.GetType();
            object value = null;
            PropertyInfo[] propertys = type.GetProperties();
            foreach (PropertyInfo property in propertys)
            {
                if (!this.Validate(property, obj))
                {
                    continue;
                }
                MethodInfo setMethod = property.GetSetMethod(false);
                if (setMethod != null)
                {
                    value = this.GetValue(property, obj);
                    if (value != null)
                    {
                        property.SetValue(obj, value, null);
                    }
                }
            }
        }

        public virtual T GetValue<T>()
        {
            Type type = typeof(T);
            object value = GetValue(type);
            if (value != null)
            {
                return (T)value;
            }
            return default(T);
        }

        public T GetValue<T>(object source)
        {
            T obj = ObjectMapperHelper.Map<T>(source);
            this.Fill(obj);
            return obj;
        }

        protected virtual bool Validate(PropertyInfo property, object obj)
        {
            if (property.PropertyType.IsAbstract)
            {
                return false;
            }
            if (ReflectionHelper.HasAttribute<NotUpdateValueAttribute>(property))
            {
                object value = property.GetValue(obj, null);
                if(value != null)
                {
                    return false;
                }
            }
            else if (ReflectionHelper.HasAttribute<ReadonlyValueAttribute>(property))
            {
                return false;
            }
            
            return true;
        }

        protected virtual object GetValue(PropertyInfo property, object obj)
        {
            if (ReflectionHelper.HasAttribute<GuidStringValueAttribute>(property))
            {
                return Guid.NewGuid().ToString();
            }
            object value = this.GetValue(property.PropertyType);

            if (property.PropertyType == typeof(string) && ReflectionHelper.HasAttribute<LimitedLengthValueAttribute>(property))
            {
                LimitedLengthValueAttribute limitedLengthValueAttribute = ReflectionHelper.GetAttribute<LimitedLengthValueAttribute>(property);

                if (value != null && value.ToString().Length > limitedLengthValueAttribute.Length)
                {
                    value = value.ToString().Substring(0, limitedLengthValueAttribute.Length);
                }
            }
            return value;
        }

        public object GetValue(Type type)
        {
            object value = null;
            if (ReflectionHelper.IsIList(type))
            {
                IList list = (IList)Activator.CreateInstance(type);
                Type listItemType = ReflectionHelper.GetCollectionItemType(type);
                object item = null;
                if (this.GetValue(listItemType, out item))
                {
                    list.Add(item);
                }
                value = list;
            }
            else
            {
                bool provided = this.GetValue(type, out value);
                if (!provided && ReflectionHelper.HasDefaultConstructor(type))
                {
                    object typeInstance = Activator.CreateInstance(type);
                    this.Fill(typeInstance);
                    value = typeInstance;
                }
            }
            return value;
        }

        protected virtual bool GetValue(Type type, out object value)
        {
            value = null;
            bool provided = false;
            
            if (type == typeof(string))
            {
                value = this.Get16Guid();
                provided = true;
            }
            else if (type == typeof(Guid) || type == typeof(Guid?))
            {
                value = Guid.NewGuid();
                provided = true;
            }
            else if (type == typeof(bool) || type == typeof(bool?))
            {
                value = true;
                provided = true;
            }
            else if (type == typeof(int) || type == typeof(int?))
            {
                value = Random.Next(100000, 1000000);
                provided = true;
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                value = (float)Random.Next(0, 10000);
                provided = true;
            }
            else if (type == typeof(double)|| type == typeof(double?))
            {
                value = (double)Random.Next(0, 10000);
                provided = true;
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                value = (decimal)Random.Next(0, 10000);
                provided = true;
            }
            else if (type == typeof(DateTime)
                || type == typeof(DateTime?))
            {
                value = DateTime.Now.AddDays(Random.Next(1, 100)).Date;
                provided = true;
            }
            else if (type == typeof(byte))
            {
                value = (byte)Random.Next(byte.MinValue, byte.MaxValue);
                provided = true;
            }
            else if (ReflectionHelper.GetDefinitionType(type).IsEnum)
            {
                type = ReflectionHelper.GetDefinitionType(type);
                List<object> enumValues = EnumHelper.GetValues(type);
                value = enumValues[Random.Next(0, enumValues.Count)];
                provided = true;
            }
            return provided;
        }

        private string Get16Guid()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
                i *= ((int)b + 1);
            return string.Format("{0:x}", i - DateTime.Now.Ticks);  
        }

        public T GetRandomItem<T>(IList<T> list)
        {
            return list[this.Random.Next(0, list.Count - 1)];
        }
    }
}
