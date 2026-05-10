using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Laywelin;
using TMPro;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
using Random = System.Random;

public class MovingShelf : MonoBehaviour {
  [SerializeField] private List<IngameBookInteractable> books = new();
  [SerializeField] private List<int> indexBooksRequired = new();

  private HashSet<int> indexBooksActive = new();

  private bool activated = false;
  
  private void Awake() {
    for (int i = 0; i < books.Count; i++) {
      int index = i;
      books[i].OnStateChange += (state) => OnStateChangeHandler(index, state);
    }
  }

  private void OnStateChangeHandler(int index, bool state) {
    if (activated)
      return;
    
    if (state)
      indexBooksActive.Add(index);
    else
      indexBooksActive.Remove(index);

    if (indexBooksActive.SetEquals(indexBooksRequired.ToHashSet())) {
      activated = true;
      transform.DOLocalMoveX(1.5f, 5).SetRelative(true);
    }

  }

}
