using UnityEngine;

public class PlanePath : MonoBehaviour
{
    [SerializeField] public Transform planeDestination; // The target destination for the plane
    [SerializeField] Transform movedObject;      // The plane or object being moved
    [SerializeField] float speed = 5f;           // Movement speed

    private void FixedUpdate()
    {
        // Check if both the movedObject and planeDestination are assigned
        if (movedObject != null && planeDestination != null)
        {
            // Move the plane towards the destination
            movedObject.position = Vector3.MoveTowards(
                movedObject.position,
                planeDestination.position,
                speed * Time.fixedDeltaTime
            );

            // Optionally, orient the plane towards the destination
            Vector3 direction = (planeDestination.position - movedObject.position).normalized;
            if (direction != Vector3.zero)
            {
                movedObject.rotation = Quaternion.LookRotation(direction);
            }
        }
        else
        {
            Debug.LogWarning("MovedObject or PlaneDestination is not assigned!");
        }
    }
}
