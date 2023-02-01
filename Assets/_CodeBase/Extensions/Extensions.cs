using System.Collections.Generic;
using UnityEngine;

namespace _CodeBase.Extensions
{
  public static class Extensions
  {
    public static bool AddIfNotExists<T>(this List<T> list, T value)
    {
      if (list.Contains(value)) return false;
			
      list.Add(value);
      return true;
    }
  }
}