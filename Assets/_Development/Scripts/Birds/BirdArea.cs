using UnityEngine;

namespace PaperDream
{
    public class BirdArea : MonoBehaviour
    {
        [SerializeField] private BirdController[] _birds;
        [SerializeField] private Collider _region;

        private Vector3 GetRandomPoint(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }

        private void Start()
        {
            InvokeRepeating(nameof(SetDestinations), 0, 5);
        }

        private void SetDestinations()
        {
            Bounds bounds = _region.bounds;
            foreach (BirdController bird in _birds)
            {
                bird.SetDestination(GetRandomPoint(bounds));
            }
        }
    }
}
