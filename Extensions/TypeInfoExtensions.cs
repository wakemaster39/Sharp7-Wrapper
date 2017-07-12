using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Sharp7.Extensions
{
    public static class TypeInfoExtensions
    {
        public static IEnumerable<FieldInfo> GetAllFields(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredFields).GroupBy(x => x.Name).Select(group => group.First());

        public static IEnumerable<PropertyInfo> GetAllProperties(this TypeInfo typeInfo)
            => GetAll(typeInfo, ti => ti.DeclaredProperties).GroupBy(x => x.Name).Select(group => group.First());

        private static IEnumerable<T> GetAll<T>(TypeInfo typeInfo, Func<TypeInfo, IEnumerable<T>> accessor)
        {
            while (typeInfo != null)
            {
                foreach (var t in accessor(typeInfo))
                {
                    yield return t;
                }

                typeInfo = typeInfo.BaseType?.GetTypeInfo();
            }
        }
    }
}
