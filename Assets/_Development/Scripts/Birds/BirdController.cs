using UnityEngine;

namespace PaperDream
{
    public class BirdController : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        private bool _destinationSet = false;
        private Vector3 _destination;

        public void SetDestination(Vector3 destination)
        {
            // Set the destination of the bird
            _destination = destination;
            _destinationSet = true;
        }

        private void Update()
        {
            // If the destination is set, move the bird towards it
            if (_destinationSet)
            {
                transform.position = Vector3.MoveTowards(transform.position, _destination, _speed * Time.deltaTime);
            }
        }
    }
}
