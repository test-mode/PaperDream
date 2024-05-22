using UnityEngine;

namespace PaperDream
{
    public static class Extensions
    {
        public static void RotateAndMove(this MonoBehaviour monoBehaviour, float originalYPosition, float rotationSpeed, float speedFactor, float lenght)
        {
            monoBehaviour.transform.Rotate(new Vector3(0f, rotationSpeed, 0f), Space.World);

            float newYPosition = originalYPosition + Mathf.PingPong(Time.time / speedFactor, lenght) - 0.5f;

            monoBehaviour.transform.position = new Vector3(
                monoBehaviour.transform.position.x,
                newYPosition,
                monoBehaviour.transform.position.z
            );
        }
    }
}
