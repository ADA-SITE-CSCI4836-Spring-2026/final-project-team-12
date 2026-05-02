using UnityEngine;

public class RobberController : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Animator animator;
    private CharacterController controller;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (animator == null || controller == null) return;

        float moveX = 0f;
        float moveZ = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))    moveZ =  1f;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))  moveZ = -1f;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))  moveX = -1f;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) moveX =  1f;

        Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;

        // Apply gravity
        moveDir.y = -9.81f * Time.deltaTime;

        controller.Move(moveDir * moveSpeed * Time.deltaTime);

        // Animation
        bool isMoving = moveX != 0f || moveZ != 0f;
        animator.SetFloat("Speed", isMoving ? 1f : 0f);

        // Rotate to face movement direction
        Vector3 flatDir = new Vector3(moveX, 0, moveZ).normalized;
        if (flatDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                Quaternion.LookRotation(flatDir),
                10f * Time.deltaTime
            );
    }
}
