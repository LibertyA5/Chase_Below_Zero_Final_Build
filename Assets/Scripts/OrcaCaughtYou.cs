using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OrcaCaughtYou : MonoBehaviour
{
    [Header("Lose Screen UI")]
    public GameObject loseScreen;

    [Header("Detection")]
    public LayerMask whatIsOrca;

    private bool isDead = false;
    public GameObject restartButton;

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
        if (isDead) return;
        isDead = true;

        if (loseScreen != null)
        {
            loseScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(restartButton);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SealMovement movement = GetComponent<SealMovement>();
        if (movement != null)
            movement.enabled = false;
    }
}
