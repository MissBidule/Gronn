using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileObject : MonoBehaviour
{
  public Tile data;

  public int x;
  public int z;

  public int age=0;
  public int evolveAge=-1;
  public bool canEvolve=false;
  public int carbonStocks=0;

  public bool newYear=false;

  //methods

  private void Update()
  {
    if(newYear==true)
    {
      age+=1;
      Unoccupied();
      Evolve();
      newYear=false;
    }
  }

  private void OnMouseOver ()
  {
    if(Input.GetMouseButtonDown(1))
    {
			Popup popup = UIController.Instance.CreatePopup();
			popup.Init(UIController.Instance.MainCanvas,this);
	  };
  }

  private void OnMouseDown()
  {
    if(ForestGameManager.gameInstance.buildingToPlace>-1 && !data.occupied)
    {
      CreateBuildingObject(ForestGameManager.gameInstance.buildingToPlace);
    }
    else if(ForestGameManager.gameInstance.buildingToPlace==-1 && data.occupied)
    {
      DestroyObject();
    }
  }

  //MIEUX si vérifie que y assez de ressources pour installer le building
  public void CreateBuildingObject(int id)
  {
    //create a game object with the data of the building id
    GameObject ObjectToCreate=BuildingsDatabase.Instance.buildingsDatabase[id].buildingModel;
    data.buildingOccupierRef=ObjectToCreate.GetComponent<BuildingObject>();
    data.buildingOccupierRef.data.buildingID=id;
    data.buildingOccupierRef.data.carbonEmission=BuildingsDatabase.Instance.buildingsDatabase[id].carbonEmission;
    data.buildingOccupierRef.data.name=BuildingsDatabase.Instance.buildingsDatabase[id].name;
    data.buildingOccupierRef.data.fedPeople=BuildingsDatabase.Instance.buildingsDatabase[id].fedPeople;
    data.buildingOccupierRef.data.ressourcesNeeded=BuildingsDatabase.Instance.buildingsDatabase[id].ressourcesNeeded;
    
    data.SetOccupied(data.buildingOccupierRef);
    SpawnBuilding();

    //increase the game fed people, the carbon emissions and the number of this type of building
    ForestGameManager.gameInstance.fedPeople+=BuildingsDatabase.Instance.buildingsDatabase[id].fedPeople;
    ForestGameManager.gameInstance.carbonEmission+=BuildingsDatabase.Instance.buildingsDatabase[id].carbonEmission;
    ForestGameManager.gameInstance.numberOfBuildings[id]+=1;

    //check the ressources needed for the building and change the fed people accordingly
    for(int i=0; i<BuildingsDatabase.Instance.buildingsDatabase[id].ressourcesNeeded.Count ; ++i)
    {
      ForestGameManager.gameInstance.fedPeople-=BuildingsDatabase.Instance.buildingsDatabase[id].ressourcesNeeded[i].y*BuildingsDatabase.Instance.buildingsDatabase[BuildingsDatabase.Instance.buildingsDatabase[id].ressourcesNeeded[i].x].fedPeople;
    }
  }

  public void CreateNatureObject(int id)
  {
    //create a game object with the data of the nature id
    GameObject ObjectToCreate=NatureDatabase.Instance.natureDatabase[id].natureModel;
    data.natureOccupierRef=ObjectToCreate.GetComponent<NatureObject>();
    data.natureOccupierRef.data.natureID=id;
    data.natureOccupierRef.data.carbonStocks=NatureDatabase.Instance.natureDatabase[id].carbonStocks;
    data.natureOccupierRef.data.name=NatureDatabase.Instance.natureDatabase[id].name;
    data.natureOccupierRef.data.carbonAbsorption=NatureDatabase.Instance.natureDatabase[id].carbonAbsorption;
    data.natureOccupierRef.data.yearsOfGrowth=NatureDatabase.Instance.natureDatabase[id].yearsOfGrowth;
    data.natureOccupierRef.data.canEvolve=NatureDatabase.Instance.natureDatabase[id].canEvolve;
    
    age=0;
    evolveAge=NatureDatabase.Instance.natureDatabase[id].yearsOfGrowth;
    canEvolve=NatureDatabase.Instance.natureDatabase[id].canEvolve;
    carbonStocks=NatureDatabase.Instance.natureDatabase[id].carbonStocks;

    data.SetOccupied(data.natureOccupierRef);
    SpawnNature();

    //Decrease the total carbon emission and increase the number of this type of nature element
    ForestGameManager.gameInstance.carbonEmission-=NatureDatabase.Instance.natureDatabase[id].carbonAbsorption;
    ForestGameManager.gameInstance.numberOfNatures[id]+=1;
  }

  private void DestroyObject()
  {
    if(data.obstacleType==Tile.ObstacleType.Nature)
    {
      ForestGameManager.gameInstance.carbonEmission+=data.natureOccupierRef.data.carbonAbsorption;
      ForestGameManager.gameInstance.carbonBudget-=carbonStocks;
      carbonStocks=0;
      ForestGameManager.gameInstance.numberOfNatures[data.occupierID]-=1;
    }
    else if(data.obstacleType==Tile.ObstacleType.Building)
    {
      ForestGameManager.gameInstance.fedPeople-=data.buildingOccupierRef.data.fedPeople;
      ForestGameManager.gameInstance.carbonEmission-=data.buildingOccupierRef.data.carbonEmission;
      ForestGameManager.gameInstance.numberOfBuildings[data.occupierID]-=1;
      //check the ressources needed for the building and change the fed people accordingly
      for(int i=0; i<BuildingsDatabase.Instance.buildingsDatabase[data.occupierID].ressourcesNeeded.Count ; ++i)
      {
        ForestGameManager.gameInstance.fedPeople+=BuildingsDatabase.Instance.buildingsDatabase[data.occupierID].ressourcesNeeded[i].y*BuildingsDatabase.Instance.buildingsDatabase[BuildingsDatabase.Instance.buildingsDatabase[data.occupierID].ressourcesNeeded[i].x].fedPeople;
      }
    }
    Destroy(data.sprite);
    data.CleanTile();
  }
  
  private void SpawnBuilding()
  {
    data.sprite = Instantiate(data.buildingOccupierRef.gameObject);
    data.sprite.transform.position=transform.position;
    data.sprite.transform.SetParent(ForestGameManager.gameInstance.buildingParent);
  }

  private void SpawnNature()
  {
    data.sprite = Instantiate(data.natureOccupierRef.gameObject);
    data.sprite.transform.position=transform.position;
    data.sprite.transform.SetParent(ForestGameManager.gameInstance.natureParent);
  }

  private void Unoccupied()
  {
    if (data.unoccupiedTime==ForestGameManager.gameInstance.yearsWithoutGrow)
    { 

      bool jungleNeighbour=false;

      int lenght=ForestGameManager.gameInstance.levelLenght;
      int width=ForestGameManager.gameInstance.levelWidth;
      int index=x*lenght+z;

      if(index%lenght!=lenght-1)
      {
        if(ForestGameManager.gameInstance.tiles[index+1].data.obstacleType==Tile.ObstacleType.Nature)
        {
          if(ForestGameManager.gameInstance.tiles[index+1].data.occupierID==1)
          {
            CreateNatureObject(0);
            jungleNeighbour=true;
          }
        }
      }
      if(jungleNeighbour==false && index%lenght!=0)
      {
        if(ForestGameManager.gameInstance.tiles[index-1].data.obstacleType==Tile.ObstacleType.Nature)
        {
          if(ForestGameManager.gameInstance.tiles[index-1].data.occupierID==1)
          {
            CreateNatureObject(0);
            jungleNeighbour=true;
          }
        }
      }
      if(jungleNeighbour==false && index+lenght<lenght*width)
      {
        if(ForestGameManager.gameInstance.tiles[index+lenght].data.obstacleType==Tile.ObstacleType.Nature)
        {
          if(ForestGameManager.gameInstance.tiles[index+lenght].data.occupierID==1)
          {
            CreateNatureObject(0);
            jungleNeighbour=true;
          }
        }
      }
      if(jungleNeighbour==false && index-lenght>-1)
      {
        if(ForestGameManager.gameInstance.tiles[index-lenght].data.obstacleType==Tile.ObstacleType.Nature)
        {
          if(ForestGameManager.gameInstance.tiles[index-lenght].data.occupierID==1)
          {
            CreateNatureObject(0);
            jungleNeighbour=true;
          }
        }
      }

      if(jungleNeighbour==false)
      {
        CreateNatureObject(2);
      }
    }
    
    else if(data.occupied==false)
    {
      data.unoccupiedTime+=1;
    }
  }

  private void Evolve()
  { 
    if(canEvolve==true)
    {
      carbonStocks+=data.natureOccupierRef.data.carbonAbsorption;
      if(age==evolveAge)
      {
        int evolutionID=data.occupierID+1;
        ForestGameManager.gameInstance.carbonBudget+=carbonStocks;
        DestroyObject();
        CreateNatureObject(evolutionID);
      }
    }
  }
  
}