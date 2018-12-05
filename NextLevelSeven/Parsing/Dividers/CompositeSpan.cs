using System;
using System.Collections.Generic;

namespace NextLevelSeven.Parsing.Dividers
{
    public static class SpanCombiner
    {
        public static ReadOnlySpan<T> Combine<T>(ReadOnlySpan<T> span0, ReadOnlySpan<T> span1)
        {
            var length0 = span0.Length;
            var length1 = span1.Length;
            var result = new T[length0 + length1].AsSpan();
            span0.CopyTo(result);
            span1.CopyTo(result.Slice(span0.Length));
            return result;
        }
        
        public static ReadOnlySpan<T> Combine<T>(ReadOnlySpan<T> span0, ReadOnlySpan<T> span1, ReadOnlySpan<T> span2)
        {
            var length0 = span0.Length;
            var length1 = span1.Length;
            var length2 = span2.Length;
            var result = new T[length0 + length1 + length2].AsSpan();
            span0.CopyTo(result);
            span1.CopyTo(result.Slice(length0, span1.Length));
            span2.CopyTo(result.Slice(length0 + length1));
            return result;
        }
    }
}