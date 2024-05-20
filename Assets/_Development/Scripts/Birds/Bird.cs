using System.Collections;
using UnityEngine;

namespace PaperDream
{
    public class Bird : MonoBehaviour
    {
        [SerializeField] private float _speed = 1;
        [SerializeField] private float _damage = 1;

        public void SetDestination(Vector3 destination)
        {
            StopAllCoroutines();
            StartCoroutine(MoveDestination(destination));
        }

        public void SetDestination(Transform target)
        {
            StopAllCoroutines();
            StartCoroutine(MoveDestination(target));
        }

        private IEnumerator MoveDestination(Vector3 destination)
        {
            // Move the bird towards the target
            while (Vector3.Distance(transform.position, destination) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, destination, _speed * Time.deltaTime);
                yield return null;
            }
        }

        private IEnumerator MoveDestination(Transform target)
        {
            // Move the bird towards the target
            while (Vector3.Distance(transform.position, target.position) > 0.1f)
            {
                transform.position = Vector3.MoveTowards(transform.position, target.position, _speed * Time.deltaTime);
                yield return null;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakeDamage(_damage);
                Destroy(gameObject);
            }
        }
    }
}
