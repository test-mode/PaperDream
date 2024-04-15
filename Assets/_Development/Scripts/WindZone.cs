using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDream
{
    public class WindZone : MonoBehaviour
    {
        [SerializeField] private float _windForce = 0f;

        private void OnTriggerStay(Collider other)
        {
            var hitObj = other.gameObject;
            if (hitObj != null)
            {
                var rb = hitObj.GetComponent<Rigidbody>();
                var dir = transform.up;
                rb.AddForce(dir * _windForce);
            }
        }
    }
}
