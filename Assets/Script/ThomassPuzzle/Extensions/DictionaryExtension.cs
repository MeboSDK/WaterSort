using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

namespace ThomassPuzzle.Extensions
{
    public static class DictionaryExtension
    {
        public static void SetForSpecialDic<TKey, TValue>(this Dictionary<TKey,TValue> keyValues, TKey selected, TValue target)
        {
            if (!keyValues.ContainsKey(selected))
                keyValues.Add(selected, target);
            else
                keyValues[selected] = target;
        }
        public static void RemoveForSpecialDic<TKey, TValue>(this Dictionary<TKey, TValue> keyValues, TKey selected)
        {
            keyValues.Remove(selected);
        }
        /// <returns>If doesn't exist, returns default of TValue</returns>
        public static TValue GetForSpecialDic<TKey, TValue>(this Dictionary<TKey, TValue> keyValues, TKey key)
        {
            if (keyValues.ContainsKey(key))
                return keyValues.GetValueOrDefault(key);
            else return default;
        }
    }
}
