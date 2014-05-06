using System.Collections.Generic;

namespace CsvGenerator
{
    public static class CsvExtensions
    {

        public static ICsv<T> Csv<T>(this IEnumerable<T> dataSource) where T : class
        {
            return new Csv<T>(dataSource);
        }

    }
}
