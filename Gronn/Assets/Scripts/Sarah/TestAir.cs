using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TestAir : MonoBehaviour
{
    [SerializeField]
    Image img;

    [SerializeField]
    List<Sprite> slides;

    [SerializeField]
    float slide_time;

    [SerializeField]
    Text text, btn_text,goodanswer_text,badanswer_text,bofbof_text;

    [SerializeField]
    GameObject btn,bofbof, goodanswer, badanswer, questionText;
    RectTransform btnRectTransform,bofbofRectTransform,goodanswerRectTransform,badanswerRectTransform, questionTextRectTransform;

    // [SerializeField]
    // int nbQuestion=7;
    // int numQestion=0;
    // int numReponse=0;
    // int sceptisisme=6;
    // int agacement=0;
    // string[]questions=new string[]{"On parle de réchauffement climatique mais bon aux États-Unis cette année t’as vu comme ils ont gelé ! Ça me fait bien rire.","Bon, ok mais le changement climatique c'est pas nouveau. C’est naturel et il y a en a eu d’autres au cours de l’histoire de la Terre. C’est par rapport aux cycles solaires.","Après … quelques degrés de plus ou de moins… ","Dans ce cas là, comme les arbres absorbent du CO2, il suffit de reboiser pour annuler l’effet de serre.","Faut pas autant dramatiser, dès qu’on arrêtera d’émettre quand on sera passé aux énergies renouvelables la situation reviendra à la normale.","Donc il est trop tard, autant profiter. De toute façon, ce n'est pas moi qui vais changer le monde.","Oui mais c’est surtout aux pays comme la Chine de faire des efforts."};
    // string[]bad=new string[]{"C'était tout le contraire deux étés plus tôt...","Ça n'a rien à voir avec le Soleil et c'est prouvé ...","ça fait quoi du coup quelques degrés en plus selon toi ?","C'est un solution de facilité ...,","C'est très mal comprendre l'effet de serre...","C'est lâche ça ...","Donc nous on fait rien ?"};
    // string[]bof=new string[]{"Je crois que ça n'empêche pas qu'il y ait un réchauffement général ...","Celui là est différent parce qu'il est plus rapide...","","Non c'est pas possible parce que ...","ça pourrait mais on est encore complètement dépendant aux énergies fossiles et les gaz à effet de serre mettent du temps à être éliminés...","C'est pas trop tard pour nos enfants ...","Historiquement nous avons une part plus importante ..."};
    // string[]good=new string[]{"Alors c'est vrai que c'est contre-intuitif mais c'est une question de différence de température...","Y a bien des cycles climatiques naturels mais ...","Au contraire quelques degrés de moyenne, ça change tout ! Par exemple ...","C'est une solution efficace mais pas suffisante...","J'avoue que ça en à l'air mais je dramatise pas ...","En réalité tu peux faire quelque chose...","Les efforts des pays européens comptent..."};

    public List<QnA2> QAndA;
    public int index;
    bool can_change_slide = false;
    IEnumerator coroutine;
    ColorBlock btn_colors;

    // Start is called before the first frame update
    void Start()
    {   
        btn_colors = btn.GetComponent<Button>().colors;
        btnRectTransform = btn.GetComponent<RectTransform>();
        bofbofRectTransform = bofbof.GetComponent<RectTransform>();
        goodanswerRectTransform = goodanswer.GetComponent<RectTransform>();
        badanswerRectTransform = badanswer.GetComponent<RectTransform>();

        index = 0;
        coroutine = change_timer(0);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if(index<slides.Count){
        img.sprite = slides[index];
        }

    }

    // void LoadQuestion()
    // {
    //     for(int i=0;i<nbQuestion;i++){
    //         if(numQestion==i)
    //         {
    //             questionTextRectTransform=questions[i];
    //             badanswer_text=bad[i];
    //             bofbof_text=bof[i];
    //             goodanswer_text=good[i];
    //         }
    //     }
    // }

    public IEnumerator change_timer(float wait_time)
    {
        can_change_slide = false;

        yield return new WaitForSeconds(wait_time);

        can_change_slide = true;
    }
    public void change_slide()
    {
        if (can_change_slide)
        {
            index++;
            if (index == 0)
            {                
            }
            if (index == 1)
            {  
                text.text = "ici on va mettre une bd du pb de pollution";
            }
            if (index == 2)
            {

                
            }
           if (index < slides.Count)
            {                
                coroutine = change_timer(slide_time);
                StartCoroutine(coroutine);
            }
            else
            {
                SceneManager.LoadScene("EarthScene");
            }
        }
    }
}
