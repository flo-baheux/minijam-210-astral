using Laywelin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI: MonoBehaviour {
    [SerializeField] private TextMeshProUGUI name;
    [SerializeField] private Image icon;

    public void Init(InventoryItemSO item) {
      name.text = item.itemName;
      icon.sprite = item.itemIcon;
    }
  }