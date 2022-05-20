using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torreta : BaseEntity
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!HasEnemy)
        {
            FindTarget();    
        }else
        {
            Attack();
            Debug.Log("Hola soy valentina y estoy atacando"); 
        }
    }
}
