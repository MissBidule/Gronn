using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndAirGame : MonoBehaviour
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
           if (index < slides.Count)
            {                
                coroutine = change_timer(slide_time);
                StartCoroutine(coroutine);
            }
            else
            {
                GameManager.instance.load_scene("EarthScene");
                GameManager.instance.set_finished_game(1);
            }
        }
    }
}
