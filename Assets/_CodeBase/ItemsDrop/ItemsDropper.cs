using _CodeBase.ItemsDrop.Data;
using UnityEngine;
using UnityEngine.Serialization;

namespace _CodeBase.ItemsDrop
{
  public class ItemsDropper : MonoBehaviour
  {
    [SerializeField] private Transform _spawnPoint;
    [Space(10)]
    [SerializeField] private DropsData _dropsData;

    public void TryDropItem(Vector3 direction)
    {
      DropItem dropItemPrefab = _dropsData.GetRandomDrop();

      if (dropItemPrefab != null)
        Drop(dropItemPrefab, direction);
    }

    private void Drop(DropItem dropItemPrefab, Vector3 direction)
    {
      DropItem dropItem = Instantiate(dropItemPrefab, _spawnPoint.position, dropItemPrefab.transform.rotation);
      dropItem.Push(direction);
    }
  }
}