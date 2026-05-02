using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public float rotationSpeed = 100f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.AddKey();
            Destroy(gameObject);
        }
    }
}
