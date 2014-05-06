using System;

namespace CsvGenerator
{
    public interface ICsv<T> where T : class
    {
        /// <summary>
        /// Defines columns of a CSV
        /// </summary>
        /// <param name="columnBuilder"></param>
        /// <returns></returns>
        ICsv<T> Columns(Action<ColumnBuilder<T>> columnBuilder);

        /// <summary>
        /// Defines if quotes have to be used around every field. 
        /// Default is false.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ICsv<T> UseSurroundingQuotesAlways(bool value);

        /// <summary>
        /// Defines if output should contain header - names of columns.
        /// Default is True.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ICsv<T> IncludeHeader(bool value);

        /// <summary>
        /// Defines if Line Break is written after last field of last record. 
        /// Default is True
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        ICsv<T> IncludeEndingLineBreak(bool value);

        /// <summary>
        /// Allows change line break chars between CRLF, CR and LF 
        /// Default is CRLF
        /// </summary>
        /// <param name="lineBreak"></param>
        /// <returns></returns>
        ICsv<T> SetLineBreak(LineBreak lineBreak);

        /// <summary>
        /// Specifies char (string) that is used as a "quote character" 
        /// Default is double quote (")
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        ICsv<T> SetFieldQuote(string quote);

        /// <summary>
        /// Specifies char (string) that is used as delimiter (separator) between fields 
        /// Default is comma (,)
        /// </summary>
        /// <param name="delimiter"></param>
        /// <returns></returns>
        ICsv<T> SetFieldDelimiter(string delimiter);

        /// <summary>
        /// Executes generation of CSV content
        /// </summary>
        /// <returns></returns>
        string Generate();
    }
}
