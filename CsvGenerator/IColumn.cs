using System;

namespace CsvGenerator
{
    /// <summary>
    /// Grid Column fluent interface
    /// </summary>
    public interface IColumn<T>
    {
        /// <summary>
        /// Specified an explicit name for the column.
        /// </summary>
        /// <param name="name">Name of column</param>
        /// <returns></returns>
        IColumn<T> Named(string name);

        /// <summary>
        /// A custom format to use when building the cell's value
        /// </summary>
        /// <param name="format">Format to use</param>
        /// <returns></returns>
        IColumn<T> Format(string format);
        
        /// <summary>
        /// Delegate used to hide the contents of the cells in a column.
        /// </summary>
        IColumn<T> CellCondition(Func<T, bool> func);

 
    }
}
