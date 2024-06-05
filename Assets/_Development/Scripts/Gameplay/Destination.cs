using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PaperDream
{
    public class Destination : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlaneController planeController)) 
            {
                Debug.Log("Reached Destination!");
                EventManager.OnReachedDestination();
            }
        }
    }
}
