namespace EFCore_DataQuality.Utils
{
    public static class Extensions
    {
        public static IEnumerable<T> GetMoreThanOnceRepeated<T> (this IEnumerable<T> extList, Func<T, object> groupProps) where T : class
        {
            return extList
                .GroupBy(groupProps)
                .SelectMany(item => item.Skip(1)); //Skips the first occurance and return all the others that repeats
        }

        public static IEnumerable<T> GetAllDuplicates<T>(this IEnumerable<T> extList, Func<T, object> groupProps) where T: class
        {
            return extList
                .GroupBy(groupProps)
                .Where(item => item.Count() > 1)
                .SelectMany(item => item);
        }
    }
}
