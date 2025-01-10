using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickaxe_Script : MonoBehaviour
{   
    
    public float PickaxeDamage = -1;
    public float Health = 3f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Ore") && Input.GetMouseButtonDown(0))
        {
         OreHealth();
        }
    }
    public void OreHealth()
    {
        Health -= PickaxeDamage;

        if (Health <= 0f)
        {
            Destroy(gameObject);
        }
    }
}
