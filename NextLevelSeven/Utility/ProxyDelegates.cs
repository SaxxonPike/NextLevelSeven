namespace NextLevelSeven.Utility
{
    /// <summary>Represents a proxy that is triggered when a value attempts to change.</summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="oldValue">Old value.</param>
    /// <param name="newValue">New value.</param>
    internal delegate void ProxyChangePendingNotifier<in TValue>(TValue oldValue, TValue newValue);

    /// <summary>Represents a proxy that gets a property.</summary>
    /// <typeparam name="TInput">Input type.</typeparam>
    /// <typeparam name="TOutput">Output type.</typeparam>
    /// <param name="value">Input value.</param>
    /// <returns>Converted value.</returns>
    internal delegate TOutput ProxyConverter<in TInput, out TOutput>(TInput value);

    /// <summary>Represents a proxy that is a factory which takes a parameter.</summary>
    /// <typeparam name="TParam">Parameter type.</typeparam>
    /// <typeparam name="TOutput">Output type.</typeparam>
    /// <returns>New object of the output type.</returns>
    internal delegate TOutput ProxyFactory<in TParam, out TOutput>(TParam parameter);
}