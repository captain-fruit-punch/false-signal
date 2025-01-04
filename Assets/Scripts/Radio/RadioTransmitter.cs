using UnityEngine;

public class RadioTransmitter : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 10f;   // Radius of the sphere cast
    [SerializeField] private LayerMask listenerLayer;       // Layer to detect listeners

    public void Transmit(RadioMessage message)
    {
        // Find all listeners within the detection radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRadius, listenerLayer);

        foreach (var hitCollider in hitColliders)
        {
            // Check if the hit object has a RadioListener
            RadioListener listener = hitCollider.GetComponent<RadioListener>();
            if (listener != null)
            {
                listener.OnReceiveMessage(message);
            }
        }
    }

    // Optional: Visualize the detection radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
