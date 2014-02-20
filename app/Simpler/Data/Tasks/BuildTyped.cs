using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Simpler.Data.Tasks
{
    public class BuildTyped<T> : InOutTask<BuildTyped<T>.Input, BuildTyped<T>.Output>
    {
        public class Input
        {
            public IDataRecord DataRecord { get; set; }
            public Dictionary<string, BuildMappings.ObjectMapping> Map { get; set; }
        }

        public class Output
        {
            public T Object { get; set; }
        }

        public void Parse(Dictionary<string, BuildMappings.ObjectMapping> objectMappings, object instance)
        {
            var instanceType = instance.GetType();
            foreach (var objectMapping in objectMappings)
            {
                var propertyInfo = instanceType.GetProperty(objectMapping.Key);
                object propertyInstance;
                Type propertyType;
                if (propertyInfo != null)
                {
                    propertyInstance = propertyInfo.GetValue(instance, null);
                    propertyType = propertyInfo.PropertyType;
                }
                else if (instanceType.IsArray)
                {
                    var array = (Array)instance;
                    var index = int.Parse(objectMapping.Key);
                    propertyInstance = array.GetValue(index);
                    propertyType = instanceType.GetElementType();
                }
                else
                {
                    continue;
                }
                
                if (propertyInstance == null)
                {
                    if (propertyType.IsArray)
                    {
                        propertyInstance = Activator.CreateInstance(propertyType, new object[] { objectMapping.Value.Children.Count });
                    }
                    else if (objectMapping.Value.Column == null)
                    {
                        propertyInstance = Activator.CreateInstance(propertyType, null);
                    }
                    else
                    {
                        var value = In.DataRecord.GetValue((int) objectMapping.Value.Column);

                        if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                        {
                            propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType);
                        }

                        if (propertyType.IsEnum)
                        {
                            value = Enum.Parse(propertyType, value.ToString());
                        }

                        propertyInstance = Convert.ChangeType(value, propertyType);
                    }

                    if (instanceType.IsArray)
                    {
                        ((Array) instance).SetValue(propertyInstance, int.Parse(objectMapping.Key));
                    }
                    else
                    {
                        propertyInfo.SetValue(instance, propertyInstance, null);
                    }

                   
                }
                Parse(objectMapping.Value.Children, propertyInstance);
            }
        }

        public override void Execute()
        {
            var instance = (T) Activator.CreateInstance(typeof (T));
            Parse(In.Map, instance);
            Out.Object = instance;
        }
    }
}