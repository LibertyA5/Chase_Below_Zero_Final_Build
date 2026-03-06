using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcaCaughtYou : MonoBehaviour
{
    [Header("Lose Screen UI")]
    public GameObject loseScreen;

    [Header("Detection")]
    public LayerMask whatIsOrca;

    private bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if ((whatIsOrca.value & (1 << collision.gameObject.layer)) != 0)
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

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SealMovement movement = GetComponent<SealMovement>();
        if (movement != null)
            movement.enabled = false;
    }
}
