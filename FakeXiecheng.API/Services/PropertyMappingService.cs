using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FakeXiecheng.API.Dto;
using FakeXiecheng.API.Models;

namespace FakeXiecheng.API.Services
{
    public class PropertyMappingService : IPropertyMappingService
    {
        private Dictionary<string, PropertyMappingValue> _touristRoutePropertyMapping =
            new Dictionary<string, PropertyMappingValue>(StringComparer.OrdinalIgnoreCase)
            {
                {"Id", new PropertyMappingValue(new List<string>(){"Id"})},
                {"Title", new PropertyMappingValue(new List<string>(){"Title"})},
                {"Rating", new PropertyMappingValue(new List<string>(){"Rating"})},
                {"OriginalPrice", new PropertyMappingValue(new List<string>(){"OriginalPrice"})},
            };

        private IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();

        public PropertyMappingService()
        {
            _propertyMappings.Add(new PropertyMapping<TouristRouteDto, TouristRoute>(_touristRoutePropertyMapping));
        }

        public bool ArePropertiesAvailable<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }
            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();
                var propertyInfo = typeof(T).GetProperty(
                    propertyName,
                    BindingFlags.IgnoreCase
                    | BindingFlags.Public
                    | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    return false;
                }
            }
            return true;
        }

        public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
        {
            var matchingMaping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

            if (matchingMaping.Count() == 1)
            {
                return matchingMaping.First()._mappingDictionary;
            }
            throw new Exception(
                $"Cannot find exact property mapping instance for <{typeof(TSource)},{typeof(TDestination)}>"
            );
        }

        public bool IsMappingAvailable<TSource, TDestination>(string fields)
        {
            var propertyMappings = GetPropertyMapping<TSource, TDestination>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                return true;
            }

            var fieldsAfterSplit = fields.Split(',');
            foreach (var field in fieldsAfterSplit)
            {
                var trimmedField = field.Trim();

                var indexOfFirstSpace = trimmedField.IndexOf(" ");
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedField
                    : trimmedField.Remove(indexOfFirstSpace);

                if (!propertyMappings.ContainsKey(propertyName))
                {
                    return false;
                }
            }
            return true;
        }
    }
}