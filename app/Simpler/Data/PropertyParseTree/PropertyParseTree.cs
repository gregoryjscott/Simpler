using System;
using System.Collections.Generic;
using System.Linq;

namespace Simpler.Data.PropertyParseTree
{
    /// <summary>
    /// A hierarchical collection of properties that can reference columns in a data reader, each represented by a PropertyParseTreeNode.
    /// </summary>
    public abstract class PropertyParseTree
    {
        /// <summary>
        /// The property type of the current node
        /// </summary>
        public Type PropertyType { get; set; }


        /// <summary>
        /// A List of PropertyParseTreeNode that represents the property parse tree nodes assigned to the current property parse tree node.
        /// </summary>
        public List<PropertyParseTreeNode> Nodes { get; set; }

        /// <summary>
        /// Creates an instance of the specified type using the constructor that best matches the specified parameters.
        /// </summary>
        /// <param name="value">The default value for the newly created object</param>
        /// <returns>A reference to the newly created object.</returns>
        public abstract object CreateObject(object value = null);

        /// <summary>
        /// Initializes a new instance of the PropertyParseTree class.
        /// </summary>
        public PropertyParseTree()
        {
            Nodes = new List<PropertyParseTreeNode>();
        }

        public PropertyParseTreeNode this[string index]
        {
            get
            {
                return Nodes.FirstOrDefault(x => x.Name == index);
            }

            set
            {
                var listIndex = Nodes.FindIndex(x => x.Name == index);
                value.Name = index;
                if (listIndex == -1)
                {
                    Nodes.Add(value);
                }
                else
                {
                    Nodes[listIndex] = value;
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
    }
}
