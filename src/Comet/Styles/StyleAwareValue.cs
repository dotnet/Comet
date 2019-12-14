using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Comet
{
	public class StyleAwareValue
    {
        protected readonly Dictionary<object, object> Data = new Dictionary<object, object>();

        public IEnumerable<(string key, object value)> ToEnvironmentValues()
            => Data.Select(x => (KeyStringValue(x.Key), x.Value));

        static readonly string defaultString = ControlState.Default.ToString();
        protected virtual string KeyStringValue(object key)
        {
            var s = key?.ToString();
            if (s == defaultString)
                return null;
            return s;
        }
    }

    public class StyleAwareValue<TEnum, TValue> : StyleAwareValue, IDictionary<TEnum, TValue>
    {
        public TValue this[TEnum key] {
            get => (TValue)Data[key];
            set => Data[key] = value;
		}

        public ICollection<TEnum> Keys => Data.Keys.OfType<TEnum>().ToList();

        public ICollection<TValue> Values => Data.Values.OfType<TValue>().ToList();

        public int Count => Data.Count;

        public bool IsReadOnly => false;

        public void Add(TEnum key, TValue value) => Data.Add(key, value);

        public void Add(KeyValuePair<TEnum, TValue> item) => Data.Add(item.Key,item.Value);

        public void Clear() => Data.Clear();

        public bool Contains(KeyValuePair<TEnum, TValue> item) => Data.Any(x => x.Key == (object)item.Key && x.Value == (object)item.Value);

        public bool ContainsKey(TEnum key) => Data.ContainsKey(key);

        public void CopyTo(KeyValuePair<TEnum, TValue>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<TEnum, TValue>> GetEnumerator() => Data.Select(x => new KeyValuePair<TEnum, TValue>((TEnum)x.Key, (TValue)x.Value)).GetEnumerator();

        public bool Remove(TEnum key) => Data.Remove(key);

        public bool Remove(KeyValuePair<TEnum, TValue> item) => Data.Remove(item.Key);

        public bool TryGetValue(TEnum key, out TValue value)
        {
            var found = Data.TryGetValue(key, out var item);
            value = (TValue)item;
            return found;
        }

        IEnumerator IEnumerable.GetEnumerator() => Data.GetEnumerator();

        public static implicit operator StyleAwareValue<TEnum,TValue>(TValue value)
        {
            var style = new StyleAwareValue<TEnum, TValue>();
            style.Data[ControlState.Default] = value;
            return style;
        }

    }
}
