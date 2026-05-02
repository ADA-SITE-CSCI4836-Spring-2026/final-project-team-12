using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float dashDistance = 4f;
    public float dashCost = 3f;
    public float dashCooldown = 0.8f;

    private CharacterController controller;
    private float cooldownTimer = 0f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (GameManager.Instance == null || !GameManager.Instance.IsGamePlaying())
            return;

        cooldownTimer -= Time.deltaTime;

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = new Vector3(x, 0f, z).normalized;

        controller.Move(move * moveSpeed * Time.deltaTime);
        controller.Move(Vector3.down * 9.81f * Time.deltaTime);

        if (move != Vector3.zero)
            transform.forward = move;

        

        if (Input.GetKeyDown(KeyCode.LeftShift) && cooldownTimer <= 0f && move != Vector3.zero)
        {
            GameManager.Instance.RemoveTime(dashCost);
            controller.Move(move * dashDistance);
            cooldownTimer = dashCooldown;
        }
    }
}