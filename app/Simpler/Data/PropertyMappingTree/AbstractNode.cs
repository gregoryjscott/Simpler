using System;
using System.Collections.Generic;
using System.Linq;

namespace Simpler.Data.PropertyMappingTree
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractNode
    {
                /// <summary>
        /// The property type of the current node
        /// </summary>
        public Type PropertyType { get; set; }


        /// <summary>
        /// A List of Node that represents the property parse tree nodes assigned to the current property parse tree node.
        /// </summary>
        public List<AbstractNode> Children { get; set; }

        /// <summary>
        /// Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="value">The default value for the newly created object</param>
        /// <returns>A reference to the newly created object.</returns>
        public abstract object CreateObject(object value = null);

        /// <summary>
        /// Initializes a new instance of the RootNode class.
        /// </summary>
        public AbstractNode()
        {
            Children = new List<AbstractNode>();
        }

        public AbstractNode this[string index]
        {
            get
            {
                return Children.FirstOrDefault(x => x.Name == index);
            }

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

        /// <summary>
        /// 
        /// </summary>
        public bool IsDynamicProperty {
            get
            {
                return PropertyType.FullName == "System.Object";
            }
        }


        /// <summary>
        /// the name of the column from the data reader this node references.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The index of the column from the data reader this node references.
        /// </summary>
        public int? Index { get; set; }

        /// <summary>
        /// Sets the property value of a specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be set.</param>
        /// <param name="value">The new property value.</param>
        public abstract void SetValue(object obj, object value);

        /// <summary>
        /// Returns the property value of a specified object.
        /// </summary>
        /// <param name="obj">The object whose property value will be returned.</param>
        /// <returns>The property value of the specified object.</returns>
        public abstract object GetValue(object obj);
    }
}
