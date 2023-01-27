using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SlidesGen : MonoBehaviour
{
    [SerializeField]
    Image img;

    [SerializeField]
    List<Sprite> slides;

    [TextArea]
    public string[] text_slide;

    [SerializeField]
    float slide_time;

    [SerializeField]
    Text text, btn_text;

    [SerializeField]
    RectTransform btn;

    [SerializeField]
    string next_scene;

    [SerializeField]
    GameObject score = null;

    int index;
    bool can_change_slide = false;
    IEnumerator coroutine;
    ColorBlock btn_colors;

    // Start is called before the first frame update
    void Start()
    {
        btn_colors = btn.GetComponent<Button>().colors;
        btn.anchoredPosition = new Vector2(305, -392);
        index = 0;
        coroutine = change_timer(0);
        StartCoroutine(coroutine);
    }

    // Update is called once per frame
    void Update()
    {
        if (index < slides.Count)
        {
            img.sprite = slides[index];
            text.text = text_slide[index];
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
            score.SetActive(false);
            if (index < slides.Count)
            {
                btn_colors.normalColor = new Color(0.6431373f, 0.7607843f, 0.3686275f, 1.0f);
                btn_colors.highlightedColor = new Color(0.9921569f, 0.7411765f, 0.007843138f, 1.0f);
                index++;
                coroutine = change_timer(slide_time);
                StartCoroutine(coroutine);
            }
            else
            {
                SceneManager.LoadScene(next_scene);
            }
        }
    }
}
