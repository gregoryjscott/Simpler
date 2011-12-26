using System;

namespace Simpler
{
    public static class Mapper
    {
        /// <summary>
        /// Uses AutoMapper if the TTarget is a class other than String, otherwise it performs
        /// a conversion.
        /// </summary>
        public static TTarget Map<TTarget>(object source)
        {
            var targetType = typeof (TTarget);

            if (targetType.FullName == "System.String" 
                ||
                !targetType.IsClass)
            {
                return (TTarget)Convert.ChangeType(source, targetType);
            }

            return AutoMapper.Mapper.DynamicMap<TTarget>(source);
        }

        /// <summary>
        /// This is effectively an additional overload to AutoMapper's DynamicMap method.
        /// </summary>
        public static void Map<TTarget>(object source, TTarget target)
        {
            var sourceType = source == null ? typeof(object) : source.GetType();
            var targetType = typeof(TTarget);
            AutoMapper.Mapper.DynamicMap(source, target, sourceType, targetType);
        }
    }
}
