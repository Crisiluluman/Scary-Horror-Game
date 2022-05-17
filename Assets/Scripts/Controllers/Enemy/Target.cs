using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private float health = 50f;


    public void TakeDamage(float amount)
    {
        health -= amount;

        Debug.Log("HEALTH: " + health );

        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}
