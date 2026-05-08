using System;
using System.Collections.Generic;

namespace Laywelin {
  public static class GameplayEventManager {
    private static readonly Dictionary<Type, Action<GameplayEvent>> eventListeners = new();
    
    public static void AddListener<T>(Action<T> listener) where T: GameplayEvent {
      Type type = typeof(T);
      
      Action<GameplayEvent> wrappedListener = (e) => listener((T)e);

      if (eventListeners.TryGetValue(type, out var existing))
        eventListeners[type] = existing + wrappedListener;
      else
        eventListeners[type] = wrappedListener;
    }

    public static void RemoveListener<T>(Action<T> listener) where T : GameplayEvent {
      Type type = typeof(T);

      if (!eventListeners.TryGetValue(type, out var existing))
        return;

      existing -= (e) => listener((T)e);

      if (existing == null)
        eventListeners.Remove(type);
      else
        eventListeners[type] = existing;
    }

    public static void Emit(GameplayEvent evt) {
      if (eventListeners.TryGetValue(evt.GetType(), out var listener))
        listener?.Invoke(evt);
    }

    public static void Clear() { 
      eventListeners.Clear();
    }
  }
}