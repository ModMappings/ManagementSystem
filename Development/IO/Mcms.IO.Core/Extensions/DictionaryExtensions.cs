using System;
using System.Collections.Generic;

namespace Mcms.IO.Core.Extensions
{
    public static class DictionaryExtensions
    {

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> producer)
        {
            if (dictionary.ContainsKey(key))
                return dictionary[key];

            var value = producer();
            dictionary.Add(key, value);
            return value;
        }
    }
}
