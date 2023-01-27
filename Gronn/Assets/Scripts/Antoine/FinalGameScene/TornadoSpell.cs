using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TornadoSpell : Spell
{
    [SerializeField]
    float life_time, speed, radius;

    [SerializeField]
    GameObject tornado_death_particles;

    void Start()
    {
        Invoke("destroy", life_time);
    }

    void Update()
    {
        speed += 0.05f;
        transform.Rotate(0, 1, 0);
        transform.position += transform.forward * Time.deltaTime * speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        damage_text.transform.GetChild(0).GetComponent<Text>().text = damage.ToString();
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().current_health -= damage;
            Instantiate(damage_text, other.transform.position, Quaternion.identity);
            
        } else if (other.tag == "Wall")
        {
            destroy();
        }
    }

    void destroy()
    {
        Instantiate(tornado_death_particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
