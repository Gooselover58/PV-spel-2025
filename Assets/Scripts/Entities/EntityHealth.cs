using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{

    [SerializeField] int baseHealth;
    int health;

    Coroutine damageFlashCoroutine = null;

    SpriteRenderer sprite;

    Color damageColor = Color.red;

    public TMP_Text deathText;

    private void Awake()
    {
        health = baseHealth;
        sprite = GetComponent<SpriteRenderer>();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Changes health by amount
    /// </summary>
    /// <param name="amount">How much should be added to health</param>
    public void ChangeHealth(int amount)
    {
        health += amount;

        if (damageFlashCoroutine == null)
        {
            damageFlashCoroutine = StartCoroutine(TakeDamageFlash());
        }
        else
        {
            StopCoroutine(damageFlashCoroutine);
            damageFlashCoroutine = StartCoroutine(TakeDamageFlash());
        }

        if (health <= 0)
        {
            print(gameObject.name + " died!!! This is so sad. ;(");
            if (gameObject.tag == "Player")
            {
                deathText.enabled = true;
                gameObject.SetActive(false);
                Destroy(gameObject, 5f);
            }
            if (gameObject.tag != "Player")
            {
                Destroy(gameObject, 0.1f);
            }
        }
    }

    private IEnumerator TakeDamageFlash()
    {
        sprite.color = damageColor;
        yield return new WaitForSeconds(0.2f);
        sprite.color = Color.white;
    }

    

}
