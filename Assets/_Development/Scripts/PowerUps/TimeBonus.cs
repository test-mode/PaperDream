using UnityEngine;

public class TimeBonus : MonoBehaviour
{
    [SerializeField] private float _secondsToAdd = 0.0f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.OnTimerUpdate(_secondsToAdd);
            GameObject.Destroy(gameObject);
        }
    }
}
