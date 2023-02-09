using System;
using System.Collections.Generic;
using _CodeBase.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace _CodeBase.Infrastructure.Services
{
	public class NavMeshService : MonoBehaviour
	{
		public event Action Initialized;
		
		public bool IsInitialized => !_firstBake;
		
		[SerializeField] private Vector3 NavMeshSize;
		[Space(10)]
		[SerializeField] private NavMeshSurface _navMeshSurface;

		private NavMeshData NavMeshData;
		private bool _firstBake = true;

		[Button()]
		public void ReBake() => BuildNavMesh();

		private void BuildNavMesh()
		{
			if (_firstBake)
			{
				NavMeshData = new NavMeshData();
				NavMesh.AddNavMeshData(NavMeshData);
			}
			
			List<NavMeshBuildMarkup> markups = new List<NavMeshBuildMarkup>();
			List<NavMeshBuildSource> Sources = new List<NavMeshBuildSource>();
			Bounds navMeshBounds = new Bounds(transform.position, NavMeshSize);

			
			NavMeshBuilder.CollectSources(navMeshBounds, _navMeshSurface.layerMask, _navMeshSurface.useGeometry, _navMeshSurface.defaultArea, markups, Sources);

			List<NavMeshModifierVolume> modifiers = NavMeshModifierVolume.activeModifiers;
			
			for (int i = 0; i < modifiers.Count; i++)
			{
				NavMeshModifierVolume currentModifier = modifiers[i];

				if (currentModifier.gameObject.CompareLayers(_navMeshSurface.layerMask) &&
				    currentModifier.AffectsAgentType(_navMeshSurface.agentTypeID))
				{
					Vector3 modifierCenter = currentModifier.transform.TransformPoint(currentModifier.center);
					Vector3 scale = currentModifier.transform.lossyScale;
					Vector3 modifierSize = new Vector3(currentModifier.size.x * Mathf.Abs(scale.x), currentModifier.size.y * Mathf.Abs(scale.y), currentModifier.size.z * Mathf.Abs(scale.z));
 
					NavMeshBuildSource source = new NavMeshBuildSource();
					source.shape = NavMeshBuildSourceShape.ModifierBox;
					source.transform = Matrix4x4.TRS(modifierCenter, currentModifier.transform.rotation, Vector3.one);
					source.size = modifierSize;
					source.area = currentModifier.area;

					Sources.Add(source);
				}
			}
			
			if (_firstBake)
			{
				NavMeshBuilder.UpdateNavMeshData(NavMeshData, _navMeshSurface.GetBuildSettings(), Sources, navMeshBounds);
				_firstBake = false;
				Initialized?.Invoke();
			}
			else
				NavMeshBuilder.UpdateNavMeshDataAsync(NavMeshData, _navMeshSurface.GetBuildSettings(), Sources, navMeshBounds);
		}
	}
}