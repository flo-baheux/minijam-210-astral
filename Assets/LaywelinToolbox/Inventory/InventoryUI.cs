using System.Collections.Generic;
using Laywelin;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI: MonoBehaviour {
    [SerializeField] private InventoryItemUI itemUIPrefab;
    [SerializeField] private Transform itemContainer;
    private Dictionary<InventoryItemSO, InventoryItemUI> itemUIBySO = new();
    
    private void Start() {
      GlobalGameManager.Instance.PlayerInventory.OnAddItem += OnAddItemHandler;
      GlobalGameManager.Instance.PlayerInventory.OnRemoveItem += OnRemoveItemHandler;
    }

    private void OnAddItemHandler(InventoryItemSO item) {
      var itemUI = Instantiate(itemUIPrefab, itemContainer);
      itemUI.Init(item);
      itemUIBySO.Add(item, itemUI);
    }

    private void OnRemoveItemHandler(InventoryItemSO item) { 
      if (!itemUIBySO.Remove(item, out var itemUI))
        return;

      Destroy(itemUI.gameObject);
    }
}