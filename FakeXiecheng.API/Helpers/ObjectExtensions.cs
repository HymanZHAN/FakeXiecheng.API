using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace FakeXiecheng.API.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(
            this TSource source,
            string fields
        )
        {
            if (source == null)
            {
                throw new ArgumentException(nameof(source));
            }

            var shapedObject = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                // all public properties should be in the ExpandoObject 
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.IgnoreCase
                                                                  | BindingFlags.Public
                                                                  | BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    // get the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(source);

                    // add the field to the ExpandoObject
                    ((IDictionary<string, object>)shapedObject).Add(propertyInfo.Name, propertyValue);
                }

                return shapedObject;
            }

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

                var propertyValue = propertyInfo.GetValue(source);
                ((IDictionary<string, object>)shapedObject).Add(propertyInfo.Name, propertyValue);
            }

            return shapedObject;
        }
    }
}