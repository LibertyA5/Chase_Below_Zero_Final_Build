using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FoundFamily : MonoBehaviour
{
    [Header("Detection")]
    public LayerMask whatIsFamily;

    private bool foundFam = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (foundFam) return;

        if ((whatIsFamily.value & (1 << collision.gameObject.layer)) != 0)
        {
            Found();
        }
    }
    private void Found()
    {
        foundFam = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("WINSCREEN");
    }
}