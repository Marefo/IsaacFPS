using _CodeBase.Infrastructure.Services;
using _CodeBase.ItemsDrop.Data;
using _CodeBase.PickableItems;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.ItemsDrop
{
  public class ItemsDropper : MonoBehaviour
  {
    [SerializeField] private Transform _spawnPoint;
    [Space(10)]
    [SerializeField] private DropsData _dropsData;

    private AudioService _audioService;
    
    public void Initialize(AudioService audioService) => _audioService = audioService;

    public void TryDropItem(Vector3 direction)
    {
      DropItem dropItemPrefab = _dropsData.GetRandomDrop();

      if (dropItemPrefab != null)
        Drop(dropItemPrefab, direction);
    }

    private void Drop(DropItem dropItemPrefab, Vector3 direction)
    {
      DropItem dropItem = Instantiate(dropItemPrefab, _spawnPoint.position, dropItemPrefab.transform.rotation);
      
      if(dropItem.TryGetComponent(out PickableItem pickableItem))
        pickableItem.Initialize(_audioService);
        
      dropItem.Push(direction);
    }
  }
}