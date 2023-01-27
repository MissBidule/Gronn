using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AirQuizzManager : MonoBehaviour
{
    public List<QnA2>QAndA;
    public GameObject[]options;
    public int currentQuestion=0;
    public int currentAnswer=0;
    public int scepticismPoints=6;
    public int annoyancePoints=0;
    public string[]congrats;
    public Text congratsText;
    public Text questionText;
    public RectTransform currentLesson; 
    public Text btn1Text;
    public Text btn2Text;
    public Text btn3Text;
    public Button[]answersButton;
    int lastQuestion=0;
    public bool nextQ=false;

    [SerializeField]
    GameObject questionCanvas, goodAnswerCanvas, badAnswerCanvas,happyEnding,sadEnding,tooUpset, nextQuestion, sameQuestion;
    RectTransform questionCanvasRectTransform,goodAnswerCanvasRectTransform,badAnswerCanvasRectTransform,happyEndingRectTransform,sadEndingRectTransform,tooUpsetRectTransform, nextQuestionRectTransform, sameQuestionRectTransform;

    private void Start()
    {
        generateQuestion();
        questionCanvasRectTransform = questionCanvas.GetComponent<RectTransform>();
        goodAnswerCanvasRectTransform = goodAnswerCanvas.GetComponent<RectTransform>();
        badAnswerCanvasRectTransform = badAnswerCanvas.GetComponent<RectTransform>();
        nextQuestionRectTransform=nextQuestion.GetComponent<RectTransform>();
        sameQuestionRectTransform=sameQuestion.GetComponent<RectTransform>();
        happyEndingRectTransform = happyEnding.GetComponent<RectTransform>();
        sadEndingRectTransform=sadEnding.GetComponent<RectTransform>();
        tooUpsetRectTransform=tooUpset.GetComponent<RectTransform>();    
    }

    public void SetCurrentAnswer(int n)
    {
            currentAnswer = n;
            lastQuestion=currentQuestion;

            if(QAndA[currentQuestion].valueAnswer[currentAnswer]==0){
                print("Mauvaise réponse");
                questionCanvas.SetActive(false);
                goodAnswerCanvas.SetActive(false);
                badAnswerCanvas.SetActive(true);
                answersButton[currentAnswer].interactable=false;
                nextQ=false;

            }else if(QAndA[currentQuestion].valueAnswer[currentAnswer]==1){
                print("Réponse Bof");
                if(currentQuestion==1){
                    annoyancePoints=annoyancePoints+1;
                }else if(currentQuestion==3){
                    annoyancePoints=annoyancePoints+1;
                    scepticismPoints=scepticismPoints-1;
                }else if(currentQuestion==4){
                    annoyancePoints=annoyancePoints+1;
                    scepticismPoints=scepticismPoints-1;
                }else if(currentQuestion==5){
                    annoyancePoints=annoyancePoints+1;
                    scepticismPoints=scepticismPoints-1;
                }else if(currentQuestion==6){
                    annoyancePoints=annoyancePoints+1;
                }

                questionCanvas.SetActive(false);
                badAnswerCanvas.SetActive(false);
                goodAnswerCanvas.SetActive(true);
                congratsText.text=congrats[1];
                currentLesson.GetComponent<Image>().sprite=QAndA[currentQuestion].lessons;
                nextQ=true;

            }else if(QAndA[currentQuestion].valueAnswer[currentAnswer]==2){
                print("Bonne réponse");
                scepticismPoints=scepticismPoints-1;
                if(currentQuestion==0){
                    annoyancePoints=annoyancePoints+1;
                }else if(currentQuestion==2){
                    annoyancePoints=annoyancePoints+1;
                }else if(currentQuestion==4){
                    annoyancePoints=annoyancePoints-1;
                }
                
                questionCanvas.SetActive(false);
                badAnswerCanvas.SetActive(false);
                goodAnswerCanvas.SetActive(true);
                congratsText.text=congrats[0];
                currentLesson.GetComponent<Image>().sprite=QAndA[currentQuestion].lessons;
                nextQ=true;

            }
            if(annoyancePoints==4){
                    goodAnswerCanvas.SetActive(false);
                    badAnswerCanvas.SetActive(false);
                    questionCanvas.SetActive(false);
                    badAnswerCanvas.SetActive(false);
                    happyEnding.SetActive(false);
                    tooUpset.SetActive(true);
                }
    }
        


    public void generateQuestion()
    {
        if(nextQ==true)
        {
            currentQuestion++;
        }
        if(currentQuestion>=QAndA.Count){
            if(scepticismPoints>0){
                goodAnswerCanvas.SetActive(false);
                badAnswerCanvas.SetActive(false);
                questionCanvas.SetActive(false);
                happyEnding.SetActive(false);
                tooUpset.SetActive(false); 
                sadEnding.SetActive(true);
            }else{
                goodAnswerCanvas.SetActive(false);
                badAnswerCanvas.SetActive(false);
                questionCanvas.SetActive(false);
                badAnswerCanvas.SetActive(false);
                happyEnding.SetActive(true);
                tooUpset.SetActive(false);
        }}else{

                    btn1Text.GetComponent<Text>().text=QAndA[currentQuestion].answers[0];
                    btn2Text.GetComponent<Text>().text=QAndA[currentQuestion].answers[1];
                    btn3Text.GetComponent<Text>().text=QAndA[currentQuestion].answers[2];

                    if(lastQuestion!=currentQuestion)
                    {
                        foreach(Button b in answersButton)
                        {
                            b.interactable=true;
                        }
                    }
                        
                    goodAnswerCanvas.SetActive(false);
                    badAnswerCanvas.SetActive(false);
                    questionCanvas.SetActive(true);
                    badAnswerCanvas.SetActive(false);
                    happyEnding.SetActive(false);
                    tooUpset.SetActive(false);
                    questionText.text=QAndA[currentQuestion].questions;
            }
    }

    public void hereWeGoAgain()
    {
        GameManager.instance.load_scene("EarthScene");
    }

    public void letsGoo()
    {
        SceneManager.LoadScene("AirGameEnd");
    }
}
