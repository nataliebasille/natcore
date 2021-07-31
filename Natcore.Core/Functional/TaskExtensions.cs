namespace System.Threading.Tasks
{
    public static class TaskExtensions
    {
        public static Task<TResult> Select<TSource, TResult>(
           this Task<TSource> source, 
           Func<TSource, TResult> selector
        )
        {
            return source.ContinueWith(result => selector(result.Result));
        }

        public static async Task<TResult> SelectMany<TSource, TResult>(
            this Task<TSource> source,
            Func<TSource, Task<TResult>> selector
        )
        {
            var result = await source.ConfigureAwait(false);
            return await selector(result).ConfigureAwait(false);
        }

        public static async Task<TResult> SelectMany<T, TSelect, TResult>(this Task<T> source, Func<T, Task<TSelect>> selector, Func<T, TSelect, TResult> resultSelector)
        {
            var sourceResult = await source.ConfigureAwait(false);
            var nextResult = await selector(sourceResult).ConfigureAwait(false);

            return resultSelector(sourceResult, nextResult);
        }
    }
}
