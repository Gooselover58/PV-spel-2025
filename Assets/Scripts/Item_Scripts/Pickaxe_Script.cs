using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe_Script : MonoBehaviour
{   
    
    public float PickaxeDamage = 1;
    public float Health = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetMouseButtonDown(0))
        {
            Debug.Log("du är inne i if-satsen");
            OreHealth();
        }
    }
    public void OreHealth()
    {
        Debug.Log("ore metoden har callats");
        Health -= PickaxeDamage;

        if (Health <= 0f)
        {
            Destroy(gameObject);
            Debug.Log("föremålet har förstörts");
        }
    }
}
