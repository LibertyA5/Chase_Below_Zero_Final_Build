using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seal_Sounds : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("Seal Random Sounds")]
    public AudioClip sound1;
    public AudioClip sound2;

    [Header("Movement Sounds")]
    public AudioClip idleSound;
    public AudioClip swimSound;

    [Header("Timing")]
    public float minTimeBetweenSounds = 5f;
    public float maxTimeBetweenSounds = 15f;

    private SealMovement sealMovement;
    private Rigidbody rb;

    void Start()
    {
        sealMovement = GetComponent<SealMovement>();
        rb = GetComponent<Rigidbody>();

        StartCoroutine(PlayRandomSealSound());
    }

    void Update()
    {
        HandleMovementSounds();
    }
    void HandleMovementSounds()
    {
        bool isMoving = Mathf.Abs(sealMovement.horizontalInput) > 0.1f ||
                        Mathf.Abs(sealMovement.verticalInput) > 0.1f;

        if (sealMovement.isInWater && isMoving)
        {
            if (audioSource.clip != swimSound)
            {
                audioSource.clip = swimSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (sealMovement.isInWater && !isMoving)
        {
            if (audioSource.clip != idleSound)
            {
                audioSource.clip = idleSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (!sealMovement.isInWater)
        {
            if (audioSource.loop)
                audioSource.Stop();
        }
    }
    IEnumerator PlayRandomSealSound()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
            yield return new WaitForSeconds(waitTime);

            AudioClip chosenSound = Random.value > 0.5f ? sound1 : sound2;
            audioSource.PlayOneShot(chosenSound);
        }
    }
}