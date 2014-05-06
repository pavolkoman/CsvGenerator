using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CsvGenerator
{

    public class Csv<T> : ICsv<T> where T : class
    {

        IList<Column<T>> _csvModelColumns = new ColumnBuilder<T>();

        private const string Cr = "\r";
        private const string Lf = "\n";
        private const string CrLf = "\r\n";

        private string _lineBreak = CrLf;
        private string _fieldDelimiter = ",";
        private string _fieldQuote = "\"";
        private bool _includeEndlingLineBreak = true;
        private bool _includeHeader = true;
        private bool _useSurroundingQuotesAlways = false;

        /// <summary>
        /// Creates a new instance of the Grid class.
        /// </summary>
        /// <param name="dataSource">The datasource for the grid</param>
        /// <param name="context"></param>
        public Csv(IEnumerable<T> dataSource)
        {
            DataSource = dataSource;
        }

        /// <summary>
        /// The datasource for the grid.
        /// </summary>
        private IEnumerable<T> DataSource { get; set; }

        public ICsv<T> Columns(Action<ColumnBuilder<T>> columnBuilder)
        {
            var builder = new ColumnBuilder<T>();
            columnBuilder(builder);

            foreach (var column in builder)
            {
                _csvModelColumns.Add(column);
            }

            return this;
        }

        public ICsv<T> SetFieldDelimiter(string delimiter)
        {
            _fieldDelimiter = delimiter;
            return this;
        }

        public ICsv<T> SetFieldQuote(string quote)
        {
            _fieldQuote = quote;
            return this;
        }

        public ICsv<T> SetLineBreak(LineBreak lineBreak)
        {
            switch (lineBreak)
            {
                case LineBreak.CrLf:
                    _lineBreak = CrLf;
                    break;
                case LineBreak.Cr:
                    _lineBreak = Cr;
                    break;
                case LineBreak.Lf:
                    _lineBreak = Lf;
                    break;
                default:
                    _lineBreak = CrLf;
                    break;
            }

            return this;
        }

        public ICsv<T> IncludeEndingLineBreak(bool value)
        {
            _includeEndlingLineBreak = value;
            return this;
        }

        public ICsv<T> IncludeHeader(bool value)
        {
            _includeHeader = value;
            return this;
        }

        public ICsv<T> UseSurroundingQuotesAlways(bool value)
        {
            _useSurroundingQuotesAlways = value;
            return this;
        }

        public string Generate()
        {
            var writer = new StringWriter();
            writer.NewLine = _lineBreak;

            if (_includeHeader)
            {
                GenerateHeader(writer);
            }

            GenerateItems(writer);

            return writer.ToString();
        }

        public override string ToString()
        {
            return Generate();

        }

        /// <summary>
        /// Generates header record (heading row)
        /// </summary>
        private void GenerateHeader(TextWriter writer)
        {

            bool firstColumn = true;
            foreach (var column in _csvModelColumns)
            {

                if (!firstColumn)
                {
                    writer.Write(_fieldDelimiter);
                }

                string quote = GetQuote(column.Name);
                writer.Write(quote);
                writer.Write(SanitizeQuotes(column.Name));
                writer.Write(quote);

                firstColumn = false;
            }

            writer.WriteLine(); 
        }

        /// <summary>
        /// Tests if quotes should be used around the field 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private bool UseQuotes(string text)
        {
            if (_useSurroundingQuotesAlways)
            {
                return true;
            }

            return text.Contains(_lineBreak) || text.Contains(_fieldDelimiter) || text.Contains(_fieldQuote);
        }

        /// <summary>
        /// Returns quote char used for current field 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string GetQuote(string text)
        {
            if (UseQuotes(text))
            {
                return _fieldQuote;
            }

            return string.Empty; 
        }

        /// <summary>
        /// Doubles quotes if contained in text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string SanitizeQuotes(string text)
        {
            return text.Replace(_fieldQuote, _fieldQuote + _fieldQuote);
        }

        /// <summary>
        /// Generates records from data source
        /// </summary>
        private void GenerateItems(TextWriter writer)
        {
            var lastItem = DataSource.LastOrDefault();

            foreach (var item in DataSource)
            {
                bool firstColumn = true;
                foreach (var column in _csvModelColumns)
                {
                    if (!firstColumn)
                    {
                        writer.Write(_fieldDelimiter);
                    }

                    var cellValue = column.GetValue(item);
                    var cellText = string.Empty;
                    if (cellValue != null)
                    {
                        cellText = cellValue.ToString();
                    }

                    string quote = GetQuote(cellText);
                    writer.Write(quote);
                    writer.Write(SanitizeQuotes(cellText));
                    writer.Write(quote);

                    firstColumn = false;
                }

                if (_includeEndlingLineBreak || item != lastItem)
                {
                    writer.WriteLine(); 
                }
            }
        }

    }
}
