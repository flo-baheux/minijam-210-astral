#nullable enable
using System;
using System.IO;
using UnityEngine;

namespace Laywelin {
  [Serializable]
  public class SaveData {
    public int saveId;
    public bool tutorialCompleted;
  }

  public class SaveManager : MonoBehaviour {
    private const float saveTimer = 360 * 1;
    private float currentSaveTimer;
    private bool saving, loading, autosaveEnabled;
    private int autosaveSaveId;

    public static string SaveFilePath(int saveId) {
      return $"{Application.persistentDataPath}/save{saveId}.data";
    }

    public SaveData? Load(int saveId) {
      if (saving || loading)
        return null;

      loading = true;

      SaveData? saveData = null;

      try {
        var filePath = SaveFilePath(saveId);

        if (File.Exists(filePath)) {
          var stringSaveData = File.ReadAllText(SaveFilePath(saveId));
          saveData = JsonUtility.FromJson<SaveData>(stringSaveData);

          if (saveData.tutorialCompleted == false)
            saveData = null;
        } else {
          Debug.LogWarning($"No save found at path {filePath}");
        }
      } catch
        (Exception e) {
        Debug.LogError("Failed to load save data. Error: " + e.Message);
        saveData = null;
      }

      loading = false;
      return saveData;
    }

    public void Save(int saveId, Action? callback = null) {
      if (saving || loading)
        return;

      saving = true;
      currentSaveTimer = 0;
      SaveData saveData = new();

      // serialize data

      try {
        File.WriteAllText(SaveFilePath(saveId), JsonUtility.ToJson(saveData));
        Debug.Log("Data saved");
      } catch (Exception e) {
        Debug.LogError("Failed to save data. Error: " + e.Message);
      }

      saving = false;
      callback?.Invoke();
    }

    public void EnableAutoSave(int saveId) {
      autosaveEnabled = true;
      autosaveSaveId = saveId;
    }

    private void Update() {
      if (autosaveEnabled == false)
        return;

      currentSaveTimer += Time.deltaTime;

      if (currentSaveTimer >= saveTimer) {
        Save(autosaveSaveId);
        currentSaveTimer = 0;
      }
    }

    public float GetLastTimeSavedInSeconds() {
      return currentSaveTimer;
    }

    public static void DeleteSave(int saveId) {
      File.Delete(SaveFilePath(saveId));
    }

    public static void OpenSaveFolder() {
      Application.OpenURL("file:///" + Path.GetDirectoryName(Application.persistentDataPath));
    }
  }
}