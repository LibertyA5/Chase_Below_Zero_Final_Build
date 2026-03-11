using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class SealBreath : MonoBehaviour
{
    [Header("Breath Settings")]
    public Slider breathSlider;
    public float maxBreath = 10f;
    public float drainRate = 1f;
    public float sprintDrainRate = 1f;
    public float refillRate = 3f;

    [Header("Detection Layers")]
    public LayerMask whatIsWater;
    public LayerMask whatIsAirHole;

    [Header("Lose Screen UI")]
    public GameObject loseScreen;

    float currentBreath;

    bool inWater = false;
    bool inAirHole = false;
    bool isDead = false;

    int waterContacts = 0;
    int airHoleContacts = 0;

    SealMovement movement;

    public GameObject restartButton;

    void Start()
    {
        currentBreath = maxBreath;
        movement = GetComponent<SealMovement>();

        breathSlider.maxValue = maxBreath;
        breathSlider.value = currentBreath;
    }

    void Update()
    {
        if (isDead) return;

        float totalDrain = drainRate;

        if (movement != null && movement.isSprinting)
            totalDrain += sprintDrainRate;
        if (inWater && !inAirHole)
        {
            currentBreath -= totalDrain * Time.deltaTime;
        }
        else
        {
            currentBreath += refillRate * Time.deltaTime;
        }

        currentBreath = Mathf.Clamp(currentBreath, 0f, maxBreath);

        if (currentBreath <= 0f)
            Die();

        breathSlider.value = currentBreath;
    }
    void Die()
    {
        isDead = true;

        if (loseScreen != null)
            loseScreen.SetActive(true);
            EventSystem.current.SetSelectedGameObject(restartButton);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SealMovement movement = GetComponent<SealMovement>();
        if (movement != null)
            movement.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {

        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
        {
            waterContacts++;
            inWater = true;

        }
        if (((1 << other.gameObject.layer) & whatIsAirHole) != 0)
        {
            airHoleContacts++;
            inAirHole = true;
        }
    }
    void OnTriggerExit(Collider other)
    {

        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
        {
            waterContacts--;

            if (waterContacts <= 0)
            {
                waterContacts = 0;
                inWater = false;
            }
        }
        if (((1 << other.gameObject.layer) & whatIsAirHole) != 0)
        {
            airHoleContacts--;

            if (airHoleContacts <= 0)
            {
                airHoleContacts = 0;
                inAirHole = false;
            }
        }
    }
}