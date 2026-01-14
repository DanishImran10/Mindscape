using UnityEngine;

public class BrainRotation : MonoBehaviour
{
    private Vector2 lastTouchPosition; // Last touch position for rotation
    private bool isDragging = false;
    public float rotationSpeed = 0.2f; // Rotation speed
    public float scaleSpeed = 0.01f; // Speed of scaling
    public float minScale = 4.7f, maxScale = 10f; // Min & Max scale limits

    void Update()
    {
        if (Input.touchCount == 1) // Single touch for rotation
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition;

                float rotationX = delta.y * rotationSpeed; // Up/Down Rotation
                float rotationY = -delta.x * rotationSpeed; // Left/Right Rotation

                transform.Rotate(Vector3.right, rotationX, Space.World);
                transform.Rotate(Vector3.up, rotationY, Space.World);

                lastTouchPosition = touch.position; // Update last position
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
            }
        }
        else if (Input.touchCount == 2) // Two fingers for scaling
        {
            Touch touch0 = Input.GetTouch(0);
            Touch touch1 = Input.GetTouch(1);

            float prevDistance = (touch0.position - touch0.deltaPosition - (touch1.position - touch1.deltaPosition)).magnitude;
            float currentDistance = (touch0.position - touch1.position).magnitude;

            float deltaDistance = currentDistance - prevDistance;

            Vector3 newScale = transform.localScale + Vector3.one * deltaDistance * scaleSpeed;
            newScale = Vector3.Max(newScale, Vector3.one * minScale); // Limit min scale
            newScale = Vector3.Min(newScale, Vector3.one * maxScale); // Limit max scale

            transform.localScale = newScale; // Apply new scale
        }
    }
}