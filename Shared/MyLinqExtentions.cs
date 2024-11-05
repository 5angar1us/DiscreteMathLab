namespace Shared;

    internal static class MyLinqExtentions
{
    public static void ForEach<T>(this IEnumerable<T> sequence, Action<int, T> action)
    {
        if (sequence == null) throw new ArgumentNullException(nameof(sequence));
        if (action == null) throw new ArgumentNullException(nameof(action));

        int i = 0;
        foreach (T item in sequence)
        {
            action(i, item);
            i++;
        }
    }
}

