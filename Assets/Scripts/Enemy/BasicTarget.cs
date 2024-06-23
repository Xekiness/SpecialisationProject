using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTarget : MonoBehaviour, ITarget
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    private Material originalMaterial;


    //Can add healthbars?
    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        originalMaterial = renderer ? renderer.material : null;

        currentHealth = maxHealth;
    }
    public void OnDamaged(int damage)
    {
        if (gameObject != null)
        {
            currentHealth -= damage;

            if (currentHealth <= 0)
            {
                Destroy();
            }
            else
            {
                StartCoroutine(FlashRed());
            }
        }
    }
    public void Destroy()
    {
        //Destroy the gameobject
        //gameObject.SetActive(false);
        Destroy(gameObject);

        //Can add particles when destroy / sound etc
    }
    private IEnumerator FlashRed()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null && originalMaterial != null)
        {
            Color originalColor = renderer.material.color;
            renderer.material.color = Color.red;

            yield return new WaitForSeconds(0.2f);

            //renderer.material.color = originalColor;
            renderer.material.color = Color.white;
        }
    }

    public int CheckHealth()
    {
        return currentHealth;
    }
}
