using System.Collections.Generic;

public static class DictionaryExt {
    public static TValue Get<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key) {
        if (key != null) {
            TValue value;
            if (dict.TryGetValue(key, out value)) {
                return value;
            }
        }
        return default(TValue);
    }
}
