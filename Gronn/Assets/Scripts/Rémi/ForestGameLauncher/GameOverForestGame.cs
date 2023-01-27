using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverForestGame : MonoBehaviour
{
    [SerializeField]
    float slide_time;

    [SerializeField]
    Text tryAgainBtn_text;
    [SerializeField]
    Text leaveBtn_text;

    [SerializeField]
    GameObject btn;
    RectTransform btnRectTransform;

    [SerializeField]
    GameObject leaveBtn;
    RectTransform leaveBtnRectTransform;

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

    public IEnumerator change_timer(float wait_time)
    {
        can_change_slide = false;

        yield return new WaitForSeconds(wait_time);

        can_change_slide = true;
    }
    
    public void leave()
    {
        if (can_change_slide)
        {
            SceneManager.LoadScene("EarthScene");
        }
    }
    public void tryAgain()
    {
        if (can_change_slide)
        {

            SceneManager.LoadScene("ForestGameScene");

        }
    }
}
