using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace FakeXiecheng.API.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(
            this IEnumerable<TSource> source,
            string fields
        )
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            var expandoObjectList = new List<ExpandoObject>();

            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase
                                                                  | BindingFlags.Public
                                                                  | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    var propertyName = field.Trim();

                    var propertyInfo = typeof(TSource).GetProperty(
                        propertyName,
                        BindingFlags.IgnoreCase
                        | BindingFlags.Public
                        | BindingFlags.Instance
                    );

                    if (propertyInfo == null)
                    {
                        throw new Exception($"属性{propertyName}不存在于{typeof(TSource)}");
                    }

                    propertyInfoList.Add(propertyInfo);
                }
            }

            foreach (TSource sourceObject in source)
            {
                var shapedObject = new ExpandoObject();

                foreach (var propertyInfo in propertyInfoList)
                {
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    ((IDictionary<string, object>)shapedObject).Add(propertyInfo.Name, propertyValue);
                }

                expandoObjectList.Add(shapedObject);
            }

            return expandoObjectList;

        }
    }
}