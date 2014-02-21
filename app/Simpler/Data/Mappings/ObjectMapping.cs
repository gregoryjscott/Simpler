using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Simpler.Data
{
    public abstract class ObjectMapping : IEnumerable<ObjectMappingNode>
    {
        public Type PropertyType { get; set; }
        public List<ObjectMappingNode> Children { get; set; }

        public ObjectMapping()
        {
            Children = new List<ObjectMappingNode>();
        }

        public bool ContainsColumn(string columnName)
        {
            return Children.Any(x => x.Name == columnName);
        }

        public int Count()
        {
            return Children.Count;
        }

        public IEnumerator<ObjectMappingNode> GetEnumerator()
        {
            return Children.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public ObjectMappingNode this[string index]
        {
            get { return Children.FirstOrDefault(x => x.Name == index); }
            set
            {
                var listIndex = Children.FindIndex(x => x.Name == index);
                value.Name = index;
                if (listIndex == -1)
                {
                    Children.Add(value);
                }
                else
                {
                    Children[listIndex] = value;
                }
            }
        }
    }
}
