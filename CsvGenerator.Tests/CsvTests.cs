using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CsvGenerator.Tests
{
    /// <summary>
    /// Model class for tests
    /// </summary>
    public class Person
    {

        public string Name { get; set; }    // public property
        public string Email;                // public variable
        public string Phone { get; set; }
        public int Height { get; set; }
        public int Weight { get; set; }
        public int Income { get; set; }
        public DateTime BirthDate { get; set; }
        public string Note { get; set; }

        public static Person GetSamplePerson()
        {
            return new Person()
            {
                Name = "Chuck Testa",
                Email = "chuck@testa.com",
                Height = 186,
                Weight = 85,
                Income = 11111,
                Phone = "777666444",
                BirthDate = new DateTime(1982, 1, 11),
                Note = "Text with \r\n newline"
            };
        }
    }

    [TestClass]
    public class CsvTests
    {
        List<Person> listOfPerson = new List<Person>();

        [TestInitialize]
        public void Initialize()
        {
            var p = new Person() { Name = "Jane Testa", Email = "jane@testa.com", Height = 176, Weight = 65, Income = 10000, Phone = "777666777", BirthDate = new DateTime(1984, 10, 16), Note = "Text with \"Quotes\"" };
            var p2 = new Person() { Name = "Chuck Norris", Email = "chuck@norris.com", Height = 178, Weight = 80, Income = 22222, Phone = "777666555", BirthDate = new DateTime(1940, 3, 10), Note = "Text, with, delimiters" };
            //var p3 = new Person() { Name = "Chuck CrLfTesta", Email = "chuck@testa.com", Height = 186, Weight = 85, Income = 11111, Phone = "777666444", BirthDate = new DateTime(1982, 1, 11), Note = "Text with \r\n newline" };
            //var p4 = new Person() { Name = "Chuck CrTesta", Email = "chuck@testa.com", Height = 186, Weight = 85, Income = 11111, Phone = "777666444", BirthDate = new DateTime(1982, 1, 11), Note = "Text with \r newline" };
            //var p5 = new Person() { Name = "Chuck LfTesta", Email = "chuck@testa.com", Height = 186, Weight = 85, Income = 11111, Phone = "777666444", BirthDate = new DateTime(1982, 1, 11), Note = "Text with \n newline" };

            listOfPerson.Add(p);
            listOfPerson.Add(p2);
            //listOfPerson.Add(p3);
            //listOfPerson.Add(p4);
            //listOfPerson.Add(p5);
        }

        //string csvContent = listOfPerson.Csv().Columns(column =>
        //{
        //    column.For(person => person.Name);
        //    column.For(person => person.Email);
        //    column.For(person => person.Phone);
        //    column.For(person => person.Height);
        //    column.For(person => person.Weight);
        //    column.For(person => person.BirthDate);
        //    column.For(person => person.Note);
        //}).IncludeEndingLineBreak(false).ToString();

        [TestMethod]
        public void RecordsSeparatedByDefaultLineBreakCrLf()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void RecordsSeparatedByDefaultLineBreakCrLfIfSetUnknownLineBreak()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).SetLineBreak((LineBreak)666).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void RecordsSeparatedByLineBreakCrLf()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).SetLineBreak(LineBreak.CrLf).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void RecordsSeparatedByLineBreakCr()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).SetLineBreak(LineBreak.Cr).ToString();

            string expectedResult = "Name\rJane Testa\rChuck Norris\r";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void RecordsSeparatedByLineBreakLf()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).SetLineBreak(LineBreak.Lf).ToString();

            string expectedResult = "Name\nJane Testa\nChuck Norris\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void LastRecordHasEndingLineBreakByDefault()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void LastRecordHasEndingLineBreak()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).IncludeEndingLineBreak(true).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void LastRecordCannotHaveEndingLineBreak()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).IncludeEndingLineBreak(false).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void IncludeHeaderByDefault()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void IncludeHeader()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).IncludeHeader(true).ToString();

            string expectedResult = "Name\r\nJane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void DoesNotIncludeHeader()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
            }).IncludeHeader(false).ToString();

            string expectedResult = "Jane Testa\r\nChuck Norris\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void FieldsDelimiterDefaultComma()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
                column.For(person => person.Email);
            }).ToString();

            string expectedResult = "Name,Email\r\nJane Testa,jane@testa.com\r\nChuck Norris,chuck@norris.com\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void FieldsDelimiterSemicolon()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
                column.For(person => person.Email);
            }).SetFieldDelimiter(";").ToString();

            string expectedResult = "Name;Email\r\nJane Testa;jane@testa.com\r\nChuck Norris;chuck@norris.com\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesNotUsedByDefault()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
                column.For(person => person.Email);
            }).ToString();

            string expectedResult = "Name,Email\r\nJane Testa,jane@testa.com\r\nChuck Norris,chuck@norris.com\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesNotUsed()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
                column.For(person => person.Email);
            }).UseSurroundingQuotesAlways(false).ToString();

            string expectedResult = "Name,Email\r\nJane Testa,jane@testa.com\r\nChuck Norris,chuck@norris.com\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUseAllways()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
                column.For(person => person.Email);
            }).UseSurroundingQuotesAlways(true).ToString();

            string expectedResult = "\"Name\",\"Email\"\r\n\"Jane Testa\",\"jane@testa.com\"\r\n\"Chuck Norris\",\"chuck@norris.com\"\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUseAllwaysSingleQuote()
        {
            string csvContent = listOfPerson.Take(2).Csv().Columns(column =>
            {
                column.For(person => person.Name);
                column.For(person => person.Email);
            }).UseSurroundingQuotesAlways(true).SetFieldQuote("'").ToString();

            string expectedResult = "'Name','Email'\r\n'Jane Testa','jane@testa.com'\r\n'Chuck Norris','chuck@norris.com'\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedByDefaultIfDefaultNewlineInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text with \r\n newline";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).ToString();

            string expectedResult = "Name,Note\r\nChuck Testa,\"Text with \r\n newline\"\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedByDefaultIfNewlineCrLfInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text with \r\n newline";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).SetLineBreak(LineBreak.CrLf).ToString();

            string expectedResult = "Name,Note\r\nChuck Testa,\"Text with \r\n newline\"\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedByDefaultIfNewlineCrInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text with \r newline";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).SetLineBreak(LineBreak.Cr).ToString();

            string expectedResult = "Name,Note\rChuck Testa,\"Text with \r newline\"\r";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedByDefaultIfNewlineLfInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text with \n newline";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).SetLineBreak(LineBreak.Lf).ToString();

            string expectedResult = "Name,Note\nChuck Testa,\"Text with \n newline\"\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedByDefaultIfQuoteInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text with \"quotes\" inside";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).ToString();

            string expectedResult = "Name,Note\r\nChuck Testa,\"Text with \"\"quotes\"\" inside\"\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedByDefaultIfDefaultFieldDelimiterInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text, with, default, delimiter";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).ToString();

            string expectedResult = "Name,Note\r\nChuck Testa,\"Text, with, default, delimiter\"\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedByDefaultIfFieldDelimiterSemicolonInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text; with; semicolon; delimiter";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).SetFieldDelimiter(";").ToString();

            string expectedResult = "Name;Note\r\nChuck Testa;\"Text; with; semicolon; delimiter\"\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }

        [TestMethod]
        public void SurroundingQuotesUsedIfSingleQuoteInText()
        {
            var person = Person.GetSamplePerson();
            person.Name = "Chuck Testa";
            person.Note = "Text with 'quotes' inside";

            string csvContent = new List<Person>() { person }.Csv().Columns(column =>
            {
                column.For(p => p.Name);
                column.For(p => p.Note);
            }).SetFieldQuote("'").ToString();

            string expectedResult = "Name,Note\r\nChuck Testa,'Text with ''quotes'' inside'\r\n";

            Assert.AreEqual(csvContent, expectedResult);
        }
    }
}
