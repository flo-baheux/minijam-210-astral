using Laywelin;
using TMPro;
using UnityEngine;

public class SanityUI: MonoBehaviour {
    [SerializeField] private TextMeshProUGUI sanityText;
    
    private void Start() {
      sanityText.text = GlobalGameManager.Instance.PlayerSanity.currentSanity.ToString();
      GlobalGameManager.Instance.PlayerSanity.OnSanityReduced += OnSanityReducedHandler;
    }

    private void OnSanityReducedHandler(int newValue) {
      sanityText.text = newValue.ToString();
    }
}