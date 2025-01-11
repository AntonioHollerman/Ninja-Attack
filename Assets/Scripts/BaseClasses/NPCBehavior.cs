using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBehavior : MonoBehaviour
{
    public float detectionRange = 5f;
    public float movementSpeed = 2f;
    public Transform[] players; // Array of player transforms

    private Transform targetPlayer;
    private bool isChasing;

    void Update()
    {
        DetectPlayers();
        if (isChasing && targetPlayer != null)
        {
            FollowPlayer();
        }
    }

    void DetectPlayers()
    {
        float closestDistance = detectionRange;
        targetPlayer = null;

        foreach (Transform player in players)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                targetPlayer = player;
                isChasing = true;
            }
        }

        if (targetPlayer == null)
        {
            isChasing = false;
        }
    }

    void FollowPlayer()
    {
        Vector2 direction = (targetPlayer.position - transform.position).normalized;
        transform.position += (Vector3)direction * movementSpeed * Time.deltaTime;
    }
}
