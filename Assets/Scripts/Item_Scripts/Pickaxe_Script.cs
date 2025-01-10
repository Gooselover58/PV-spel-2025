using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe_Script : MonoBehaviour
{   
    public float pickaxeDamage = 1;
    public float health = 3f;
    bool nearOre = false;
    public GameObject[] pickaxeSounds;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (nearOre && Input.GetMouseButtonDown(0))
        {
            OreHealth();
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nearOre = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nearOre = false;
        }
    }

    public void OreHealth()
    {
        health -= pickaxeDamage;

        if (health <= 0f)
        {
            Destroy(gameObject);
        }
    }

    public void pickaxeAudioPlayer()
    {
        Instantiate()
    }
}