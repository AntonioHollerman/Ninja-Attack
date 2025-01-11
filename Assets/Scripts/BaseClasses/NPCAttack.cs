using System.Collections;
using UnityEngine;

public class NPCAttack : MonoBehaviour
{
    public float attackRange = 1f;
    public int attackDamage = 10;
    public float attackCooldown = 1.5f;

    private float attackTimer;
    private Transform targetPlayer;

    void Update()
    {
        if (targetPlayer != null && attackTimer <= 0)
        {
            float distance = Vector2.Distance(transform.position, targetPlayer.position);
            if (distance <= attackRange)
            {
                Attack();
            }
        }

        attackTimer -= Time.deltaTime;
    }

    public void SetTargetPlayer(Transform player)
    {
        targetPlayer = player;
    }

    void Attack()
    {
        if (targetPlayer != null)
        {
            PlayerController playerController = targetPlayer.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.TakeDamage(attackDamage);
            }
        }

        attackTimer = attackCooldown;
    }
}
