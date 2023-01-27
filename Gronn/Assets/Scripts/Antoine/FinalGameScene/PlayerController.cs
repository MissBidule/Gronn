using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Slider health_bar_slider;

    public int max_health;
    public int current_health;

    [SerializeField]
    Spawner spawner;

    [SerializeField]
    List<ParticleSystem> water_particles;

    [SerializeField]
    CameraController camera_controller;

    [SerializeField]
    float speed = 3.0f;

    [SerializeField]
    float rotate_speed = 3.0f;

    CharacterController controller;

    [SerializeField]
    GameObject tree_spell_prefab, tornado_spell_prefab, god_spell_prefab;

    [SerializeField]
    float tree_spell_cooldown;
    bool can_tree_spell = true;

    [SerializeField]
    float tornado_spell_cooldown;
    bool can_tornado_spell = true;

    bool change_loop = true;

    public int nb_waste = 0;

    Animator animator;
    //bool can_regen = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
        current_health = max_health;
        health_bar_slider.maxValue = max_health;
    }

    void Update()
    {
        /*
        if (can_regen && current_health < max_health)
        {
            StartCoroutine(regen_life());
        }*/
        health_bar_slider.value = current_health;

        if (current_health <= 0)
        {
            speed = 0;
            rotate_speed = 0;
            GameManager.instance.load_scene("EarthScene");
        }

        transform.Rotate(0, Input.GetAxis("Horizontal") * rotate_speed, 0);
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        float cur_speed = speed * Input.GetAxis("Vertical");
        controller.SimpleMove(forward * cur_speed);

        animator.SetBool("Grounded", controller.isGrounded);
        animator.SetFloat("MoveSpeed", cur_speed);

        if (can_tree_spell == true &&spawner.current_wave > -1)
        {
            StartCoroutine(tree_spell(tree_spell_cooldown));
        }

        if (can_tornado_spell == true && spawner.current_wave > 0)
        {
            StartCoroutine(tornado_spell(tornado_spell_cooldown));
        }

        if (Input.GetKey("space") && spawner.current_wave > 1)
        {
            if (change_loop == true)
            {
                foreach (ParticleSystem ps in water_particles)
                {
                    ps.Play(true);
                    var ps_main = ps.main;
                    ps_main.loop = true;
                }
                change_loop = false;
            }
        }
        else
        {
            if (change_loop == false)
            {
                foreach (ParticleSystem ps in water_particles)
                {
                    var ps_main = ps.main;
                    ps_main.loop = false;
                }
                change_loop = true;
            }
        }

        if (nb_waste == 3)
        {
            StartCoroutine(god_spell());
            nb_waste = 0;
        }
    }

    public IEnumerator tree_spell(float wait_time)
    {
        Instantiate(tree_spell_prefab, new Vector3(transform.position.x, 9.9f, transform.position.z), Quaternion.identity);

        can_tree_spell = false;
       
        yield return new WaitForSeconds(wait_time);

        can_tree_spell = true;
    }

    public IEnumerator tornado_spell(float wait_time)
    {
        Instantiate(tornado_spell_prefab, new Vector3(transform.position.x, 9.9f, transform.position.z) + transform.forward * 2, Quaternion.identity);

        can_tornado_spell = false;

        yield return new WaitForSeconds(wait_time);

        can_tornado_spell = true;
    }

    public IEnumerator god_spell()
    {
        camera_controller.offset.x += 20;
        camera_controller.offset.y += 10;

        Instantiate(god_spell_prefab, new Vector3(Random.Range(-30, 30), 70, Random.Range(-30, 30)), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Instantiate(god_spell_prefab, new Vector3(Random.Range(-30, 30), 70, Random.Range(-30, 30)), Quaternion.identity);

        yield return new WaitForSeconds(0.5f);

        Instantiate(god_spell_prefab, new Vector3(Random.Range(-30, 30), 70, Random.Range(-30, 30)), Quaternion.identity);

        yield return new WaitForSeconds(1f);

        camera_controller.offset.x -= 20;
        camera_controller.offset.y -= 10;

        yield return new WaitForSeconds(2f);

        spawner.nb_waste_alive = 0;
    }
    /*
    public IEnumerator regen_life()
    {
        current_health += 1;
        if (current_health > max_health)
        {
            current_health = max_health;
        }

        can_regen = false;

        yield return new WaitForSeconds(1);

        can_regen = true;
    }*/
}
