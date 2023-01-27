using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    int damage;

    public int max_health;
    public int current_health;
    int last_health;

    public NavMeshAgent nav_mesh_agent;

    public Transform player;

    public GameObject spawn_particles;

    [SerializeField]
    Material enemy_hit_mat;
    Material base_mat;
    Renderer mat_renderer;

    bool can_hit_player = true;

    void Start()
    {
        mat_renderer = transform.GetChild(0).GetComponent<Renderer>();
        base_mat = mat_renderer.material;
        current_health = max_health;
        last_health = current_health;
        nav_mesh_agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        nav_mesh_agent.SetDestination(player.position);
        if (last_health != current_health)
        {
            StartCoroutine(enemy_hit());
            last_health = current_health;
        }
        if (current_health <= 0)
        {
            Instantiate(spawn_particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && can_hit_player == true)
        {
            collision.gameObject.GetComponent<PlayerController>().current_health -= damage;
            StartCoroutine(hit_player(0.5f));
        }
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.tag == "Player" && can_hit_player == true)
        {
            
            collision.gameObject.GetComponent<PlayerController>().current_health -= damage;
            StartCoroutine(hit_player(0.5f));
        }
    }

    public IEnumerator enemy_hit()
    {
        mat_renderer.material = enemy_hit_mat;

        yield return new WaitForSeconds(0.15f);

        mat_renderer.material = base_mat;
    }

    public IEnumerator hit_player(float time)
    {
        can_hit_player = false;

        yield return new WaitForSeconds(time);

        can_hit_player = true;
    }
}
