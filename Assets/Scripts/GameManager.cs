using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class GameManager : Manager<GameManager>
{   
      

    public Transform team1Parent;
    public Transform team2Parent;

    public Action OnRoundStart;
    public Action OnRoundEnd;
    public Action<BaseEntity> OnUnitDied;

    List<BaseEntity> team1Entities = new List<BaseEntity>();
    List<BaseEntity> team2Entities = new List<BaseEntity>();
    public List<BaseEntity>allentitiesprefab;
    Dictionary<Team,List<BaseEntity>> entitiesByTeam=new Dictionary<Team, List<BaseEntity>>();
    int unitsPerTeam=2;
    void Start()
    {
        InstantiateUnits();
    }

    private void InstantiateUnits(){

        entitiesByTeam.Add(Team.team1,new List<BaseEntity>());
        entitiesByTeam.Add(Team.team2,new List<BaseEntity>());
        for(int i =0; i<unitsPerTeam;i++){
            //Crear unidades para team1
            int randomIndex=UnityEngine.Random.Range(0,allentitiesprefab.Count-1);
            BaseEntity newEntity= Instantiate(allentitiesprefab[randomIndex]);
            entitiesByTeam[Team.team1].Add(newEntity);

            newEntity.Setup(Team.team1,GridManager.Instance.GetfreeNode(Team.team1));
            Debug.Log("Created unit for team1");



            
            //Crear unidades para team2
            //int randomIndex2=UnityEngine.Random.Range(0,allentitiesprefab.Count-1);
            //newEntity= Instantiate(allentitiesprefab[randomIndex]);
            //entitiesByTeam[Team.team1].Add(newEntity);
            
            //newEntity.Setup(Team.team1,GridManager.Instance.GetfreeNode(Team.team1));

        }
    }

    public List<BaseEntity> GetBaseEntitiesAgainst(Team Againsteam){
        if(Againsteam==Team.team1){
            return entitiesByTeam[Team.team2];

        }
        else
        {
            return entitiesByTeam[Team.team1];
        }
    
    }
    public void UnitDead(BaseEntity entity)
    {
        if(OnUnitDied!=null){
            OnUnitDied(entity);
            Debug.Log("Unidad muerta");
        }
    }
}

public enum Team{
    team1,
    team2,
}