using System.Collections;
using UnityEngine;

namespace PaperDream
{
    public class WeatherController : MonoBehaviour
    {
        public RainScript RainPrefab;
        public GameObject Sun;
        public bool isRaining = false;
        public bool hasBeenRaining = true;

        private RainScript _currentRain;

        private void Start()
        {
            Sun.transform.rotation = Quaternion.Euler(Random.Range(0f, 180f), 0.0f, 0.0f);

            if (hasBeenRaining)
            {
                StartRain();
            }
            else
            {
                StartCoroutine(RainRoutine());
            }
        }

        public void StartRain()
        {
            _currentRain = Instantiate(RainPrefab);
            isRaining = true;
            _currentRain.RainIntensity = Random.Range(0.1f, 1f);
        }

        private IEnumerator RainRoutine()
        {
            while (true)
            {
                if (!hasBeenRaining)
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

                isRaining = false;
                Destroy(_currentRain.gameObject);
                hasBeenRaining = false; // After the first rain, subsequent rains will be random
            }
        }

        private IEnumerator Countdown(float duration)
        {
            float totalTime = duration;
            while (totalTime > 0)
            {
                Debug.Log("Time remaining: " + totalTime);
                totalTime -= Time.deltaTime;
                yield return null;
            }
        }
    }
}
