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

    private bool playingRandomSound = false;

    void Start()
    {
        sealMovement = GetComponent<SealMovement>();
        StartCoroutine(PlayRandomSealSound());
    }
    void Update()
    {
        HandleMovementSounds();

        // Stop bark instantly if entering water
        if (sealMovement.isInWater && playingRandomSound)
        {
            audioSource.Stop();
            playingRandomSound = false;
        }
    }
    void HandleMovementSounds()
    {
        bool isMoving = Mathf.Abs(sealMovement.horizontalInput) > 0.1f ||
                        Mathf.Abs(sealMovement.verticalInput) > 0.1f;

        if (sealMovement.isInWater && isMoving)
        {
            if (audioSource.clip != swimSound)
            {
                audioSource.Stop();
                audioSource.clip = swimSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (sealMovement.isInWater && !isMoving)
        {
            if (audioSource.clip != idleSound)
            {
                audioSource.Stop();
                audioSource.clip = idleSound;
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else if (!sealMovement.isInWater && audioSource.loop)
        {
            audioSource.Stop();
        }
    }
    IEnumerator PlayRandomSealSound()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenSounds, maxTimeBetweenSounds);
            yield return new WaitForSeconds(waitTime);

            if (sealMovement.isInWater)
                continue;

            AudioClip chosenSound = Random.value > 0.5f ? sound1 : sound2;

            playingRandomSound = true;

            audioSource.clip = chosenSound;
            audioSource.loop = false;
            audioSource.Play();

            yield return new WaitUntil(() => !audioSource.isPlaying);

            playingRandomSound = false;
        }
    }
}