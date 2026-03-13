using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OrcaCaughtYou : MonoBehaviour
{
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
        if (isDead) return;
        isDead = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("LOSESCREEN");
    }
}