using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public int value;
    public enum PickupType { Experience, Money, Fragment }
    public PickupType type; 
    public float pickupRadius = 1f;

    private void Update()
    {
        //Check if player is within pickup radius
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && Vector2.Distance(transform.position, player.transform.position) <= pickupRadius)
        {
            switch (type)
            {
                case PickupType.Experience:
                    GameManager.instance.AddExperience(value);
                    break;
                case PickupType.Money:
                    GameManager.instance.AddMoney(value);
                    break;
                case PickupType.Fragment:
                    GameManager.instance.AddFragments(value); // Ensure you have a method to add fragments in your GameManager
                    break;
            }

            Destroy(gameObject);
        }
    }
}
