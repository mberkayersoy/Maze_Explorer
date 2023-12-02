using System;
using System.Collections.Generic;

public static class EventBus
{
    private static Dictionary<Type, List<Action<object>>> eventListeners = new Dictionary<Type, List<Action<object>>>();

    public static void Subscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (!eventListeners.ContainsKey(eventType))
        {
            eventListeners[eventType] = new List<Action<object>>();
        }

        Action<object> typeSafeListener = (param) => listener((T)param);
        eventListeners[eventType].Add(typeSafeListener);
    }

    public static void Unsubscribe<T>(Action<T> listener)
    {
        Type eventType = typeof(T);

        if (eventListeners.ContainsKey(eventType))
        {
            Action<object> typeSafeListener = (param) => listener((T)param);
            eventListeners[eventType].Remove(typeSafeListener);

            if (eventListeners[eventType].Count == 0)
            {
                eventListeners.Remove(eventType);
            }
        }
    }

    public static void Publish<T>(T eventData)
    {
        Type eventType = typeof(T);

        if (eventListeners.ContainsKey(eventType))
        {
            foreach (var listener in eventListeners[eventType])
            {
                listener.Invoke(eventData);
            }
        }
    }
}
