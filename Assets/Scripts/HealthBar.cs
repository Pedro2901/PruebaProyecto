using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
public Transform bar ;
public Vector3 offset;

private float MaxHealth;
private Transform target;

public void Setup(Transform target,float MaxHealth){
this.MaxHealth=MaxHealth;
this.target=target;
UptadeBar(MaxHealth);

}
public void UptadeBar(float NewMaxHealth){
    float newScale= NewMaxHealth/MaxHealth;
    Vector3 scale = bar.transform.localScale;
    scale.x= newScale;
    bar.transform.localScale=scale;
}

private void Update()
{
    if(target!=null){
        this.transform.position=target.position + offset;
    }  
}
}
