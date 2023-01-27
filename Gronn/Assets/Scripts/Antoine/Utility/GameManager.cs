using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public List<Sprite> strip_sprites;

    public List<bool> finished_games;
    public int nb_finished_games = 0;

    [SerializeField]
    RectTransform scene_transition_prefab;
    RectTransform scene_transition;
    RectTransform canvas;

    bool can_change_scene = true;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        } else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void set_finished_game(int game_index)
    {
        finished_games[game_index] = true;

        int nb = 0;
        foreach (bool b in finished_games)
        {
            if (b == true)
            {
                nb++;
            }
        }

        nb_finished_games = nb;
    }

    public void load_scene(string scene_name)
    {
        if (can_change_scene == true)
        {
            can_change_scene = false;
            canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
            scene_transition = Instantiate(scene_transition_prefab);
            scene_transition.SetParent(canvas);
            scene_transition.anchoredPosition = Vector2.zero;
            LeanTween.scale(scene_transition, Vector3.zero, 0);
            LeanTween.scale(scene_transition, new Vector3(5, 5, 5), 1f).setEase(LeanTweenType.easeInOutExpo).setOnComplete(() => {
                SceneManager.LoadScene(scene_name);
                can_change_scene = true;
            });
        }
    }
}
