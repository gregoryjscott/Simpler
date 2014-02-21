using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Simpler.Data.Tasks
{
    public class BuildObject<T> : InOutTask<BuildObject<T>.Input, BuildObject<T>.Output>
    {
        public class Input
        {
            public IDataRecord DataRecord { get; set; }
            public ObjectMapping ObjectMapping { get; set; }
        }

        public class Output
        {
            public T Object { get; set; }
        }

        public void Parse(ObjectMappingNode objectMapping, object instance)
        {
            if (objectMapping.ColumnIndex == null)
            {
                objectMapping.SetValue(instance, null);
            }
            else
            {
                var value = In.DataRecord.GetValue((int)objectMapping.ColumnIndex);
                if (value != null && value.GetType() != typeof(DBNull))
                {
                    objectMapping.SetValue(instance, value);
                }
            }

            var childInstance = objectMapping.GetValue(instance);

            foreach (var children in objectMapping)
            {
                Parse(children, childInstance);
            }
        }

        public override void Execute()
        {
            object instance;
            if (typeof (T).FullName == "System.Object")
            {
                instance = new ExpandoObject();
            }
            else
            {
                instance = Activator.CreateInstance(typeof(T));
            }

            foreach (var objectMapping in In.ObjectMapping)
            {
                Parse(objectMapping, instance);
            }
            Out.Object = (T)instance;
        }
    }
}