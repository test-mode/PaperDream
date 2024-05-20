using UnityEngine.Events;

namespace PaperDream
{
    public static class EventManager
    {
        public static event UnityAction TimerStart;
        public static event UnityAction TimerStop;

        public static event UnityAction<float> TimerUpdate;

        public static event UnityAction<bool> AfterBurnerToggle;

        public static event UnityAction<bool> CloudBarrierToggle;

        public static void OnTimerStart() => TimerStart?.Invoke();
        public static void OnTimerStop() => TimerStop?.Invoke();
        public static void OnTimerUpdate(float value) => TimerUpdate?.Invoke(value);

        public static void OnAfterBurnerToggle(bool value) => AfterBurnerToggle?.Invoke(value);

        public static void OnCloudBarrierToggle(bool value) => CloudBarrierToggle?.Invoke(value);

    }
}