using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameInfo : Info
{
    public string scene_name;

    [SerializeField]
    int game_index;

    [SerializeField]
    Material game_finished_material;

    void Start()
    {
        if (GameManager.instance.finished_games[game_index] == true)
        {
            GetComponent<MeshRenderer>().material = game_finished_material;
        }
    }
}
