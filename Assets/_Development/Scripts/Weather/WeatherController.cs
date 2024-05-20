using System.Collections;
using UnityEngine;

namespace PaperDream
{
    public class WeatherController : MonoBehaviour
    {
        [SerializeField] private RainScript RainPrefab;
        [SerializeField] private CloudBarrier CloudPrefab;
        [SerializeField] private GameObject Sun;

        [SerializeField] private bool _isRaining = false;
        [SerializeField] private bool _hasBeenRaining = true;

        private RainScript _currentRain;
        private CloudBarrier _currentCloudBarrier;

        private void Start()
        {
            Sun.transform.rotation = Quaternion.Euler(Random.Range(0f, 180f), 0.0f, 0.0f);

            if (_hasBeenRaining)
            {
                StartCoroutine(StartRain());
            }
            else
            {
                StartCoroutine(RainRoutine());
            }
        }

        private void Update()
        {
            if (_currentCloudBarrier == null && CloudPrefab.Altitude > CloudPrefab.MinAltitude)
            {
                _currentCloudBarrier = Instantiate(CloudPrefab);
            }
        }

        private void LateUpdate()
        {
            if (_currentCloudBarrier != null && _currentCloudBarrier.Altitude < CloudPrefab.MinAltitude)
            {
                Destroy(_currentCloudBarrier.gameObject);
            }
        }

        public IEnumerator StartRain()
        {
            float waitTime = 8.0f;
            yield return StartCoroutine(Countdown(waitTime));
            _currentRain = Instantiate(RainPrefab);
            _isRaining = true;
            _currentRain.RainIntensity = Random.Range(0.1f, 1f);
        }

        private IEnumerator RainRoutine()
        {
            while (true)
            {
                if (!_hasBeenRaining)
                {
                    float waitTime = Random.Range(5f, 60f);
                    yield return StartCoroutine(Countdown(waitTime));
                    StartRain();
                }

                float duration = Random.Range(20f, 60f);
                yield return StartCoroutine(Countdown(duration));

                while (_currentRain.RainIntensity > 0)
                {
                    _currentRain.RainIntensity -= Time.deltaTime * 0.1f;
                    yield return null;
                }

                _isRaining = false;
                Destroy(_currentRain.gameObject);
                _hasBeenRaining = false;
            }
        }

        private IEnumerator Countdown(float duration)
        {
            float totalTime = duration;
            while (totalTime > 0)
            {
                Debug.Log("Rain toggle... " + totalTime);
                totalTime -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
