using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using FakeXiecheng.API.Services;

namespace FakeXiecheng.API.Helpers
{
    public static class IQueryableExtensions
    {
        public static IQueryable<T> ApplySort<T>(
            this IQueryable<T> source,
            string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (mappingDictionary == null)
            {
                throw new ArgumentNullException("mappingDictionary");
            }
            if (String.IsNullOrEmpty(orderBy))
            {
                return source;
            }

            var orderByString = string.Empty;
            var orderByAfterSplit = orderBy.Split(',');

            foreach (var order in orderByAfterSplit)
            {
                var trimmedOrderBy = order.Trim();
                var orderDescending = trimmedOrderBy.EndsWith(" desc");

                var indexOfFirstSpace = trimmedOrderBy.IndexOf(' ');
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderBy
                    : trimmedOrderBy.Remove(indexOfFirstSpace);

                if (!mappingDictionary.ContainsKey(propertyName))
                {
                    throw new ArgumentNullException($"Key mapping for {propertyName} is missing.");
                }

                var propertyNameMappingValue = mappingDictionary[propertyName];
                if (propertyNameMappingValue == null)
                {
                    throw new ArgumentNullException("propertyMappingValue");
                }

                foreach (var destinationProperty in propertyNameMappingValue.DestinationProperties.Reverse())
                {
                    orderByString = orderByString
                        + (string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ")
                        + destinationProperty
                        + (orderDescending ? " descending" : " ascending");
                }

            }
            return source.OrderBy(orderByString);
        }
    }
}