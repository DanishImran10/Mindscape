using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    public GameObject targetObject;

    public float rotationSpeed = 5f; // for smooth rotation

    public void RotateTowardsTargetSmoothly()
    {
        if (targetObject != null)
        {
            Vector3 directionToTarget = targetObject.transform.position - transform.position;

            if (directionToTarget != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        else
        {
            Debug.LogWarning("Target object is not assigned!");
        }
    }
}