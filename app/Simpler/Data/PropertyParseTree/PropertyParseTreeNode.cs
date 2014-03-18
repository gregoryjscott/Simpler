namespace Simpler.Data.PropertyParseTree
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PropertyParseTreeNode : PropertyParseTree
    {
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
