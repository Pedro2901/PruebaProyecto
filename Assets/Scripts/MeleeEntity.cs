using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEntity : BaseEntity
{

    
    public void Update()
{
    //Find new target
    if(!HasEnemy)
    {
        Debug.Log("Hola soy valentina y no tengo con quien divertirme te me unes?");
        FindTarget();
    }else
    {
        Debug.Log("Hola soy valentina y ya tengo con quien divertirme");
    }
   if (!HasEnemy)
        return;
    if( !moving){
         //attack
        Debug.Log("Hola soy valentina y me estoy moviendo");
        if(canAttack){
        Attack();
        Debug.Log("Ataca");
        }
        
    }else{
        GetInRange();
    }
}

    protected override void Attack()
    {
        base.Attack();
        currentTarget.TakeDamage(baseDamage);
    }
}
