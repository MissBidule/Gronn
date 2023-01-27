using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slides : MonoBehaviour
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
    ColorBlock btn_colors;

    void Start()
    {
        btn_colors = btn.GetComponent<Button>().colors;
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
                btn.anchoredPosition = new Vector2(725, -360);
                btn_colors.normalColor = new Color(0.6431373f, 0.7607843f, 0.3686275f, 1.0f);
                btn_colors.highlightedColor = new Color(0.9921569f, 0.7411765f, 0.007843138f, 1.0f);
                text.text = "Cher habitant de la planète Terre, l'état de santé de notre belle maison se détériore de jour en jour... Il est temps de réagir !\nLe changement climatique devient plus en plus problématique pour sa survie et celle de ses habitants.";
                btn_text.text = "Suivant";
            }
            if (index == 2)
            {
                text.text = "Je vais t'expliquer rapidement ce qu'est le changement climatique...";
            }
            if (index == 3)
            {
                text.text = "";
            }
            if (index == 6)
            {
                text.text = "Maintenant que tu as conscience des enjeux du changement climatique, il est temps pour toi de partir à l'aventure et d'aider les divinités de la nature.\n\nAutour de la planète, des indications et informations sur la Terre peuvent t'aider dans ta quête !";
                btn_text.text = "C'est parti !";
            }
            if (index < slides.Count)
            {
                coroutine = change_timer(slide_time);
                StartCoroutine(coroutine);
            }
            else
            {
                GameManager.instance.load_scene("EarthScene");
            }
        }
    }
}
