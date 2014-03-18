using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Simpler.Data.Tasks
{
    public class BuildObjects<T> : InOutTask<BuildObjects<T>.Input, BuildObjects<T>.Output>  
    {
        public class Input
        {
            public IDataReader Reader { get; set; }
        }

        public class Output
        {
            public T[] Objects { get; set; }
        }

        public FindColumns FindColumns { get; set; }
        public BuildPropertyParseTree BuildPropertyParseTree { get; set; }
        public BuildObject<T> BuildObject { get; set; }

        public override void Execute()
        {
            var objectList = new List<T>();

            //read the first record off and determine the column mappings
            In.Reader.Read();

            FindColumns.In.Reader = In.Reader;
            FindColumns.Execute();

            BuildPropertyParseTree.In.Columns = FindColumns.Out.Columns;
            BuildPropertyParseTree.In.InitialType = typeof (T);
            BuildPropertyParseTree.Execute();
            
            do
            {
                BuildObject.In.PropertyParseTree = BuildPropertyParseTree.Out.PropertyParseTree;
                BuildObject.In.DataRecord = In.Reader;
                BuildObject.Execute();
                objectList.Add(BuildObject.Out.Object);
            } while (In.Reader.Read());

            Out.Objects = objectList.ToArray();
        }
    }
}
