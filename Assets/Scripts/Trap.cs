using UnityEngine;

public class Trap : MonoBehaviour
{
    public float damageTime = 7f;
    public float damageCooldown = 1f;
    public float damageRadius = 2.5f;

    private float cooldownTimer = 0f;
    private Transform player;

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    private void Update()
    {
        cooldownTimer -= Time.deltaTime;

        if (player == null)
            return;

        Vector3 trapPosition = new Vector3(transform.position.x, 0f, transform.position.z);
        Vector3 playerPosition = new Vector3(player.position.x, 0f, player.position.z);

        float distance = Vector3.Distance(trapPosition, playerPosition);

        if (distance <= damageRadius && cooldownTimer <= 0f)
        {
            GameManager.Instance.RemoveTime(damageTime);
            GameManager.Instance.gameMessage = "Trap hit! -7 seconds";
            cooldownTimer = damageCooldown;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}