using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    RectTransform scene_transition_prefab;
    RectTransform scene_transition;

    void Start()
    {
        scene_transition = Instantiate(scene_transition_prefab);
        scene_transition.SetParent(transform);
        scene_transition.anchoredPosition = Vector2.zero;
        scene_transition.localScale = new Vector3(5, 5, 5);
        LeanTween.alpha(scene_transition, 1, 0);
        LeanTween.alpha(scene_transition, 0, 1.5f).setOnComplete(() =>
        {
            Destroy(scene_transition.gameObject);
        });
    }
}
