using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GodSpell : Spell
{
    Transform boss;

    [SerializeField]
    float speed;

    [SerializeField]
    GameObject god_spell_death_particles;

    GameObject main;
    ParticleSystem sparkles_ps, trail_ps, front_ps;

    void Start()
    {
        boss = GameObject.FindGameObjectWithTag("Boss").transform;
        transform.LookAt(boss);
        main = transform.Find("Main").gameObject;
        front_ps = transform.Find("Front").gameObject.GetComponent<ParticleSystem>();
        sparkles_ps = transform.Find("Sparkles").gameObject.GetComponent<ParticleSystem>();
        trail_ps = transform.Find("Trail").gameObject.GetComponent<ParticleSystem>();
    }

    void Update()
    {
        if (boss != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, boss.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Boss")
        {
            Enemy other_enemy = other.GetComponent<Enemy>();
            other_enemy.current_health -= damage;
            Instantiate(god_spell_death_particles, transform.position, Quaternion.identity);
            Destroy(main);
            var front_main = front_ps.main;
            front_main.loop = false;
            var sparkles_main = sparkles_ps.main;
            sparkles_main.loop = false;
            var trail_main = trail_ps.main;
            trail_main.loop = false;
        }
    }

    
}
