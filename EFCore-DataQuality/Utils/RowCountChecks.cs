using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCore_DataQuality.Utils
{
    public static class RowCountChecks
    {
        public static int GetRowCount<T>(this IEnumerable<T> extList) where T : class
        {
			try
			{
				return extList.Count();
			}
			catch (Exception e)
			{
                Console.WriteLine($"An error occured. Please check if table is available or table is populated with data {e.Message}");				
			}
			return 0;
        }
    }
}
