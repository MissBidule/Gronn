using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Building
{
    public string name;

    public int buildingID;

    public int carbonEmission;

    public int fedPeople;

    public List<Vector2Int> ressourcesNeeded;
    
    public GameObject buildingModel;
}
