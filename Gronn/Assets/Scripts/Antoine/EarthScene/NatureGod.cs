using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NatureGod : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    EarthController earth_controller;

    [SerializeField]
    Text nature_god_text;

    [SerializeField]
    float min_alpha, max_alpha, alpha_speed;
    Image img;
    float current_alpha;

    bool is_hovering = false;

    [SerializeField]
    Transform canvas;

    Button close_btn;

    [SerializeField]
    GameObject nature_god_info_prefab;
    public GameObject nature_god_info;

    void Start()
    {
        img = GetComponent<Image>();
        current_alpha = img.color.a;
    }

    void Update()
    {
        if (is_hovering == true && current_alpha < max_alpha && earth_controller.earth_rotating_target == false)
        {
            current_alpha += alpha_speed;
            
            if (current_alpha > max_alpha)
            {
                current_alpha = max_alpha;
            }
            
        } else if (is_hovering == false && current_alpha > min_alpha)
        {
            current_alpha -= alpha_speed;
            if (current_alpha < min_alpha)
            {
                current_alpha = min_alpha;
            }
        }

        if (img.color.a != current_alpha)
        {
            img.color = new Color(1, 1, 1, current_alpha);
            nature_god_text.color = new Color(1, 1, 1, current_alpha);
        }

        if (is_hovering && Input.GetMouseButtonDown(0) && earth_controller.earth_rotating_target == false && nature_god_info == null)
        {
            nature_god_info = Instantiate(nature_god_info_prefab);
            nature_god_info.transform.SetParent(canvas);
            nature_god_info.GetComponent<RectTransform>().anchoredPosition = new Vector2(-375, 150);
            close_btn = nature_god_info.transform.Find("CloseButton").GetComponent<Button>();
            close_btn.onClick.AddListener(close_nature_god_info);
        }

        if (nature_god_info != null && is_hovering == false)
        {
            is_hovering = true;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        is_hovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        is_hovering = false;
    }

    void close_nature_god_info()
    {
        is_hovering = false;
        Destroy(nature_god_info);
        nature_god_info = null;
    }
}
