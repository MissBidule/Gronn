using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField]
    Transform player;

    Vector3 random_point;
    NavMeshHit hit;

    [SerializeField]
    float safe_spawn_distance;

    [SerializeField]
    GameObject enemy_prefab, strong_enemy_prefab, fire_tree_prefab, waste_prefab, boss_prefab;

    [SerializeField]
    int[] nb_total_enemy, nb_enemy_wave, nb_strong_enemy_wave, nb_fire_tree_wave;
    int nb_enemy, nb_strong_enemy, nb_fire_tree, nb_boss = 0;

    [SerializeField]
    float wave_timer;
    public int current_wave = -1;
    bool can_change_wave = true;

    public int nb_waste_alive = 0;

    [SerializeField]
    float enemy_spawn_time, strong_enemy_spawn_time, fire_tree_spawn_time;
    bool can_spawn_enemy = true;
    bool can_spawn_strong_enemy = true;
    bool can_spawn_fire_tree = true;

    [SerializeField]
    GameObject boss_health_bar_prefab;
    GameObject boss_health_bar;
    GameObject boss;

    [SerializeField]
    Text wave_text;

    void Start()
    {
        StartCoroutine(next_wave(wave_timer));
    }

    void Update()
    {
        if (current_wave >= 0 && current_wave < 4)
        {
            if (nb_enemy < nb_enemy_wave[current_wave] && can_spawn_enemy)
            {
                spawn_enemy(enemy_prefab, 0);
            }

            if (nb_strong_enemy < nb_strong_enemy_wave[current_wave] && can_spawn_strong_enemy)
            {
                spawn_enemy(strong_enemy_prefab, 1);
            }

            if (nb_fire_tree < nb_fire_tree_wave[current_wave] && can_spawn_fire_tree)
            {
                spawn_enemy(fire_tree_prefab, 2);
            }

            if (current_wave == 3 && nb_boss == 0 && nb_enemy == nb_enemy_wave[current_wave] && nb_strong_enemy == nb_strong_enemy_wave[current_wave] && nb_fire_tree == nb_fire_tree_wave[current_wave])
            {
                while (nb_boss == 0)
                {
                    spawn_boss();
                }
            }

            if (boss != null)
            {
                boss_health_bar.GetComponent<Slider>().value = boss.GetComponent<Enemy>().current_health;
                while (nb_waste_alive < 3 && player.GetComponent<PlayerController>().nb_waste == 0)
                {
                    spawn_waste();
                }
            }
            else if (boss_health_bar)
            {
                Destroy(boss_health_bar);
            }

            if (nb_enemy == nb_enemy_wave[current_wave] && nb_strong_enemy == nb_strong_enemy_wave[current_wave] && nb_fire_tree == nb_fire_tree_wave[current_wave])
            {
                if (can_change_wave && GameObject.FindGameObjectsWithTag("Enemy").Length == 0 && current_wave < 3)
                {
                    can_change_wave = false;
                    StartCoroutine(next_wave(wave_timer));
                } else if (current_wave == 3 && boss == null)
                {
                    end_level();
                }
             }
        }
    }

    void spawn_enemy(GameObject enemy, int enemy_type)
    {
        random_point = new Vector3(Random.Range(-41, 38), 11, Random.Range(-23, 31));

        if (NavMesh.SamplePosition(random_point, out hit, 1, NavMesh.AllAreas))
        {
            float dist = Vector3.Distance(player.position, random_point);
            if (dist > safe_spawn_distance)
            {
                Instantiate(enemy, random_point, Quaternion.identity);
                Instantiate(enemy.GetComponent<Enemy>().spawn_particles, random_point, Quaternion.identity);
                StartCoroutine(spawn_timer(enemy_type));
                if (enemy_type == 0)
                {
                    nb_enemy++;
                } 
                else if (enemy_type == 1)
                {
                    nb_strong_enemy++;
                }
                else if (enemy_type == 2)
                {
                    nb_fire_tree++;
                }
            } else
            {
                spawn_enemy(enemy, enemy_type);
            }
        }
    }

    void spawn_waste()
    {
        random_point = new Vector3(Random.Range(-41, 38), 11f, Random.Range(-23, 31));

        if (NavMesh.SamplePosition(random_point, out hit, 1, NavMesh.AllAreas))
        {
            float dist = Vector3.Distance(player.position, random_point);
            if (dist > safe_spawn_distance)
            {
                Instantiate(waste_prefab, random_point, Quaternion.identity);
                Instantiate(waste_prefab.GetComponentInChildren<Waste>().spawn_particles, random_point, Quaternion.identity);
                nb_waste_alive++;
            }
        }
    }
    void spawn_boss()
    {
        random_point = new Vector3(Random.Range(-41, 38), 11, Random.Range(-23, 31));

        if (NavMesh.SamplePosition(random_point, out hit, 1, NavMesh.AllAreas))
        {
            float dist = Vector3.Distance(player.position, random_point);
            if (dist > safe_spawn_distance)
            {
                boss = Instantiate(boss_prefab, random_point, Quaternion.identity);
                Instantiate(boss.GetComponent<Enemy>().spawn_particles, random_point, Quaternion.identity);
                boss_health_bar = Instantiate(boss_health_bar_prefab);
                boss_health_bar.transform.SetParent(GameObject.Find("Canvas").transform);
                boss_health_bar.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 40);
                boss_health_bar.GetComponent<Slider>().maxValue = boss.GetComponent<Enemy>().max_health;
                nb_boss++;
            }
        }
    }


    public IEnumerator spawn_timer(int enemy_type)
    {
        if (enemy_type == 0)
        {
            can_spawn_enemy = false;

            yield return new WaitForSeconds(enemy_spawn_time);

            can_spawn_enemy = true;
        } 
        else if (enemy_type == 1)
        {
            can_spawn_strong_enemy = false;

            yield return new WaitForSeconds(strong_enemy_spawn_time);

            can_spawn_strong_enemy = true;
        }
        else if (enemy_type == 2)
        {
            can_spawn_fire_tree = false;

            yield return new WaitForSeconds(fire_tree_spawn_time);

            can_spawn_fire_tree = true;
        }
    }

    public IEnumerator next_wave(float next_wave_timer)
    {
        if (current_wave == -1)
        {
            wave_text.text = "VAGUE 1 \nEnnemis : Petits déchets \n Sorts : Forêt";
        } else if (current_wave == 0)
        {
            wave_text.text = "VAGUE 2 \nEnnemis : Petits déchets, Gros déchets \n Sorts : Forêt, Air";
        } else if (current_wave == 1)
        {
            wave_text.text = "VAGUE 3 \nEnnemis : Petits déchets, Gros déchets, Arbres en feu \n Sorts : Forêt, Air, Eau";
        } else if (current_wave == 2)
        {
            wave_text.text = "VAGUE FINALE \nEnnemis : Petits déchets, Gros déchets, Arbe en feu, Dieu de la pollution \n Sorts : Forêt, Air, Eau, Ultime";
        }
        wave_text.gameObject.SetActive(true);

        yield return new WaitForSeconds(next_wave_timer);

        wave_text.gameObject.SetActive(false);

        if (current_wave < 3)
        {
            current_wave++;
            nb_enemy = 0;
            nb_strong_enemy = 0;
            can_change_wave = true;
        }
    }

    void end_level()
    {
        GameManager.instance.load_scene("EndScene");
        GameManager.instance.set_finished_game(4);
    }
}
