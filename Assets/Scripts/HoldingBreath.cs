using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SealBreath : MonoBehaviour
{
    [Header("Breath Settings")]
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

    void Start()
    {
        currentBreath = maxBreath;
        movement = GetComponent<SealMovement>();
        Debug.Log("Breath system initialized. Breath = " + currentBreath);
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

            Debug.Log($"Breath DRAINING | Breath: {currentBreath:F2} | WaterContacts: {waterContacts} | AirHoleContacts: {airHoleContacts}");
        }
        else
        {
            currentBreath += refillRate * Time.deltaTime;

            Debug.Log($"Breath REFILLING | Breath: {currentBreath:F2} | WaterContacts: {waterContacts} | AirHoleContacts: {airHoleContacts}");
        }

        currentBreath = Mathf.Clamp(currentBreath, 0f, maxBreath);

        if (currentBreath <= 0f)
            Die();
    }
    void Die()
    {
        isDead = true;
        Debug.Log("Seal drowned LOL!");

        if (loseScreen != null)
            loseScreen.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SealMovement movement = GetComponent<SealMovement>();
        if (movement != null)
            movement.enabled = false;
    }
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Entered trigger: " + other.name);

        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
        {
            waterContacts++;
            inWater = true;

            Debug.Log("Entered WATER | Contacts: " + waterContacts);
        }
        if (((1 << other.gameObject.layer) & whatIsAirHole) != 0)
        {
            airHoleContacts++;
            inAirHole = true;

            Debug.Log("Entered AIR HOLE | Contacts: " + airHoleContacts);
        }
    }
    void OnTriggerExit(Collider other)
    {
        Debug.Log("Exited trigger: " + other.name);

        if (((1 << other.gameObject.layer) & whatIsWater) != 0)
        {
            waterContacts--;

            if (waterContacts <= 0)
            {
                waterContacts = 0;
                inWater = false;
            }
            Debug.Log("Exited WATER | Contacts: " + waterContacts);
        }
        if (((1 << other.gameObject.layer) & whatIsAirHole) != 0)
        {
            airHoleContacts--;

            if (airHoleContacts <= 0)
            {
                airHoleContacts = 0;
                inAirHole = false;
            }
            Debug.Log("Exited AIR HOLE | Contacts: " + airHoleContacts);
        }
    }
}