using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace CsvGenerator.Tests
{

    [TestClass]
    public class ColumnTests
    {

        [TestMethod]
        public void NamedTest()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text with 'quotes' inside";

            string csvContent = new List<Person>().Csv().Columns(column =>
            {
                column.For(p => p.Name).Named("Name of Person");
                column.For(p => p.Note).Named("Notes");
            }).ToString();

            string expectedResult = "Name of Person,Notes\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void FormatTest()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.BirthDate = new DateTime(1960, 2, 22);


            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.BirthDate).Format("{0:dd.MM.yyyy}");
            }).IncludeHeader(false).ToString();

            string expectedResult = "22.02.1960\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void CellConditionTest()
        {
            var p1 = Person.GetSamplePerson();
            p1.Name = "Chuck";
            p1.Income = 11111;

            var p2 = Person.GetSamplePerson();
            p2.Name = "Jane";
            p2.Income = 500000;

            string csvContent = new List<Person>() { p1, p2 }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Income).CellCondition(p=> p.Income < 100000);
            }).ToString();

            string expectedResult = "Name,Income\r\nChuck,11111\r\nJane,\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void CustomColumnTest()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.Custom(p => { return "Street 123, 12345, City"; }).Named("Address");
            }).ToString();

            string expectedResult = "Name,Address\r\nChuck Testa,\"Street 123, 12345, City\"\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }
    }
}
