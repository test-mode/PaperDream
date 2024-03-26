using UnityEngine;

namespace PaperDream
{
    public class PlaneController : MonoBehaviour
    {
        [SerializeField] private Transform CameraTarget;
        [SerializeField] private VariableJoystick joystick;

        [SerializeField, Range(0, 1)] private float CameraSpring = 0.96f;
        [SerializeField] private float MinThrust = 600f;
        [SerializeField] private float MaxThrust = 1200f;
        [SerializeField] private float ThrustIncreaseSpeed = 400f;
        [SerializeField] private float PitchIncreaseSpeed = 300f;
        [SerializeField] private float RollIncreaseSpeed = 300f;

        private Rigidbody _rigidbody;
        private Camera _camera;

        private float _currentThrust;
        private float _deltaPitch;
        private float _deltaRoll;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _camera = Camera.main;

            _camera.transform.SetParent(null);
        }

        private void Update()
        {
            float thrustDelta = 0f;
            if (Input.GetKey(KeyCode.Space))
            {
                thrustDelta += ThrustIncreaseSpeed;
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                thrustDelta -= ThrustIncreaseSpeed;
            }

            _currentThrust += thrustDelta * Time.deltaTime;
            _currentThrust = Mathf.Clamp(_currentThrust, MinThrust, MaxThrust);

            _deltaPitch = 0f;
            if (Input.GetKey(KeyCode.S))
            {
                _deltaPitch -= PitchIncreaseSpeed;
            }

            if (Input.GetKey(KeyCode.W))
            {
                _deltaPitch += PitchIncreaseSpeed;
            }

            _deltaPitch *= Time.deltaTime;

            _deltaRoll = 0f;
            if (Input.GetKey(KeyCode.A))
            {
                _deltaRoll += RollIncreaseSpeed;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _deltaRoll -= RollIncreaseSpeed;
            }

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

            cameraTransform.position = (cameraTransform.position * CameraSpring) + (cameraTargetPosition * (1 - CameraSpring));
            _camera.transform.LookAt(CameraTarget);
        }
    }
}
