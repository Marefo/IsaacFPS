using System;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.Extensions;
using UnityEngine;

namespace _CodeBase.Points
{
	public abstract class PointsStorage<T> : MonoBehaviour where T : Point
	{
		public event Action FirstPointReleased;
		public event Action PointReleased;

		public List<T> Points { get; private set; }

		private void Awake() => Points = transform.GetComponentsInChildren<T>().ToList();

		private void OnEnable() => 
			Points.ForEach(point => point.Released += OnPointRelease);

		private void OnDisable() => 
			Points.ForEach(point => point.Released -= OnPointRelease);

		private void OnPointRelease(Point point)
		{
			PointReleased?.Invoke();
			if(Points.First() != point) return;
			FirstPointReleased?.Invoke();
		}

		public void AddPoint(T point) => Points.Add(point);
		
		public void OnPointPositionsChange() => Points.ForEach(point => point.OnPositionChange());

		public bool HasAvailable() => Points.Count(point => point.Available) > 0;

		public int GetAvailablePointsAmount() => Points.Count(point => point.Available); 
			
		public T GetPoint() => Points.FirstOrDefault(point => point.Available);
		
		public List<T> GetPoints(int amount) => Points.Where(point => point.Available).Take(3).ToList();
		
		public T TryToGetCloserPoint(T point)
		{
			T result = point;
			int closerPointIndex = Points.IndexOf(point) - 1;

			if (closerPointIndex == -1 || Points[closerPointIndex].Available == false)
				return result;

			return Points[closerPointIndex];
		}

		public T GetRandomPoint()
		{
			List<T> availablePoints = Points.Where(point => point.Available).ToList().Shuffle();
			return availablePoints.GetRandomValue();
		}

		public T GetClosestPoint(Vector3 to)
		{
			List<T> points = Points.Where(point => point.Available).ToList(); 
			T closestPoint = Points.First();
			float minDistance = float.MaxValue;

			foreach (T point in points)
			{
				float distance = Vector3.Distance(point.Position, to);

				if ((distance < minDistance) == false) continue;
				
				minDistance = distance;
				closestPoint = point;
			}

			return closestPoint;
		}

		public T GetFarPoint(Vector3 to)
		{
			List<T> points = Points.Where(point => point.Available).ToList(); 
			T farPoint = Points.First();
			float maxDistance = float.MinValue;

			foreach (T point in points)
			{
				float distance = Vector3.Distance(point.Position, to);

				if ((distance > maxDistance) == false) continue;
				
				maxDistance = distance;
				farPoint = point;
			}

			return farPoint;
		}
		
		public T GetFirstPoint() => Points.First();
	}
}