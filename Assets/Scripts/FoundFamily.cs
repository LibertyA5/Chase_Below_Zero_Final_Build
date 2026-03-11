using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FoundFamily : MonoBehaviour
{
    [Header("Win Screen UI")]
    public GameObject winScreen;

    [Header("Detection")]
    public LayerMask whatIsFamily;

    private bool foundFam = false;
    public GameObject restartButton;

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
        Debug.Log("You made it back to your family!");

        if (winScreen != null)
            winScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(restartButton);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SealMovement movement = GetComponent<SealMovement>();
        if (movement != null)
            movement.enabled = false;
    }
}
