using DG.Tweening;
using UnityEngine;

namespace PaperDream
{
    public class Destination : MonoBehaviour
    {
        [SerializeField] private Transform _landPosition;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out PlaneController planeController))
            {
                Debug.Log("Reached Destination!");
                EventManager.OnReachedDestination();
                planeController.OnReachedDestination();
                planeController.transform.DOMove(_landPosition.position, 2f).OnComplete(() =>
                {

                });

                planeController.transform.DORotate(Quaternion.identity.eulerAngles, 2f).OnComplete(() =>
                {

                });
            }
        }
    }
}
