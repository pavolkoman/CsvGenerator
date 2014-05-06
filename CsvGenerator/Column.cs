using System;

namespace CsvGenerator
{

    public class Column<T>: IColumn<T> where T: class 
    {
        private string _name;
        private readonly Func<T, object> _columnValueFunc;
        private Func<T, bool> _cellCondition = x => true;
        private string _format;

        /// <summary>
        /// Name of the column
        /// </summary>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Gets the value for a particular cell in this column
        /// </summary>
        /// <param name="instance">Instance from which the value should be obtained</param>
        /// <returns>Item to be rendered</returns>
        public object GetValue(T instance)
        {
            if (!_cellCondition(instance))
            {
                return null;
            }

            var value = _columnValueFunc(instance);

            if (!string.IsNullOrEmpty(_format))
            {
                value = string.Format(_format, value);
            }

            return value;
        }

        public IColumn<T> Named(string name)
        {
            _name = name;
            return this;
        }

        public IColumn<T> Format(string format)
        {
            _format = format;
            return this;
        }

        /// <summary>
		/// Creates a new instance of the Column class
		/// </summary>
		public Column(Func<T, object> columnValueFunc, string name)
		{
			_name = name;
			_columnValueFunc = columnValueFunc;
		}

        public IColumn<T> CellCondition(Func<T, bool> func)
        {
            _cellCondition = func;
            return this;
        }
    }
}
