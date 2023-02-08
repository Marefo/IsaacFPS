using UnityEngine;

namespace _CodeBase.Infrastructure
{
  public static class Helpers
  {
    public static Vector3 RandomPointOnCircleEdge(Vector3 targetPoint, float radius)
    {
      var vector2 = Random.insideUnitCircle.normalized * radius;
      return targetPoint + new Vector3(vector2.x, 0, vector2.y);
    }
    
    public static bool CompareLayers(LayerMask layerMask1, LayerMask layerMask2) => 
      layerMask2 == (layerMask2 | (1 << layerMask1));
    
    public static Vector3 MultiplyVectors(Vector3 vector1, Vector3 vector2) => 
      new Vector3(vector1.x * vector2.x, vector1.y * vector2.y, vector1.z * vector2.z);
		
    public static Vector3 DivideVectors(Vector3 vector1, Vector3 vector2) => 
      new Vector3(vector1.x / vector2.x, vector1.y / vector2.y, vector1.z / vector2.z);
  }
}