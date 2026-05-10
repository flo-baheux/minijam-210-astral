using UnityEngine;

namespace Laywelin {
  [CreateAssetMenu(fileName = "InventoryItemSO", menuName = "Laywelin/SO/InventoryItem")]
  public class InventoryItemSO: ScriptableObject {
    [SerializeField] public string itemName;
    [SerializeField] public Sprite itemIcon;
  }
}