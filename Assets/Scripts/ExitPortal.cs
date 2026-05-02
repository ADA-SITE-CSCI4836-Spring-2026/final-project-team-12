using UnityEngine;

public class ExitPortal : MonoBehaviour
{
    public float rotationSpeed = 80f;

    private void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.TryWin();
        }
    }
}   