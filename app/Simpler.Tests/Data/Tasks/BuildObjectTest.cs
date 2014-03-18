using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using NUnit.Framework;
using Simpler.Data.Tasks;
using Moq;
using System.Data;
using Simpler.Tests.Core.Mocks;

namespace Simpler.Tests.Data.Tasks
{
    [TestFixture]
    public class BuildObjectTest
    {
        public static Simpler.Data.PropertyParseTree.PropertyParseTree get_parse_tree_from_data_table(DataTable table, Type type)
        {
            var buildPropertyParseTree = new BuildPropertyParseTree();
            buildPropertyParseTree.In.Columns = table.Columns.Cast<DataColumn>().Select((x, i) => new { x.ColumnName, i }).ToDictionary(x => x.ColumnName, x => x.i);
            buildPropertyParseTree.In.InitialType = type;
            buildPropertyParseTree.Execute();
            return buildPropertyParseTree.Out.PropertyParseTree;
        }

        [Test]
        public void should_create_root_typed_object()
        {
            //Arrange
            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "John Doe" });

            var dataReader = table.CreateDataReader();
            dataReader.Read();

            var task = Task.New<BuildObject<MockPerson>>();
            task.In.DataRecord = dataReader;
            task.In.PropertyParseTree = get_parse_tree_from_data_table(table, typeof(MockPerson));

            //Act
            task.Execute();

            //Assert
            Assert.That(task.Out.Object, Is.Not.Null);
            Assert.That(task.Out.Object, Is.TypeOf(typeof(MockPerson)));
        }

        [Test]
        public void should_create_root_dynamic_object()
        {
            //Arrange
            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "John Doe" });

            var dataReader = table.CreateDataReader();
            dataReader.Read();

            var task = Task.New<BuildObject<dynamic>>();
            task.In.DataRecord = dataReader;
            task.In.PropertyParseTree = get_parse_tree_from_data_table(table, typeof(object));

            //Act
            task.Execute();

            //Assert
            Assert.That(task.Out.Object, Is.Not.Null);
            Assert.That(task.Out.Object, Is.TypeOf(typeof(ExpandoObject)));
        }

        [Test]
        public void should_assign_simple_properties()
        {
            //Arrange
            var table = new DataTable();
            table.Columns.Add("Name", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "John Doe" });

            var dataReader = table.CreateDataReader();
            dataReader.Read();

            var task = Task.New<BuildObject<MockPerson>>();
            task.In.DataRecord = dataReader;
            task.In.PropertyParseTree = get_parse_tree_from_data_table(table, typeof(MockPerson));

            //Act
            task.Execute();

            //Assert
            Assert.That(task.Out.Object, Is.Not.Null);
            Assert.That(task.Out.Object.Name, Is.EqualTo("John Doe"));
        }

        [Test]
        public void should_assign_nested_properties()
        {
            //Arrange
            var table = new DataTable();
            table.Columns.Add("PetName", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "Spot" });

            var dataReader = table.CreateDataReader();
            dataReader.Read();

            var task = Task.New<BuildObject<MockPerson>>();
            task.In.DataRecord = dataReader;
            task.In.PropertyParseTree = get_parse_tree_from_data_table(table, typeof(MockPerson));

            //Act
            task.Execute();

            //Assert
            Assert.That(task.Out.Object, Is.Not.Null);
            Assert.That(task.Out.Object.Pet.Name, Is.EqualTo("Spot"));
        }

        [Test]
        public void should_assign_array_properties()
        {
            //Arrange
            var table = new DataTable();
            table.Columns.Add("Vehicles0Make", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "Jeep" });

            var dataReader = table.CreateDataReader();
            dataReader.Read();

            var task = Task.New<BuildObject<MockPerson>>();
            task.In.DataRecord = dataReader;
            task.In.PropertyParseTree = get_parse_tree_from_data_table(table, typeof(MockPerson));

            //Act
            task.Execute();

            //Assert
            Assert.That(task.Out.Object, Is.Not.Null);
            Assert.That(task.Out.Object.Vehicles[0].Make, Is.EqualTo("Jeep"));
        }

        [Test]
        public void should_assign_dynamic_properties()
        {
            //Arrange
            var table = new DataTable();
            table.Columns.Add("OtherCity", Type.GetType("System.String"));
            table.Rows.Add(new object[] { "Anchorage" });

            var dataReader = table.CreateDataReader();
            dataReader.Read();

            var task = Task.New<BuildObject<MockPerson>>();
            task.In.DataRecord = dataReader;
            task.In.PropertyParseTree = get_parse_tree_from_data_table(table, typeof(MockPerson));

            //Act
            task.Execute();

            //Assert
            Assert.That(task.Out.Object, Is.Not.Null);
            Assert.That(task.Out.Object.Other.City, Is.EqualTo("Anchorage"));
        }
    }
}