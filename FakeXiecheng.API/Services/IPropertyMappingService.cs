using System.Collections.Generic;

namespace FakeXiecheng.API.Services
{

    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool IsMappingAvailable<TSource, TDestination>(string fields);
        bool ArePropertiesAvailable<T>(string fields);
    }

}