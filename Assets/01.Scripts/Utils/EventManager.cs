using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System;
using EnumTypes;
using UnityEditor;

namespace EventLibrary
{
    [Serializable]
    public class TransformEvent : UnityEvent<Transform> { }

    [Serializable]
    public class FloatEvent : UnityEvent<float> { }

    public class EventManager<TEnum>
    {
        private static Dictionary<TEnum, UnityEvent> eventDictionary = new Dictionary<TEnum, UnityEvent>();
        private static Dictionary<TEnum, TransformEvent> transformEventDictionary = new Dictionary<TEnum, TransformEvent>();
        private static Dictionary<TEnum, FloatEvent> floatEventDictionary = new Dictionary<TEnum, FloatEvent>();

        public static void StartListening(TEnum eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new UnityEvent();
                thisEvent.AddListener(listener);
                eventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StartListening(TEnum eventName, UnityAction<Transform> listener)
        {
            TransformEvent thisEvent = null;
            if (transformEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new TransformEvent();
                thisEvent.AddListener(listener);
                transformEventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StartListening(TEnum eventName, UnityAction<float> listener)
        {
            FloatEvent thisEvent = null;
            if (floatEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.AddListener(listener);
            }
            else
            {
                thisEvent = new FloatEvent();
                thisEvent.AddListener(listener);
                floatEventDictionary.Add(eventName, thisEvent);
            }
        }

        public static void StopListening(TEnum eventName, UnityAction listener)
        {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.RemoveListener(listener);
            }
        }

        public static void TriggerEvent(TEnum eventName)
        {
            UnityEvent thisEvent = null;
            if (eventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke();
            }
        }

        public static void TriggerEvent(TEnum eventName, Transform transform)
        {
            TransformEvent thisEvent = null;
            if (transformEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(transform);
            }
        }

        public static void TriggerEvent(TEnum eventName, float val)
        {
            FloatEvent thisEvent = null;
            if (floatEventDictionary.TryGetValue(eventName, out thisEvent))
            {
                thisEvent.Invoke(val);
            }
        }
    }
}