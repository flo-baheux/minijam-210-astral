#nullable enable
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace Laywelin {
  public static class LayRandom {
    public static T PickRandomWeighted<T>(this IList<T> list, Func<T, int> weightSelector) {
      var totalWeight = 0;
      foreach (var item in list)
        totalWeight += weightSelector(item);

      var randomValue = Random.Range(0, totalWeight);

      foreach (var item in list) {
        randomValue -= weightSelector(item);
        if (randomValue < 0)
          return item;
      }

      return list[^1];
    }

    public static T PickRandom<T>(this IList<T> list) {
      return list[Random.Range(0, list.Count)];
    }

    public static void Shuffle<T>(this IList<T> list) {
      var rng = new System.Random();
      var n = list.Count;
      for (var i = n - 1; i > 0; i--) {
        var j = rng.Next(i + 1);
        (list[i], list[j]) = (list[j], list[i]);
      }
    }
  }
}