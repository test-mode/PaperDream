using UnityEngine;

namespace PaperDream
{
    public class PlaneController : MonoBehaviour
    {
        [SerializeField] private Transform _cameraTarget;
        [SerializeField] private FixedJoystick _joystick;

        [SerializeField, Range(0, 1)] private float _cameraSpring = 0.96f;
        [SerializeField] private float _minThrust = 600f;
        [SerializeField] private float _maxThrust = 1200f;
        [SerializeField] private float _thrustIncreaseSpeed = 400f;
        [SerializeField] private float _pitchIncreaseSpeed = 300f;
        [SerializeField] private float _rollIncreaseSpeed = 300f;

        private Rigidbody _rigidbody;
        private Camera _camera;

        private float _currentThrust;
        private float _deltaPitch;
        private float _deltaRoll;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;
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

        private void FixedUpdate()
        {
            Quaternion localRotation = transform.localRotation;
            localRotation *= Quaternion.Euler(0f, 0f, _deltaRoll);
            localRotation *= Quaternion.Euler(_deltaPitch, 0f, 0f);
            transform.localRotation = localRotation;
            _rigidbody.velocity = transform.forward * (_currentThrust * Time.fixedDeltaTime);

            Vector3 cameraTargetPosition = transform.position + (transform.forward * -8f) + new Vector3(0f, 3f, 0f);
            Transform cameraTransform = _camera.transform;

            cameraTransform.position = (cameraTransform.position * _cameraSpring) + (cameraTargetPosition * (1 - _cameraSpring));
            _camera.transform.LookAt(_cameraTarget);

            Vector3 cameraRotation = _camera.transform.rotation.eulerAngles;
            Quaternion targetRotation = Quaternion.Euler(cameraRotation.x, cameraRotation.y, localRotation.eulerAngles.z);
            _camera.transform.rotation = targetRotation;
        }
    }
}
