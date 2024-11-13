using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MilkFoamMaker : MonoBehaviour
{
    public GameObject pitcher;
    public GameObject steamWand;
    public float foamingDuration = 2.0f;  // Duration required for foaming

    public Sprite pitcherWithFoamSprite;
    public Sprite pitcherWithMilkSprite;

    public AudioSource steamWandAudio;  // Audio source for steam wand sound

    private bool isFoaming = false;
    private bool hasPlayedSteamSound = false; // To ensure sound plays only once per collision
    private float foamingTime = 0.0f;  // Track how long the pitcher has been colliding with the steam wand

    private void Update()
    {
        if (isFoaming)
        {
            // Increment the foaming time
            foamingTime += Time.deltaTime;

            // Check if the foaming time exceeds the required duration
            if (foamingTime >= foamingDuration)
            {
                pitcher.GetComponent<SpriteRenderer>().sprite = pitcherWithFoamSprite;
                StopFoamingProcess();
                Debug.Log("Finished foaming!");
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collide with pitcher");

        if (other.gameObject == pitcher && pitcher.GetComponent<SpriteRenderer>().sprite == pitcherWithMilkSprite)
        {
            StartFoamingProcess();
            // Play steam wand sound effect if it hasn't played yet
            if (!hasPlayedSteamSound && steamWandAudio != null)
            {
                steamWandAudio.Play();
                hasPlayedSteamSound = true;
                Debug.Log("Steam wand sound played.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == pitcher)
        {
            StopFoamingProcess();
            // Reset sound flag when pitcher leaves
            hasPlayedSteamSound = false;
        }
    }

    private void StartFoamingProcess()
    {
        if (!isFoaming)
        {
            isFoaming = true;
            foamingTime = 0.0f;  // Reset foaming time
        }
    }

    private void StopFoamingProcess()
    {
        isFoaming = false;
        foamingTime = 0.0f;  // Reset foaming time
        if (steamWandAudio != null && steamWandAudio.isPlaying)
        {
            steamWandAudio.Stop(); // Stop steam sound when foaming stops
        }
    }
}
