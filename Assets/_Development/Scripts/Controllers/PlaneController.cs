using System.Threading.Tasks;
using UnityEngine;

namespace PaperDream
{
    public class PlaneController : MonoBehaviour, IDamageable
    {
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private FixedJoystick _joystick;

        [SerializeField, Range(0, 1)] private float _cameraSpring = 0.96f;
        [SerializeField] private float _minThrust = 600f;
        [SerializeField] private float _maxThrust = 1200f;
        [SerializeField] private float _thrustIncreaseSpeed = 400f;
        [SerializeField] private float _pitchIncreaseSpeed = 300f;
        [SerializeField] private float _rollIncreaseSpeed = 300f;
        [SerializeField] private float _afterBurnerTimer = 3.0f;

        private Rigidbody _rigidbody;
        private Camera _camera;

        private float _currentThrust;
        private float _deltaPitch;
        private float _deltaRoll;

        private bool _accelerate;
        private bool _afterburner;

        private void OnEnable()
        {
            EventManager.AfterBurnerToggle += EventManagerAfterBurnerToggle;
        }

        private void OnDisable()
        {
            EventManager.AfterBurnerToggle -= EventManagerAfterBurnerToggle;
        }

        private async void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;
            _accelerate = false;
            _afterburner = false;

            await Task.Delay(6000);
            _accelerate = true;
        }

        private void Update()
        {
            float thrustDelta = 0f;
            _currentThrust += thrustDelta * Time.deltaTime;
            _currentThrust = Mathf.Clamp(_currentThrust, _minThrust, _maxThrust);

            _deltaPitch = 0f;
            _deltaPitch += _pitchIncreaseSpeed * _joystick.Vertical;
            _deltaPitch *= Time.deltaTime;

            _deltaRoll = 0f;
            _deltaRoll -= _rollIncreaseSpeed * _joystick.Horizontal;
            _deltaRoll *= Time.deltaTime;
        }

        private void LateUpdate()
        {
            if (_accelerate)
            {
                Quaternion localRotation = transform.localRotation;
                Vector3 cameraRotation = _camera.transform.rotation.eulerAngles;
                Quaternion targetRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, localRotation.eulerAngles.z);
                _camera.transform.rotation = targetRotation;
            }
        }

        private void FixedUpdate()
        {
            Quaternion localRotation = transform.localRotation;
            localRotation *= Quaternion.Euler(0f, 0f, _deltaRoll);
            localRotation *= Quaternion.Euler(_deltaPitch, 0f, 0f);
            transform.localRotation = localRotation;

            if (_accelerate)
            {
                _rigidbody.useGravity = true;
                _rigidbody.velocity = transform.forward * (_currentThrust * Time.fixedDeltaTime);

                Vector3 cameraTargetPosition = transform.position + (transform.forward * -8f) + new Vector3(0f, 3f, 0f);
                Transform cameraTransform = _camera.transform;

                cameraTransform.position = (cameraTransform.position * _cameraSpring) + (cameraTargetPosition * (1 - _cameraSpring));
                _camera.transform.LookAt(_cameraTarget);
            }
        }

        private void EventManagerAfterBurnerToggle(bool toggle)
        {
            _afterburner = toggle;
            _currentThrust = _maxThrust;
            _cameraSpring = 0.965f;
            StartCoroutine(ToggleOffAfterCertainTime(_afterBurnerTimer));

        }

        System.Collections.IEnumerator ToggleOffAfterCertainTime(float delay)
        {
            yield return new WaitForSeconds(delay);
            _afterburner = false;
            EventManager.OnAfterBurnerToggle(_afterburner);
            _currentThrust = _minThrust;
            _cameraSpring = 0.96f;
        }

        public void TakeDamage(float damage)
        {
            Debug.Log($"Plane took {damage} damage!");
        }
    }
}
