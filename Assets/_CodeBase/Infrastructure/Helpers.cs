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

    public static Vector3 GetRandomPositionInCollider(Collider collider, float positionY = 0)
    {
      float positionX = Random.Range(collider.bounds.min.x, collider.bounds.max.x);
      float positionZ = Random.Range(collider.bounds.min.z, collider.bounds.max.z);
      Vector3 targetPosition = new Vector3(positionX, positionY, positionZ);

      return targetPosition;
    }

    public static Vector3 ClampPositionByCollider(Collider collider, Vector3 position)
    {
      Vector3 result = position;
      
      var boundsMin = collider.bounds.min;
      var boundsMax = collider.bounds.max;
      
      result.x = Mathf.Clamp(position.x, boundsMin.x, boundsMax.x);
      result.z = Mathf.Clamp(position.z, boundsMin.z, boundsMax.z);

      return result;
    }
    
    public static bool CompareLayers(LayerMask layerMask1, LayerMask layerMask2) => 
      layerMask2 == (layerMask2 | (1 << layerMask1));
    
    public static Vector3 MultiplyVectors(Vector3 vector1, Vector3 vector2) => 
      new Vector3(vector1.x * vector2.x, vector1.y * vector2.y, vector1.z * vector2.z);
		
    public static Vector3 DivideVectors(Vector3 vector1, Vector3 vector2) => 
      new Vector3(vector1.x / vector2.x, vector1.y / vector2.y, vector1.z / vector2.z);
  }
}