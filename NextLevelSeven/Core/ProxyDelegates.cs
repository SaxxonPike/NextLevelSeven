using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextLevelSeven.Core
{
    /// <summary>
    ///     Represents a proxy that gets a property using an index.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <returns>Property value.</returns>
    internal delegate TValue IndexedProxyGetter<out TValue>(int index);

    /// <summary>
    ///     Represents a proxy that gets a property using an index.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <returns>Property value.</returns>
    internal delegate TValue IndexedProxyGetter<in TKey, out TValue>(TKey index);

    /// <summary>
    ///     Represents a proxy that sets a property using an index.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="value">New value.</param>
    internal delegate void IndexedProxySetter<in TValue>(int index, TValue value);

    /// <summary>
    ///     Represents a proxy that sets a property using an index.
    /// </summary>
    /// <typeparam name="TKey">Type of key.</typeparam>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="value">New value.</param>
    internal delegate void IndexedProxySetter<in TKey, in TValue>(TKey index, TValue value);

    /// <summary>
    ///     Represents a proxy that is triggered when a value attempts to change.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="oldValue">Old value.</param>
    /// <param name="newValue">New value.</param>
    internal delegate void ProxyChangePendingNotifier<in TValue>(TValue oldValue, TValue newValue);

    /// <summary>
    ///     Represents a proxy that gets a property.
    /// </summary>
    /// <typeparam name="TInput">Input type.</typeparam>
    /// <typeparam name="TOutput">Output type.</typeparam>
    /// <param name="value">Input value.</param>
    /// <returns>Converted value.</returns>
    internal delegate TOutput ProxyConverter<in TInput, out TOutput>(TInput value);

    /// <summary>
    ///     Represents a proxy that gets a property.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <returns>Property value.</returns>
    internal delegate TValue ProxyGetter<out TValue>();

    /// <summary>
    ///     Represents a proxy that sets a property.
    /// </summary>
    /// <typeparam name="TValue">Type of value.</typeparam>
    /// <param name="value">New value.</param>
    internal delegate void ProxySetter<in TValue>(TValue value);
}
