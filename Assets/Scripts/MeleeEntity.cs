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
        FindTarget();
    }
   if (!HasEnemy)
        return;
    if(IsInRange && !moving){
         //attack

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
