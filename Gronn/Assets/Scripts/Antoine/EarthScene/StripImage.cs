using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StripImage : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().sprite = GameManager.instance.strip_sprites[GameManager.instance.nb_finished_games];
    }

}
