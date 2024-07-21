using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public enum PickupType { Experience, Money, Fragment }
    public PickupType type;
    public int value;
    [SerializeField] private float pickupRadius = 1f;

    private PlayerHud playerHud;
    private PlayerData playerData;

    [Header("Sound Effects")]
    [SerializeField] private AudioClip experiencePickupSound;
    [SerializeField] private AudioClip moneyPickupSound;
    [SerializeField] private AudioClip fragmentPickupSound;

    //private AudioSource audioSource;
    private bool isPickedUp = false;

    private void Start()
    {
        playerHud = GameObject.FindObjectOfType<PlayerHud>();
        playerData = GameManager.instance.playerData;
        //audioSource = GetComponent<AudioSource>();
    }

    //private void Update()
    //{
    //    if(isPickedUp)
    //    {
    //        return;
    //    }

    //    //Check if player is within pickup radius
    //    GameObject player = GameObject.FindGameObjectWithTag("Player");
    //    if (player != null && Vector2.Distance(transform.position, player.transform.position) <= pickupRadius)
    //    {
    //        switch (type)
    //        {
    //            case PickupType.Experience:
    //                playerData.AddExperience(value);
    //                playerHud.UpdateExperienceBar(playerData.currentExperience, playerData.experienceToNextLevel);
    //                PlaySound(experiencePickupSound);
    //                break;
    //            case PickupType.Money:
    //                playerData.AddMoney(value);
    //                playerHud.UpdateMoney(playerData.totalMoney);
    //                PlaySound(moneyPickupSound);
    //                break;
    //            case PickupType.Fragment:
    //                playerData.AddFragments(value);
    //                playerHud.UpdateFragments(playerData.totalFragments);
    //                PlaySound(fragmentPickupSound);
    //                break;
    //        }
    //        isPickedUp = true;

    //        // Deactivate the item and destroy it after the sound has played
    //        gameObject.SetActive(false);
    //        Destroy(gameObject, 1f);
    //    }
    //}

    public void Collected()
    {
        if (isPickedUp)
            return;

        PlayerHud playerHud = GameObject.FindObjectOfType<PlayerHud>();
        PlayerData playerData = GameManager.instance.playerData;

        switch (type)
        {
            case PickupType.Experience:
                playerData.AddExperience(value);
                if (playerHud != null)
                    playerHud.UpdateExperienceBar(playerData.currentExperience, playerData.experienceToNextLevel);
                PlaySoundAtPoint(experiencePickupSound);
                break;
            case PickupType.Money:
                playerData.AddMoney(value);
                if (playerHud != null)
                    playerHud.UpdateMoney(playerData.totalMoney);
                PlaySoundAtPoint(moneyPickupSound);
                break;
            case PickupType.Fragment:
                playerData.AddFragments(value);
                if (playerHud != null)
                    playerHud.UpdateFragments(playerData.totalFragments);
                PlaySoundAtPoint(fragmentPickupSound);
                break;
        }

        isPickedUp = true;

        // Deactivate the item and destroy it after the sound has played
        gameObject.SetActive(false);
        Destroy(gameObject, 1f);
    }

    private void PlaySoundAtPoint(AudioClip clip)
    {
        if (clip != null)
        {
            // Play the audio clip at the current position of the pickup object
            AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}