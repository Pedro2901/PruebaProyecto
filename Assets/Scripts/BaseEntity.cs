using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BaseEntity : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public HealthBar barprefab;

    public int baseDamage=1;
    public int baseHealth=1;
    [Range(1,5)]
    public int range=1;
    public float attackspeed=1f;//ataque por segundo
    public float MovementSpeed=1f;
    protected Team myTeam;
    protected Node CurrentNode;
    protected bool moving;
    protected Node Destination;
    protected BaseEntity currentTarget=null;
    protected bool HasEnemy => currentTarget != null;
        protected bool IsInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;

    protected bool canAttack=true;
    protected bool dead= false;

    protected HealthBar healthBar;


    public void Setup(Team team , Node SpawnNode){
        myTeam=team;
        if(myTeam==Team.team2){
            spriteRenderer.flipX=true;
        }
    this.CurrentNode=SpawnNode;
    transform.position=CurrentNode.WorldPosition;
    CurrentNode.SetOccupied(true);
    healthBar=Instantiate(barprefab,this.transform);
    healthBar.Setup(this.transform, baseHealth);

    }



    protected void FindTarget(){
        var allenemies=GameManager.Instance.GetBaseEntitiesAgainst(myTeam);

        float minDistance=Mathf.Infinity;
        BaseEntity candidateTarget=null;
        foreach(BaseEntity e in allenemies){
            if(Vector3.Distance(e.transform.position,this.transform.position)<minDistance){
                minDistance=Vector3.Distance(e.transform.position, this.transform.position );
                candidateTarget=e;
            }
        }
        currentTarget=candidateTarget;
}

protected void GetInRange(){
    if(currentTarget!=null){
    return;
}
if(!moving){
Node candidateDestination=null;
List<Node> candidateNodes=GridManager.Instance.GetNodesCloseTo(currentTarget.CurrentNode);//nodos cercanos 
candidateNodes= candidateNodes.OrderBy(n=>Vector3.Distance(n.WorldPosition,this.transform.position)).ToList();//ordenar por distancia
    for(int i=0; i<candidateNodes.Count; i++){
        if(!candidateNodes[i].IsOccupied){
            candidateDestination=candidateNodes[i];
            break;
    }
}
if(candidateDestination==null){
return;
//Encontrar el camino de destino 

}
var path=GridManager.Instance.GetPath(CurrentNode,candidateDestination);
if(path==null || path.Count<=1){
    return;

}
if(path[1].IsOccupied){
    return;

}
path[1].SetOccupied(true);
Destination=path[1];


}
moving=!MoveTowards();
if(!moving){
    CurrentNode.SetOccupied(false);
    CurrentNode=Destination;
}
}
protected bool MoveTowards(){
    Vector3 direction=Destination.WorldPosition -this.transform.position;
    if(direction.sqrMagnitude <= 0.002f ){
    transform.position=Destination.WorldPosition;
    return true;
    }
    this.transform.position += direction.normalized * MovementSpeed * Time.deltaTime;
    return false;
}

protected virtual void Attack(){
    if(!canAttack){
return;
    }
    float WaitBeetweenAttack=1;
    StartCoroutine(WaitAttackCoroutine(WaitBeetweenAttack));

}

IEnumerator WaitAttackCoroutine(float waittime){

    canAttack =false;
    yield return new WaitForSeconds(waittime); 
    canAttack =true;

}

public void TakeDamage(int Amount){
    baseHealth-=Amount;
    healthBar.UptadeBar(baseHealth);
    if(baseHealth<=0){
    dead=true;
    CurrentNode.SetOccupied(false);
    GameManager.Instance.UnitDead(this);
    }
}
}