using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{

    [SerializeField] int baseHealth;
    int health;

    private void Awake()
    {
        health = baseHealth;
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
        if (health <= 0)
        {
            print(gameObject.name + " died!!! This is so sad.");
        }
    }

}
