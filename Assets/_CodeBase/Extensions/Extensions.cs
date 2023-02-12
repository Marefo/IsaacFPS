using System.Collections.Generic;
using _CodeBase.Data;
using UnityEngine;
using UnityEngine.AI;
using SystemRandom = System.Random;

namespace _CodeBase.Extensions
{
  public static class Extensions
  {
    private static readonly SystemRandom rng = new SystemRandom(); 
    
    public static bool CompareLayers(this GameObject obj, LayerMask layerMask) => 
      layerMask == (layerMask | (1 << obj.layer));
    
    public static Transform SetLossyScale(this Transform transform, float? x = null, float? y = null, float? z = null)
    {
      var lossyScale = transform.lossyScale.Change3(x, y, z);

      transform.localScale = Vector3.one;
      transform.localScale = new Vector3(lossyScale.x / transform.lossyScale.x,
        lossyScale.y / transform.lossyScale.y,
        lossyScale.z / transform.lossyScale.z);

      return transform;
    }
    
    public static Vector3 Change3(this Vector3 vector, float? x = null, float? y = null, float? z = null)
    {
      if (x.HasValue) vector.x = x.Value;
      if (y.HasValue) vector.y = y.Value;
      if (z.HasValue) vector.z = z.Value;
      return vector;
    }
    
    public static int GetSignForInterpolation(this float currentValue, float targetValue) => 
      targetValue > currentValue ? 1 : -1;
    
    public static Vector3 GetNavMeshSampledPosition(this Vector3 position)
    {
      NavMesh.SamplePosition(position, out NavMeshHit hit, float.MaxValue, NavMesh.AllAreas);
      return hit.position;
    }
    
    public static float GetRandomValue(this Range range) => Random.Range(range.Min, range.Max);
    
    public static bool AddIfNotExists<T>(this List<T> list, T value)
    {
      if (list.Contains(value)) return false;
			
      list.Add(value);
      return true;
    }
    
    public static T GetRandomValue<T>(this List<T> list) => list[Random.Range(0, list.Count)];
		
    public static List<T> Shuffle<T>(this List<T> list)
    {
      List<T> result = new List<T>(list);
      int n = result.Count;  
      while (n > 1) {  
        n--;  
        int k = rng.Next(n + 1);
        (result[k], result[n]) = (result[n], result[k]);
      }
      return result;
    }
    
    public static void EnableEmission(this ParticleSystem particles)
    {
      var particlesEmission = particles.emission;
      particlesEmission.enabled = true;
    }
		
    public static void DisableEmission(this ParticleSystem particles)
    {
      var particlesEmission = particles.emission;
      particlesEmission.enabled = false;
    }
  }
}