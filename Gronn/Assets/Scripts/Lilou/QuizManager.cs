using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public List<QnA> QAndA;
    public Text[] answersText;
    public List<GameObject> players;
    public GameObject timer;
    public GameObject BG;

    public Text questionText;

    [SerializeField] private List<Animator> answerBlock = null;
    
    private GameObject lift = null;
    private GameObject playerParent = null;

    int currentQuestion = -1;
    bool endOfTheRound = true;
    float score = 0;
    int nbQuestions;

    void singlePlayer() {
        players[1].SetActive(false);
        players.RemoveAt(1);
        Vector3 pos = players[0].transform.localPosition;
        pos.z = 1;
        players[0].transform.localPosition = pos;
    }

    void Start() {
        //singlePlayer();
        generateQuestion();
        nbQuestions = QAndA.Count;
        playerParent = players[0].transform.parent.gameObject;
        lift = playerParent.transform.parent.gameObject;
    }

    void setAnswers() {
        for (int i = 0; i < answersText.Length; i++)
        {
            answersText[i].text = (char)(i+65) + ". " + QAndA[currentQuestion].answers[i];
        }
    }

    void generateQuestion() {
        if (QAndA.Count == currentQuestion+1 || players.Count == 0) {
            ending();
            return;
        } 

        endOfTheRound = false;
        currentQuestion += 1;//Random.Range(0, QAndA.Count);

        questionText.text = QAndA[currentQuestion].question;
        setAnswers();

        timer.GetComponent<Timer>().restart();

        for (int i = 0; i < players.Count; i++)
        {
            players[i].GetComponent<PlayerScript>().startQuestion();
        }
    }

    void Update() {
        if (timer.GetComponent<Timer>().isOver() && !endOfTheRound) {
            endOfTheRound = true;

            for (int i = 0; i < players.Count; i++)
            {
                players[i].GetComponent<PlayerScript>().defaultAnswer();
            }

            StartCoroutine(checkAnswer());
        }
    }

    IEnumerator checkAnswer() {
        yield return new WaitForSeconds(3);

        lift.GetComponent<Animator>().Play("lift", 0, 0.0f);
        yield return new WaitForSeconds(1);
        BG.GetComponent<BGmanager>().startRoutine();

        answerBlock[QAndA[currentQuestion].correctAnswer%2].Play("open", 0, 0.0f);
        yield return new WaitForSeconds(1.5f);
        answerBlock[QAndA[currentQuestion].correctAnswer%2].Play("close", 0, 0.0f);

        for (int i = 0; i < players.Count; i++)
        {
            if(players[i].GetComponent<PlayerScript>().getAnswer() == QAndA[currentQuestion].correctAnswer) {
                Debug.Log("Player " + (i+1) + " has the correct Answer");
                score += 1.0f/(float)players.Count;
                //+1 point au score
            }
            else {
                Debug.Log("Player " + (i+1) + " has the wrong Answer");
                players[i].GetComponent<PlayerScript>().wrongAnswer();
                players[i].transform.parent = null;
                if (players[i].GetComponent<PlayerScript>().isDead()) {
                    players.RemoveAt(i);
                    i--;
                }
                //players[i] is hurting and losing a life
            }
        }

        yield return new WaitForSeconds(3);

        for (int i = 0; i < players.Count; i++)
        {
            players[i].transform.parent = playerParent.transform;
        }

        //QAndA.RemoveAt(currentQuestion);

        generateQuestion();
    }

    void ending() {
        Debug.Log("Score de la partie : " + score + "/" + nbQuestions);

        if (players.Count == 0)
        {
            Debug.Log("Score insuffisant...");
            GameManager.instance.load_scene("WaterGameEnd");
        }
        else
        {
            GameManager.instance.set_finished_game(0);
            GameManager.instance.load_scene("WaterGameEndBis");
        }
        //Là c'est la fiiiin.
        
    }
}
