using UnityEngine;

namespace PaperDream
{
    public class OpeningCinematic : MonoBehaviour
    {
        [SerializeField] private ParticleSystem[] _particleSystems;
        [SerializeField] private WeatherController _weatherController;
        [SerializeField] private float _time;

        public Transform target;
        public float rotationSpeed = 5f;

        private void Start()
        {
            StartCoroutine(LookAtTargetCoroutine());
        }

        private System.Collections.IEnumerator LookAtTargetCoroutine()
        {
            while (_time > 0f)
            {
                if (target != null)
                {
                    Vector3 direction = target.position - Camera.main.transform.position;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    Camera.main.transform.rotation = Quaternion.Slerp(Camera.main.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    _time -= Time.deltaTime;
                }

                yield return null;
            }
            _weatherController.StartRain();
        }
    }
}
