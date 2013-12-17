using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;

namespace Simpler.Data.Tasks
{
    public class BuildDynamic : InOutTask<BuildDynamic.Input, BuildDynamic.Output> 
    {
        public class Input
        {
            public IDataRecord DataRecord { get; set; }
        }

        public class Output
        {
            public dynamic Object { get; set; }
        }

        class Dynamic : DynamicObject
        {
            readonly Dictionary<string, object> _members = new Dictionary<string, object>();

            public override bool TryGetMember(GetMemberBinder binder, out object result)
            {
                return _members.TryGetValue(binder.Name, out result);
            }

            public override bool TrySetMember(SetMemberBinder binder, object value)
            {
                _members[binder.Name.ToLower()] = value;
                return true;
            }

            public void AddMember(string memberName, object memberValue)
            {
                _members[memberName] = memberValue;
            }
        }

        public override void Execute()
        {
            Out.Object = new ExpandoObject();
            var OutObject = Out.Object as IDictionary<string, Object>;
            for (var i = 0; i < In.DataRecord.FieldCount; i++)
            {
                var columnName = In.DataRecord.GetName(i);
                var columnValue = In.DataRecord[columnName];
                if (columnValue.GetType() != typeof(System.DBNull))
                {
                    OutObject.Add(columnName, columnValue);
                }
            }
        }
    }
}
