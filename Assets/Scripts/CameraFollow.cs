using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 18, -14);
    public float smoothSpeed = 8f;

    // Wall collision
    public float minDistance = 1.5f;  // how close camera can get to player
    public LayerMask collisionLayers; // set to "Default" in Inspector

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPos = target.position + offset;

        // Raycast from player toward desired camera position
        Vector3 dir = desiredPos - target.position;
        float desiredDist = dir.magnitude;

        RaycastHit hit;
        if (Physics.Raycast(target.position, dir.normalized, out hit, desiredDist, collisionLayers))
        {
            // Wall found — pull camera in front of it
            float safeDistance = Mathf.Max(hit.distance - 0.3f, minDistance);
            desiredPos = target.position + dir.normalized * safeDistance;
        }

        transform.position = Vector3.Lerp(transform.position, desiredPos, smoothSpeed * Time.deltaTime);
        transform.LookAt(target.position + Vector3.up * 1.5f);
    }
}
