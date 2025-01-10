using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke(nameof(Selfdestruct),15f);  
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Selfdestruct()
    {
        Destroy(gameObject);
    }
}
