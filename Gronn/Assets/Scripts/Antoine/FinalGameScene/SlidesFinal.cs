using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlidesFinal : MonoBehaviour
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
    RectTransform btn;

    int index;
    bool can_change_slide = false;
    IEnumerator coroutine;

    void Start()
    {
        index = 0;
        coroutine = change_timer(0);
        StartCoroutine(coroutine);
    }

    void Update()
    {
        if (index < slides.Count)
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
            if (index == 1)
            {
                text.text = "Dans ce ce jeu style Shoot them up, tu vas devoir survivre à plusieurs vagues d'ennemis. Il te suffira d'utiliser les flèches directionnelles pour te déplacer sur l'île. \n\nPour récompenser ton travail précédent les dieux ont décidé de t'aider en te donnant des sorts qui vont te permettre de survivre aux différentes vagues.\n\nLorsque tu auras accès au sort de l'eau, appuie sur la barre d'espace pour l'utiliser !";
            }
            if (index == 2)
            {
                text.text = "Le dieu de la pollution fera son apparition lors de la dernière vague, il faudra que tu ramasses les déchets pour faire appel aux dieux afin de le vaincre. \n\nAlors es-tu prêt(e) à relever ce défi ?";
                btn_text.text = "C'est parti !";
            }
            if (index < slides.Count)
            {
                coroutine = change_timer(slide_time);
                StartCoroutine(coroutine);
            }
            else
            {
                GameManager.instance.load_scene("FinalGameScene");
            }
        }
    }
}
