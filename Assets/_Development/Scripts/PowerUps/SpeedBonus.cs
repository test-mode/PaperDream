using UnityEngine;

namespace PaperDream
{
    public class SpeedBonus : MonoBehaviour
    {
        [Header("Pick-Up Attributes")]
        [SerializeField] private float _rotationSpeed = 0.5f;
        [SerializeField] private float _speedFactor = 1;
        [SerializeField] private float _lenght = 1;

        private bool _afterburner = false;

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
                _afterburner = true;
                EventManager.OnAfterBurnerToggle(_afterburner);
                GameObject.Destroy(gameObject);
            }
        }
    }
}
