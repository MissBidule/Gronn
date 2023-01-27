using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Tile
{
    public BuildingObject buildingOccupierRef;
    public NatureObject natureOccupierRef;
    public GameObject sprite;

    public int occupierID;
    public bool occupied; 
    public int unoccupiedTime;
    
    public ObstacleType obstacleType;
    public enum ObstacleType
    {
        None,
        Nature,
        Building
    }
    
    public void SetOccupied(BuildingObject b)
    {
        occupied = true;
        obstacleType = ObstacleType.Building;
        occupierID = b.data.buildingID;
        unoccupiedTime = 0;
    }
    public void SetOccupied(NatureObject n)
    {
        occupied = true;
        obstacleType = ObstacleType.Nature;
        occupierID = n.data.natureID;
        unoccupiedTime = 0;
    }
    public void CleanTile()
    {
        unoccupiedTime=0;
        obstacleType = ObstacleType.None;
        occupied = false;
    }
}
