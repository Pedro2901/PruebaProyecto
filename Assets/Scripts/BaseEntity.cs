using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class BaseEntity : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public HealthBar barprefab;

    public int baseDamage = 1;
    public int baseHealth = 1;
    [Range(1, 5)]
    public int range = 1;
    public float attackspeed = 1f;//ataque por segundo
    public float MovementSpeed = 1f;
    protected Team myTeam;
    protected Node currentNode;
    protected bool moving;
    protected Node destination;
    protected BaseEntity currentTarget = null;
    protected bool HasEnemy => currentTarget != null;
    protected bool IsInRange => currentTarget != null && Vector3.Distance(this.transform.position, currentTarget.transform.position) <= range;

    protected bool canAttack = true;
    protected bool dead = false;



    protected HealthBar healthBar;


    protected Node candidateDestination =null;

    public void Setup(Team team, Node SpawnNode)
    {
        myTeam = team;
        if (myTeam == Team.team2)
        {
            spriteRenderer.flipX = true;
        }
        this.currentNode = SpawnNode;
        transform.position = currentNode.WorldPosition;
        currentNode.SetOccupied(true);
        healthBar = Instantiate(barprefab, this.transform);
        healthBar.Setup(this.transform, baseHealth);

    }



    protected void FindTarget()
    {
        Debug.Log("finding target");
        var allenemies = GameManager.Instance.GetBaseEntitiesAgainst(myTeam);

        float minDistance = Mathf.Infinity;
        BaseEntity candidateTarget = null;
        foreach (BaseEntity e in allenemies)
        {
            if (Vector3.Distance(e.transform.position, this.transform.position) < minDistance)
            {
                Debug.Log("found new target");
                Debug.Log("Distance to " + e.name + " is " + Vector3.Distance(e.transform.position, this.transform.position));
                minDistance = Vector3.Distance(e.transform.position, this.transform.position);
                candidateTarget = e;
            }
        }
        currentTarget = candidateTarget;
    }

    protected void GetInRange()
    {
        if (currentTarget == null)
            return;

        if(!moving)
        {
            destination = null;
            List<Node> candidates = GridManager.Instance.GetNodesCloseTo(currentTarget.currentNode);
            candidates = candidates.OrderBy(x => Vector3.Distance(x.WorldPosition, this.transform.position)).ToList();
            for(int i = 0; i < candidates.Count;i++)
            {
                if (!candidates[i].IsOccupied)
                {
                    destination = candidates[i];
                    break;
                }
            }
            if (destination == null)
                return;

            var path = GridManager.Instance.GetPath(currentNode, destination);
            if (path == null && path.Count >= 1)
                return;

            if (path[1].IsOccupied)
                return;

            path[1].SetOccupied(true);
            destination = path[1];            
        }

        moving = !MoveTowards(destination);
        if(!moving)
        {
            //Free previous node
            currentNode.SetOccupied(false);
            SetCurrentNode(destination);
        }
    }

    public void SetCurrentNode(Node node)
    {
        currentNode = node;
    }

    public bool MoveTowards(Node Destino)
    {
        Debug.Log("Hola soy valentina y estoy en MoveTowards");
        Vector3 direction =Destino.WorldPosition - this.transform.position;
        if (direction.sqrMagnitude <= 0.005f )
        {
            transform.position = Destino.WorldPosition;
            return true;
        }
        this.transform.position += direction.normalized * MovementSpeed * Time.deltaTime;
        Debug.Log("Hola soy valentina y voy a falseeee");
        return false;
    }

    protected virtual void Attack()
    {
        if (!canAttack)
        {
            return;
        }
        float WaitBeetweenAttack = 1;
        StartCoroutine(WaitAttackCoroutine(WaitBeetweenAttack));

    }

    IEnumerator WaitAttackCoroutine(float waittime)
    {

        canAttack = false;
        yield return new WaitForSeconds(waittime);
        canAttack = true;

    }

    public void TakeDamage(int Amount)
    {
        baseHealth -= Amount;
        Debug.Log("Health is " + baseHealth);
        healthBar.UptadeBar(baseHealth);

        if (baseHealth <= 0)
        {

            dead = true;
            currentNode.SetOccupied(false);
            GameManager.Instance.UnitDead(this);
        }
    }
}