namespace Simpler
{
    public class Mapper
    {
        /// <summary>
        /// This is effectively an additional overload to AutoMapper's DynamicMap method.
        /// </summary>
        internal static void DynamicMap<TDestination>(object source, TDestination destination)
        {
            var sourceType = source == null ? typeof(object) : source.GetType();
            var destinationType = typeof(TDestination);
            AutoMapper.Mapper.DynamicMap(source, destination, sourceType, destinationType);
        }
    }
}
