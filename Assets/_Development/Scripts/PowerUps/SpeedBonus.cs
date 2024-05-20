using UnityEngine;

namespace PaperDream
{
    public class SpeedBonus : MonoBehaviour
    {
        private bool _afterburner = false;

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _afterburner = true;
                EventManager.OnAfterBurnerToggle(_afterburner);
                GameObject.Destroy(gameObject);
            }
        }
    }
}
