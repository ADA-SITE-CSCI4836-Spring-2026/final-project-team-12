using UnityEngine;

public class TimeCollector : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float baseSpeed = 2.5f;
    public float speed = 2.5f;

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

        if (GameManager.Instance.gameOver)
            return;

        if (player == null)
            return;

        // If collector just hit player, wait before chasing again
        if (pauseTimer > 0f)
        {
            pauseTimer -= Time.deltaTime;
            return;
        }

        // Chase player
        Vector3 targetPosition = new Vector3(
            player.position.x,
            transform.position.y,
            player.position.z
        );

        transform.position = Vector3.MoveTowards(
            transform.position,
            targetPosition,
            speed * Time.deltaTime
        );

        Vector3 direction = targetPosition - transform.position;

        if (direction != Vector3.zero)
            transform.forward = direction;

        // Distance-based hit detection
        Vector3 enemyFlat = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 playerFlat = new Vector3(player.position.x, 0f, player.position.z);

        float distance = Vector3.Distance(enemyFlat, playerFlat);

        if (distance <= damageRadius)
        {
            GameManager.Instance.RemoveTime(damageTime);
            GameManager.Instance.gameMessage = "Time Collector hit! -10 seconds";

            // Wait 2 seconds before chasing again
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