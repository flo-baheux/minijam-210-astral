using System;
using System.Collections.Generic;
using UnityEngine;

namespace Laywelin {
  public class PlayerInventory {
    private readonly HashSet<InventoryItemSO> items = new();
    public Action<InventoryItemSO> OnAddItem, OnRemoveItem;
    
    public void AddItem(InventoryItemSO item) {
      if (items.Add(item))
        OnAddItem?.Invoke(item);
      else
        Debug.LogError($"Cannot add item {item.name} - inventory already contains one");
    }

    public bool ContainsItem(InventoryItemSO item) {
      return items.Contains(item);
    }

    public void RemoveItem(InventoryItemSO item) {
      if (!ContainsItem(item)) {
        Debug.LogError($"Cannot remove item {item.name} - not in inventory");
        return;
      }

      items.Remove(item);
      OnRemoveItem?.Invoke(item);
    }
  }
}