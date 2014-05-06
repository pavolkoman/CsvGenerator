using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace CsvGenerator
{
    /// <summary>
    /// Builds columns
    /// </summary>
    public class ColumnBuilder<T> : IList<Column<T>> where T : class
    {
        private readonly List<Column<T>> _columns = new List<Column<T>>();

        /// <summary>
        /// Creates a column for custom code.
        /// </summary>
        public IColumn<T> Custom(Func<T, object> customColumnCode)
        {
            var column = new Column<T>(customColumnCode, "");
            Add(column);
            return column;
        }

        /// <summary>
        /// Specifies a column should be constructed for the specified property.
        /// </summary>
        /// <param name="propertySpecifier">Lambda that specifies the property for which a column should be constructed</param>
        public IColumn<T> For(Expression<Func<T, object>> propertySpecifier)
        {
            var memberExpression = GetMemberExpression(propertySpecifier);
            var inferredName = memberExpression == null ? null : memberExpression.Member.Name;
            var column = new Column<T>(propertySpecifier.Compile(), inferredName);

            Add(column);

            return column;
        }

        protected IList<Column<T>> Columns
        {
            get { return _columns; }
        }

        public IEnumerator<Column<T>> GetEnumerator()
        {
            return _columns
                .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public static MemberExpression GetMemberExpression(LambdaExpression expression)
        {
            return RemoveUnary(expression.Body) as MemberExpression;
        }

        private static Expression RemoveUnary(Expression body)
        {
            var unary = body as UnaryExpression;
            if (unary != null)
            {
                return unary.Operand;
            }
            return body;
        }

        protected virtual void Add(Column<T> column)
        {
            _columns.Add(column);
        }

        void ICollection<Column<T>>.Add(Column<T> column)
        {
            Add(column);
        }

        void ICollection<Column<T>>.Clear()
        {
            _columns.Clear();
        }

        bool ICollection<Column<T>>.Contains(Column<T> column)
        {
            return _columns.Contains(column);
        }

        void ICollection<Column<T>>.CopyTo(Column<T>[] array, int arrayIndex)
        {
            _columns.CopyTo(array, arrayIndex);
        }

        bool ICollection<Column<T>>.Remove(Column<T> column)
        {
            return _columns.Remove(column);
        }

        int ICollection<Column<T>>.Count
        {
            get { return _columns.Count; }
        }

        bool ICollection<Column<T>>.IsReadOnly
        {
            get { return false; }
        }

        int IList<Column<T>>.IndexOf(Column<T> item)
        {
            return _columns.IndexOf(item);
        }

        void IList<Column<T>>.Insert(int index, Column<T> item)
        {
            _columns.Insert(index, item);
        }

        void IList<Column<T>>.RemoveAt(int index)
        {
            _columns.RemoveAt(index);
        }

        Column<T> IList<Column<T>>.this[int index]
        {
            get { return _columns[index]; }
            set { _columns[index] = value; }
        }
    }
}
