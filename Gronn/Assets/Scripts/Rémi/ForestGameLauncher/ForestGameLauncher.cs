using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ForestGameLauncher : MonoBehaviour
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
        text.text = "C'est un désastre... 17 % des forêts tropicales humides ont disparu depuis 1990 ce qui a détruit l'habitat de nombreuses espèces et libéré énormément de CO2.\nEt tout ça est dû pour 80% à l'agriculture ...";
    }

    // Update is called once per frame
    void Update()
    {
        if(index<slides.Count)
        {
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
            else if (index == 1)
            {   
                text.text = "Mais bon retroussons-nous les manches, on va inverser la tendance !\n\nTu dois donc ramener ton exploitation agricole en forêt tropicale dans l’accord de Paris.\nTu disposes de 100 ans et gagneras si tu ne dépasses pas un budget carbone en tonnes de CO2 à émettre au cours de la partie.\nPour cela, tu auras besoin de laisser de la place à la jungle pour se reconstituer, tout en continuant de nourrir ta population avec ton agriculture.";
            }
            else if (index == 2)
            {   
                text.text = "Utilise Z (haut), S (bas), Q (gauche), D (droite) pour te déplacer \n (avec SHIFT GAUCHE appuyé pour une vitesse rapide).\nA et E pour faire tourner le monde.\nLa molette de la souris ou les touches + et - pour zoomer.\nEt enfin clic gauche pour construire/détruire.\n\nTu peux faire un clic droit sur chaque case pour connaitre son état et ses caractéristiques.\n\nAttention !! Si tu perds plus de 5 % de ta population en 1 année, tu subiras une révolte et perdras la partie.";
            }
           else if (index < slides.Count)
            {
                coroutine = change_timer(slide_time);
                StartCoroutine(coroutine);
            }
            else
            {
                SceneManager.LoadScene("ForestGameScene");
            }
        }
    }

    public void thisIsTheEnd()
    {
        SceneManager.LoadScene("EarthScene");
    }
}
