using UnityEngine;

public class TimeCollector : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float baseSpeed = 2.5f;
    public float speed = 2.5f;
    public float turnSpeed = 10f;

    [Header("Damage")]
    public float damageTime = 10f;
    public float damageRadius = 2.2f;

    [Header("Pause After Hit")]
    public float pauseAfterHit = 2f;

    private float pauseTimer = 0f;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        // IMPORTANT FIX (phase check)
        if (!GameManager.Instance.IsGamePlaying())
            return;

        if (player == null)
            return;

        // Pause after hitting player
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        // ===== CHASE LOGIC (FIXED) =====
Vector3 playerPos = player.position;
playerPos.y = transform.position.y;

Vector3 direction = playerPos - transform.position;

if (direction.magnitude > 0.1f)
{
    direction.Normalize();

    transform.forward = Vector3.Lerp(
        transform.forward,
        direction,
        turnSpeed * Time.deltaTime
    );

    transform.position += transform.forward * speed * Time.deltaTime;
}

        // ===== HIT DETECTION =====
        Vector3 enemyFlat = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 playerFlat = new Vector3(player.position.x, 0f, player.position.z);

        float distance = Vector3.Distance(enemyFlat, playerFlat);

        if (distance <= damageRadius + 0.3f)
        {
            GameManager.Instance.RemoveTime(damageTime);
            GameManager.Instance.gameMessage = "Time Collector hit! -10 seconds";

            pauseTimer = pauseAfterHit;
        }
    }

    public void SetSpeedByDebt(int debt)
    {
        speed = baseSpeed + debt * 0.7f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
