using System;
using System.Collections.Generic;
using System.Linq;
using _CodeBase.Etc;
using _CodeBase.IndicatorCode;
using _CodeBase.Infrastructure.Services;
using _CodeBase.Interfaces;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace _CodeBase.Units.Monsters
{
  public class Monster : MonoBehaviour, IDamageable
  {
    public event Action Initialized;
    public event Action<Monster> Dead;

    public bool IsDead { get; private set; }
    public TriggerListener RoomZone { get; private set; }
    
    [field: SerializeField] public bool IsBoss { get; private set; }
    [field: SerializeField] public Health Health { get; private set; }
    [field: SerializeField] public bool HasSpawnOffsetY { get; private set; }
    [field: ShowIf("HasSpawnOffsetY"), SerializeField] public float SpawnOffsetY { get; private set; }
    [Space(10)] 
    [SerializeField] private Material _damagedMaterial;
    [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshRenderers;
    [SerializeField] private List<MeshRenderer> _meshRenderers;

    protected MonsterMonitor _monsterMonitor { get; private set; }
    protected AudioService _audioService { get; private set; }
    private Tween _changeMaterialTween;
    private List<Material[]> _defaultSkinnedMeshMaterials;
    private List<Material[]> _defaultMeshMaterials;

    public void Initialize(TriggerListener roomZone, MonsterMonitor monsterMonitor, AudioService audioService)
    {
      _defaultSkinnedMeshMaterials = _skinnedMeshRenderers.Select(mesh => mesh.materials).ToList();
      _defaultMeshMaterials = _meshRenderers.Select(mesh => mesh.materials).ToList();
      
      RoomZone = roomZone;
      _monsterMonitor = monsterMonitor;
      _audioService = audioService;
      Initialized?.Invoke();
    }

    public virtual void ReceiveDamage(int damageValue, Vector3 position)
    {
      Health.Decrease(damageValue);
      _changeMaterialTween?.Kill();
      SetMaterial(_damagedMaterial);
      _changeMaterialTween = DOVirtual.DelayedCall(0.125f, ResetMaterial).SetLink(gameObject);
    }

    private void SetMaterial(Material material)
    {
      foreach (SkinnedMeshRenderer mesh in _skinnedMeshRenderers)
      {
        Material[] materials = mesh.materials;
        
        for (int i = 0; i < materials.Length; i++)
        {
          materials[i] = material;
        }

        mesh.materials = materials;
      }
      
      foreach (MeshRenderer mesh in _meshRenderers)
      {
        Material[] materials = mesh.materials;
        
        for (int i = 0; i < materials.Length; i++)
        {
          materials[i] = material;
        }

        mesh.materials = materials;
      }
    }

    private void ResetMaterial()
    {
      for (int i = 0; i < _skinnedMeshRenderers.Count; i++)
      {
        SkinnedMeshRenderer mesh = _skinnedMeshRenderers[i];
        mesh.materials = _defaultSkinnedMeshMaterials[i];
      }
      
      for (int i = 0; i < _meshRenderers.Count; i++)
      {
        MeshRenderer mesh = _meshRenderers[i];
        mesh.materials = _defaultMeshMaterials[i];
      }
    }

    protected void SubscribeEvents() => Health.ValueCameToZero += Die;
    protected void UnSubscribeEvents() => Health.ValueCameToZero -= Die;

    protected virtual void Die()
    {
      IsDead = true;
      Dead?.Invoke(this);
    }
  }
}