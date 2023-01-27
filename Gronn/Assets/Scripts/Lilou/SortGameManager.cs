using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortGameManager : MonoBehaviour
{
    public List<GameObject> spawners;
    public List<GameObject> brownWaste;
    public List<GameObject> greenWaste;
    public List<GameObject> yellowWaste;
    private List<Trash> spawnedWaste;

    public List<GameObject> players;
    public List<GameObject> planks;

    public float vitesse;
    private float spawnTimer = 0f;
    public GameObject globalTimer;

    bool started = false;
    bool ended = false;
    //1 for right, -1 for left
    int[] currentSide = { 1, 1 };

    int score = 0;
    int total = 0;

    bool PositiveAnimation = true;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(tempWait());
    }

    IEnumerator tempWait() {
        yield return new WaitForSeconds(3);
        startGame();
    }

    void startGame() {
        spawnedWaste = new List<Trash>();
        globalTimer.GetComponent<Timer>().restart();
        ended = false;
        started = true;
        for (int i = 0; i < players.Count; i++) {
            players[i].GetComponent<Animator>().Play("Head Layer.up", -1, 0.0f);
            players[i].GetComponent<Animator>().Play("Arms Layer.FirstTurn", -1, 0.0f);
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (started) {

            if (globalTimer.GetComponent<Timer>().isOver() && !ended) {
                if (spawnedWaste.Count == 0) {
                    ended = true;
                    ending();
                }
            }

            spawnTimer -= 1 * Time.deltaTime;
            if (globalTimer.GetComponent<Timer>().remainingTime() >= vitesse) {
                if (spawnTimer <= 0) {
                    spawnTimer = vitesse;
                    //spawn waste
                    spawnTrash();
                }
            }

            for (int i = 0; i < spawnedWaste.Count; i++) {
                if (spawnedWaste[i].prefab.GetComponent<WasteCollisionManager>().hasScored) {
                    if (spawnedWaste[i].prefab.GetComponent<WasteCollisionManager>().trash == spawnedWaste[i].type) {
                        score++;
                    }  
                    else 
                        PositiveAnimation = false;  
                    Destroy(spawnedWaste[i].prefab, 3.0f);
                    spawnedWaste.RemoveAt(i);
                    i--;
                    count++;
                }
            }

            if (count >= 2) {
                count = 0;
                for (int i = 0; i < players.Count; i++) {
                    if (PositiveAnimation) 
                        players[i].GetComponent<Animator>().Play("Head Layer.yay", -1, 0.0f);
                    else 
                        players[i].GetComponent<Animator>().Play("Head Layer.nay", -1, 0.0f);
                }
                PositiveAnimation = true;
            }

            //Left P1
            if (Input.GetKeyDown(KeyCode.Q)) {
                if (currentSide[0] > 0) {
                    players[0].GetComponent<Animator>().Play("Arms Layer.R2L", -1, 0.0f);
                    currentSide[0] = -1;
                }
            }
            //Right P1
            else if (Input.GetKeyDown(KeyCode.D)) {
                if (currentSide[0] < 0) {
                    players[0].GetComponent<Animator>().Play("Arms Layer.L2R", -1, 0.0f);
                    currentSide[0] = 1;
                }
            }
            //Up P1
            else if (Input.GetKeyDown(KeyCode.Z)) {
                //R
                if (currentSide[0] > 0) {
                    players[0].GetComponent<Animator>().Play("Arms Layer.launch", -1, 0.0f);
                    planks[0].tag = "up";
                    StartCoroutine(releaseTag(0));
                }
                //L
                if (currentSide[0] < 0)
                {
                    players[0].GetComponent<Animator>().Play("Arms Layer.launchB", -1, 0.0f);
                    planks[0].tag = "up";
                    StartCoroutine(releaseTag(0));
                }
            }

            //Left P2
            if (Input.GetKeyDown(KeyCode.LeftArrow)) {
                if (currentSide[1] > 0) {
                    players[1].GetComponent<Animator>().Play("Arms Layer.R2L", -1, 0.0f);
                    currentSide[1] = -1;
                }
            }
            //Right P2
            else if (Input.GetKeyDown(KeyCode.RightArrow)) {
                if (currentSide[1] < 0) {
                    players[1].GetComponent<Animator>().Play("Arms Layer.L2R", -1, 0.0f);
                    currentSide[1] = 1;
                }
            }
            //Up P2
            else if (Input.GetKeyDown(KeyCode.UpArrow)) {
                //R
                if (currentSide[1] < 0) {
                    players[1].GetComponent<Animator>().Play("Arms Layer.launch", -1, 0.0f);
                    planks[1].tag = "up";
                    StartCoroutine(releaseTag(1));
                }
                //L
                if (currentSide[1] > 0)
                {
                    players[1].GetComponent<Animator>().Play("Arms Layer.launchB", -1, 0.0f);
                    planks[1].tag = "up";
                    StartCoroutine(releaseTag(0));
                }
            }
        }
    }

    IEnumerator releaseTag(int i) {
        yield return new WaitForSeconds(0.25f);
        planks[i].tag = "blank";
    }

    void spawnTrash() {
        for (int i = 0; i < spawners.Count ; i++) {
            Trash newTrash = new Trash(Random.Range(0,3));
            total ++;
            switch (newTrash.type) {
                case 0:
                    newTrash.prefab = Instantiate(brownWaste[Random.Range(0,brownWaste.Count)], spawners[i].transform.position, spawners[i].transform.rotation);
                    break;
                case 1:
                    newTrash.prefab = Instantiate(greenWaste[Random.Range(0,greenWaste.Count)], spawners[i].transform.position, spawners[i].transform.rotation);
                    break;
                case 2:
                    newTrash.prefab = Instantiate(yellowWaste[Random.Range(0,yellowWaste.Count)], spawners[i].transform.position, spawners[i].transform.rotation);
                    break;
                default:
                    break;
            }
            spawnedWaste.Add(newTrash);
        }
    }

    void ending() {
        //Score
        Debug.Log(score + "/" + total);

        if (score > 15)
        {
            GameManager.instance.set_finished_game(3);
            GameManager.instance.load_scene("EarthGameEndBis");
        } else
        {
            GameManager.instance.load_scene("EarthGameEnd");
        }
    }
}
