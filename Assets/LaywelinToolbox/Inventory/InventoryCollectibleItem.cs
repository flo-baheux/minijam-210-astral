using UnityEngine;

namespace Laywelin {
  public class InventoryCollectibleItem: Interactable {
    [SerializeField] private InventoryItemSO inventoryItem;

    public override void Interact() {
      base.Interact();
      GlobalGameManager.Instance.PlayerInventory.AddItem(inventoryItem);
      canInteract = false;
      Destroy(gameObject);
    }
  }
}