using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils {
public static class RandomUtils {
    public static T nextEnum<T>() where T : Enum {
        var values = Enum.GetValues(typeof(T));
        return (T) values.GetValue(Random.Range(0, values.Length));
    }

    public static List<T> generateEnums<T>(int totalAmount, params float[] proportions) where T : Enum {
        var values = Enum.GetValues(typeof(T));
        var list = new List<T>();
        for (var i = 0; i < proportions.Length && list.Count < totalAmount; i++) {
            var amount = Mathf.RoundToInt(proportions[i] * totalAmount);
            if (totalAmount > 0 && amount == 0) amount = 1;
            for (var j = 0; j < amount; j++) {
                list.Add((T) values.GetValue(i));
                if (list.Count == totalAmount) break;
            }
        }
        if (list.Count < totalAmount) {
            var remainder = totalAmount - list.Count;
            for (var i = 0; i < remainder; i++) {
                list.Add(nextEnum<T>());
            }
        }
        return list;
    }

    public static T nextItem<T>(IList<T> list) => list[Random.Range(0, list.Count)];

    public static bool nextBool() => Random.value > 0.5f;

    public static float nextFloat(float min = 0f, float max = 1f) => min + Random.value * (max - min);

    /// <summary>
    /// return a random number within the specified range, the number will be the opposite sign with 50% chance.
    /// </summary>
    public static float nextFloatWithRandomSign(float min = 0f, float max = 1f) {
        return nextBool() ? nextFloat(min, max) : -nextFloat(min, max);
    }

    public static float nextSign(float value = 1f) => nextBool() ? value : -value;

    public static int nextInt(int minInclusive, int maxExclusive) => Random.Range(minInclusive, maxExclusive);

    public static List<int> nonRepeatingList(int count, int minInclusive, int maxExclusive) {
        if (maxExclusive <= minInclusive || count < 0 || 
            (count > maxExclusive - minInclusive && maxExclusive - minInclusive > 0)) {
            throw new ArgumentException($"incorrect count ({count}) or range ({minInclusive}, {maxExclusive})");
        }
        var list = new List<int>();
        for (var i = 0; i < count; i++) {
            int num;
            do {
                num = Random.Range(minInclusive, maxExclusive);
            } while (list.Contains(num));
            list.Add(num);
        }
        return list;
    }

    public static List<int> randomList(int count, int minInclusive, int maxExclusive) {
        if (maxExclusive <= minInclusive || count < 0) {
            throw new ArgumentException($"incorrect count ({count}) or range ({minInclusive}, {maxExclusive})");
        }
        var list = new List<int>();
        for (var i = 0; i < count; i++) {
            list.Add(nextInt(minInclusive, maxExclusive));
        }
        return list;
    }
}
}