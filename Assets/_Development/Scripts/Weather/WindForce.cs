using UnityEngine;

namespace PaperDream
{
    public class WindZone : MonoBehaviour
    {
        [SerializeField] private float _windForce = 0f;

        private void OnTriggerStay(Collider other)
        {
            if (other.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                Vector3 dir = transform.up;
                rb.AddForce(dir * _windForce);
            }
        }
    }
}
