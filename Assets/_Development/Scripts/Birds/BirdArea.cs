using UnityEngine;

namespace PaperDream
{
    public class BirdArea : MonoBehaviour
    {
        [SerializeField] private Collider _region;
        [SerializeField] private Bird _birdPrefab;
        [SerializeField] private int _birdCount = 10;

        private Bird[] _birds;

        private Vector3 GetRandomPoint(Bounds bounds)
        {
            Vector3 point = new()
            {
                x = Random.Range(bounds.min.x, bounds.max.x),
                y = Random.Range(bounds.min.y, bounds.max.y),
                z = Random.Range(bounds.min.z, bounds.max.z)
            };

            return point;
        }

        private void Start()
        {
            _birds = new Bird[_birdCount];
            for (int i = 0; i < _birdCount; i++)
            {
                _birds[i] = Instantiate(_birdPrefab, GetRandomPoint(_region.bounds), Quaternion.identity, transform);
            }
            InvokeRepeating(nameof(SetDestinations), 0, 5);
        }

        private void SetDestinations()
        {
            Bounds bounds = _region.bounds;
            foreach (Bird bird in _birds)
            {
                bird.SetDestination(GetRandomPoint(bounds));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                foreach (Bird bird in _birds)
                {
                    CancelInvoke(nameof(SetDestinations));
                    bird.SetDestination(other.transform);
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out IDamageable damageable))
            {
                InvokeRepeating(nameof(SetDestinations), 0, 5);
            }
        }
    }
}
