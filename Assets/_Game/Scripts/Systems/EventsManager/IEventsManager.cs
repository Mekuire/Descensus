using LightDI;
using UnityEngine.Events;

namespace HeroicEngine.Systems.Events
{
    /// <summary>
    /// this interface declares functions that object is must to create
    /// also its needed to limit functionality for 
    /// for example class that has reference to IEventsManager
    /// (not to main EventsManager object) cant accidentally destroy this object (to protect the game from crashes)
    /// Its good practice to have interfaces for most of your game services
    /// </summary>
    public interface IEventsManager : ISystem
    {
        void RegisterListener<T>(string eventType, UnityAction<T> listener);
        void RegisterListener<T1, T2>(string eventType, UnityAction<T1, T2> listener);
        void UnregisterListener<T>(string eventType, UnityAction<T> listener);
        void UnregisterListener<T1, T2>(string eventType, UnityAction<T1, T2> listener);
        void TriggerEvent<T>(string eventType, T value);
        void TriggerEvent<T1, T2>(string eventType, T1 value1, T2 value2);
        void RegisterListener(string eventType, UnityAction listener);
        void UnregisterListener(string eventType, UnityAction listener);
        void TriggerEvent(string eventType);
    }
}