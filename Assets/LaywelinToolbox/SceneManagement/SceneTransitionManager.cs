using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Laywelin {
  public static class SceneTransferData {
    public static int selectedSaveId = 1;
  }

  public class SceneTransitionManager : MonoBehaviour {
    [SerializeField] private UISceneTransition UISceneTransition;
    public static SceneTransitionManager Instance { get; private set; }

    [NonSerialized] public float sceneTransitionProgress = 0;
    
    private SceneBootstrapper currentLoadingSceneBootstrapper;
    private bool isTransitioning;
    
    private void Awake() {
      // == SINGLETON IMPLEMENTATION ==
      if (Instance != null && Instance != this)
        Destroy(gameObject);
      else
        Instance = this;
      DontDestroyOnLoad(gameObject);
      // == END OF SINGLETON CODE ==
    }

    public void TransitionToScene(string sceneName) {
      if (isTransitioning)
        return;

      UISceneTransition.ShowTransition(
        () => StartCoroutine(TransitionToSceneCoroutine(sceneName, () => UISceneTransition.HideTransition()))
        );
    }

    private IEnumerator TransitionToSceneCoroutine(string sceneName, Action callback = null) {
      isTransitioning = true;

      if (!Application.CanStreamedLevelBeLoaded(sceneName)) {
        Debug.LogError($"Scene '{sceneName}' not found.");
        isTransitioning = false;
        callback?.Invoke();
        yield break;
      }

      var loadSceneAsyncOp = SceneManager.LoadSceneAsync(sceneName);
      loadSceneAsyncOp.allowSceneActivation = false;

      while (!loadSceneAsyncOp.isDone) {
        sceneTransitionProgress = loadSceneAsyncOp.progress;

        if (loadSceneAsyncOp.progress >= 0.9f)
          loadSceneAsyncOp.allowSceneActivation = true;

        yield return null;
      }
      
      sceneTransitionProgress = Random.Range(0.91f, 0.99f);

      // Backup if scene bootstrapper is not already registered
      if (currentLoadingSceneBootstrapper == null) 
        yield return new WaitForSecondsRealtime(3);

      if (currentLoadingSceneBootstrapper == null)
        Debug.LogWarning("No scene bootstrapper found - skipped initialization phase.");
      
      yield return currentLoadingSceneBootstrapper?.Initialize();
        
      currentLoadingSceneBootstrapper = null;
      isTransitioning = false;
      callback?.Invoke();
    }

    public void RegisterSceneBootstrapper(SceneBootstrapper sceneBootstrapper) {
      currentLoadingSceneBootstrapper = sceneBootstrapper;
    }
  }
}