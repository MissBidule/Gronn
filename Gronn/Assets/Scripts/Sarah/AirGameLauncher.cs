using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AirGameLauncher : MonoBehaviour
{
    [SerializeField]
    Image img;

    [SerializeField]
    List<Sprite> slides;

    [SerializeField]
    float slide_time;

    [SerializeField]
    Text text, btn_text;

    [SerializeField]
    GameObject btn;
    RectTransform btnRectTransform;

    public int index;
    bool can_change_slide = false;
    IEnumerator coroutine;
    ColorBlock btn_colors;

    // Start is called before the first frame update
    void Start()
    {   
        btn_colors = btn.GetComponent<Button>().colors;
        btnRectTransform = btn.GetComponent<RectTransform>();

        index = 0;
        coroutine = change_timer(0);
        StartCoroutine(coroutine);
        text.text = "Le changement climatique et en particulier la pollution de l'air prennent de plus en plus d'ampleur.\n\nMalheureusement, aujourd'hui, de moins en moins de citoyens se préoccupent des questions environnementales...  ";

    }

    // Update is called once per frame
    void Update()
    {
        if(index<slides.Count){
        img.sprite = slides[index];
        }

    }

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
                text.text = "Des personnes affirment que le changement climatique n'existe pas et que s'il s'avère qu'il existe, l'Homme n'en ai pas responsable... On les appelle les climatosceptiques ! \n\nCes personnes sont très dangereuses ! En effet, elles nient la réalité et veulent insinuer le doute chez les citoyens. Elles agissent de manière non scientifiques mais idéologiques. \n\nTa mission est de les convaincre que le changement climatique existe ! ";
            }
            if (index == 2)
            {   
                text.text = "Voici les règles du jeu : \n \nTu vas réaliser un débat contre un climatosceptique. Ton objectif est de réussir à le convaincre, avec des arguments fondés, que le changement climatique et la pollution de l'air sont bels et bien réels. \n \nPour remporter le débat, le climatosceptique doit avoir perdu tout son scepticisme. \n \nAttention ! Si tu l'agace trop rapidement, il risque de mettre fin au débat !";
            }
           if (index < slides.Count)
            {
                print("oui");
                
                coroutine = change_timer(slide_time);
                StartCoroutine(coroutine);
            }
            else
            {
                SceneManager.LoadScene("AirGame");
            }
        }
    }

    public void thisIsTheEnd()
    {
        SceneManager.LoadScene("EarthScene");
    }
}
