using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ForestGameManager : MonoBehaviour
{    
    //survol des boutons donne les infos 
    //ne peut pas cliquer derriere un bouton 


    //INSTANCE
    public static ForestGameManager gameInstance;
    private void Awake(){gameInstance=this;}


    //ATTRIBUTES IN INSPECTOR

    [Header("Time")]
    public int gameDurationInSeconds;
    public int timeWithoutGrow;

    [Header("Objectives")]
    public int carbonReductionObjectivePercentage;
    public int maximalPopulationLossPercentage;

    [Header("World Builder")]
    public int levelWidth;
    public int levelLenght;
    public GameObject tilePrefab;
    public float tilesSize;
    public Transform tilesParent;
    public Transform buildingParent;
    public Transform natureParent;

    [Header("Initial ressources")]
    public int jungleBound;
    [Range(0,1)]
    public float jungleDensity; 
    [Range(0,1)]
    public float cerealsProportion;
    private int cereals;   
    [Range(0,1)]
    public float elevagesProportion; 
    private int elevages; 
    [Range(0,1)]
    public float vegetablesProportion;  
    private int vegetables;


    //OTHER ATTRIBUTES

    //Time
    [HideInInspector]
    public float timeLeft;
    [HideInInspector]
    public int year;
    [HideInInspector]
    public int yearsWithoutGrow;
    private float previousYearTime;
    private float yearDuration;

    //Tiles list
    [HideInInspector]
    public List<TileObject> tiles;

    //Building selected by the player
    [HideInInspector] 
    public int buildingToPlace;  

    //people
    [HideInInspector]
    public int peopleAtBeginning;
    [HideInInspector]
    public int people;
    [HideInInspector]
    public int previousPeople;
    [HideInInspector]
    public int fedPeople;

    //carbon
    [HideInInspector]
    public int initialCarbonBudget;
    [HideInInspector]
    public int carbonBudget;
    [HideInInspector]
    public int carbonEmission;

    //Number of building type list range by id
    [HideInInspector]
    public List<int> numberOfBuildings;
    //Number of nature type list range by id.
    [HideInInspector]
    public List<int> numberOfNatures;


    //METHODS

    private void Start()
    {   
        variablesInitialisation();
        CreateGrid();
        CreateInitialBuildings();
        FillEmptyTiles();
        InitializePeople();
        DefineCarbonBudget();
    }

    private void Update() 
    {
        timeLeft-=Time.deltaTime;
        
        if(NewYear())
        {
            FuturePeople();
            ActualCarbonBudget();
            if(Revolt()==true)
            {
                //Game over revolt
                SceneManager.LoadScene("ForestGameOverPeople");
            }
        }

        if(year==2100)
        {
            if(carbonBudget>=0)//victoires
            {   
                //great victory
                if(peopleAtBeginning<people){SceneManager.LoadScene("ForestGreatVictory");}
                //little victory
                else{SceneManager.LoadScene("ForestLittleVictory");}
            }
            else//game over non respect accord de Paris
            {
                SceneManager.LoadScene("ForestGameOverCarbon");
            }
        }

    }

    private void variablesInitialisation()
    {
        buildingToPlace=-2;

        timeLeft=(float)gameDurationInSeconds;
        previousYearTime=gameDurationInSeconds;
        year=2000;
        yearDuration=gameDurationInSeconds/100;
        yearsWithoutGrow=(int)((float)timeWithoutGrow/(float)yearDuration); 

        int TilesOutJungle=(levelWidth-(int)(2*jungleDensity*jungleBound)-1)*(levelLenght-(int)(2*jungleDensity*jungleBound)-1);
        cereals=(int)(cerealsProportion*TilesOutJungle)-1;
        elevages=(int)(elevagesProportion*TilesOutJungle)-1;
        vegetables=(int)(vegetablesProportion*TilesOutJungle)-1;

        for(int i=0; i< BuildingsDatabase.Instance.buildingsDatabase.Count; ++i)
        {
            numberOfBuildings.Add(0);
        }
        for(int i=0; i< NatureDatabase.Instance.natureDatabase.Count; ++i)
        {
            numberOfNatures.Add(0);
        }
    }

    //Grid creation
    private void CreateGrid()
    {   
        for(int x=0; x<levelWidth; ++x)
        {
            for(int z=0; z<levelLenght; ++z)
            {
                tiles.Add(SpawnTile(x*tilesSize,z*tilesSize));
                tiles[x*levelLenght+z].x=x;
                tiles[x*levelLenght+z].z=z;

                if(x<jungleBound || z<jungleBound || z> (levelLenght-jungleBound) || x>(levelWidth-jungleBound))
                {
                    bool spawnForest = Random.value <=jungleDensity;
                    if(spawnForest)
                    {
                        tiles[x*levelLenght+z].CreateNatureObject(1);
                    }
                }
            }
        }
    }
    private void CreateInitialBuildings()
    {   
        int cerealIndex=RandomIndexOutJungle();
        while(tiles[cerealIndex].data.occupied==true)
        {
            cerealIndex=RandomIndexOutJungle();
        }
        while(cereals>0)
        {   
            tiles[cerealIndex].CreateBuildingObject(0);
            cerealIndex=NewEmptyTileIndex(cerealIndex);
            cereals-=1;
        }

        int elevageIndex=RandomIndexOutJungle();
        while(tiles[elevageIndex].data.occupied==true)
        {
            elevageIndex=RandomIndexOutJungle();
        }
        while(elevages>0)
        {   
            tiles[elevageIndex].CreateBuildingObject(1);
            elevageIndex=NewEmptyTileIndex(elevageIndex);
            elevages-=1;
        }

        int vegetablesIndex=RandomIndexOutJungle();
        while(tiles[vegetablesIndex].data.occupied==true)
        {
            vegetablesIndex=RandomIndexOutJungle();
        }
        while(vegetables>0)
        {   
            tiles[vegetablesIndex].CreateBuildingObject(2);
            vegetablesIndex=NewEmptyTileIndex(vegetablesIndex);
            vegetables-=1;
        }
    }   
    private void FillEmptyTiles()
    {
        for(int i=0; i< tiles.Count;++i)
        {
            if(!tiles[i].data.occupied)
            {
                tiles[i].CreateNatureObject(1);
            }
        }
    }
    private TileObject SpawnTile(float x, float z)
    {
        GameObject tmp = Instantiate(tilePrefab) ;
        tmp.transform.position = new Vector3(x,0,z);
        tmp.transform.SetParent(tilesParent);
        TileObject tile = tmp.GetComponent<TileObject>();
        tile.data.CleanTile();
        return tile;
    }
    private int RandomIndexOutJungle()
    {    // selects a random index of a point outside the jungle strip
        int index=Random.Range(levelWidth, levelLenght*levelWidth-levelLenght);
        return index;
    }
    private int NewEmptyTileIndex(int index)
    {   
        int[] directions= new int[]{1, -1};
        int choice=Random.Range(0,1);       
        int tries=0;
        while(tiles[index+directions[choice]].data.occupied==true && tries<2)
        {
            tries+=1;
            if(choice!=1)
            {
                choice+=1;
            }
            else{choice=0;}
        }

        if(tiles[index+directions[choice]].data.occupied==true)
        {
            index=RandomIndexOutJungle();
            while(tiles[index].data.occupied==true)
            {
                index=RandomIndexOutJungle();
            }
        }
        else
        {
            index+=directions[choice];
        }
        return index;
    }

    //BuildMenuManager
    public void BuildingSelection(int id)
    {
        buildingToPlace=id;
    }

    //PeopleManager
    private void InitializePeople()
    {
        people=fedPeople;
        if(people<0){people=0;}
        previousPeople=people;
        peopleAtBeginning=people;
    }
    private void FuturePeople()
    {   
        previousPeople=people;
        
        if(people>fedPeople)
        {
            people=fedPeople;
        }
        else if(fedPeople>=(int)1.1f*people)
        {
            people=(int)1.1f*people+1;
        }
        else
        {
            people=fedPeople;
        }
        if(people<0){people=0;}
    }
    private bool Revolt()
    {
        if(people-previousPeople<-(int)(maximalPopulationLossPercentage*previousPeople/100))
        {
            return true;
        }
        return false;
    }

    //Carbon Manager
    private void DefineCarbonBudget()
    {
        carbonBudget=(int)((float)carbonEmission*(20.0f+(30.0f*(1.0f-(float)carbonReductionObjectivePercentage/200.0f))+(50.0f*(1.0f-(float)carbonReductionObjectivePercentage/100.0f))));
        initialCarbonBudget=carbonBudget;
    }
    private void ActualCarbonBudget()
    {
        carbonBudget-=carbonEmission;
    }


    //Year Manager
    public bool NewYear()
    {
        if((int)(timeLeft%yearDuration)==0 && (previousYearTime-timeLeft)>=yearDuration)
        {
            previousYearTime=timeLeft;
            year+=1;
            giveNewYearInfo();
            return true;
        }
        else
        {
            return false;
        }
    }
    public void giveNewYearInfo()
    {
        for(int i=0; i<tiles.Count;++i)
        {
            tiles[i].newYear=true;
        }
    }
} 