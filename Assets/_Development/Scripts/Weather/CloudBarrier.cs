using UnityEngine;

namespace PaperDream
{
    public class CloudBarrier : MonoBehaviour
    {
        [SerializeField] private GameObject Cloud;

        [SerializeField] private float _minAltitude = 60.0f;
        [SerializeField] private float _maxAltitude = 100.0f;
        [SerializeField] private float _minDensity = 0.0f;
        [SerializeField] private float _maxDensity = 2.0f;

        [SerializeField] private Vector3 _cloudOffset = new Vector3(0.0f, -10.0f, 50.0f);
        
        public float MinAltitude
        {
            get { return _minAltitude; }
            set { _minAltitude = value; }
        }

        public float Altitude
        {
            get { return Camera.main != null ? Camera.main.transform.position.y : 0; }
        }

        void SetCloudDensity()
        {
            float normalizedAltitude = Mathf.InverseLerp(MinAltitude, _maxAltitude, Altitude);

            float density = Mathf.Lerp(_maxDensity, _minDensity, normalizedAltitude);

            Renderer rendererCloudHigh = Cloud.GetComponent<Renderer>();

            Material materialCloudHigh = rendererCloudHigh.material;

            materialCloudHigh.SetFloat("_Density", density);
        }

        void Update()
        {
            transform.position = Camera.main.transform.position + _cloudOffset;

            SetCloudDensity();
        }

    }
}
