using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaCaughtYou : MonoBehaviour
{
    [Header("Lose Screen UI")]
    public GameObject loseScreen;

    private bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Orca"))
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        Debug.Log("The Orca got you!");

        if (loseScreen != null)
            loseScreen.SetActive(true);

        SealMovement movement = GetComponent<SealMovement>();
        if (movement != null)
            movement.enabled = false;
    }
}
