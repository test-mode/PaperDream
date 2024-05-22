using UnityEngine;

namespace PaperDream
{
    public class TimeBonus : MonoBehaviour
    {
        [Header("Bonus Attibutes")]
        [SerializeField] private float _secondsToAdd = 0.0f;

        [Header("Pick-Up Attributes")]
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private float _speedFactor = 1;
        [SerializeField] private float _lenght = 1;

        private float _originalYPosition;

        private void Start()
        {
            _originalYPosition = transform.position.y;
        }

        private void Update()
        {
            this.RotateAndMove(_originalYPosition, _rotationSpeed, _speedFactor, _lenght);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                EventManager.OnTimerUpdate(_secondsToAdd);
                GameObject.Destroy(gameObject);
            }
        }
    }
}