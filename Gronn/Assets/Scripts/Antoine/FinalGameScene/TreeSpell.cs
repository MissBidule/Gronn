using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeSpell : Spell
{
    [SerializeField]
    float life_time;

    [SerializeField]
    GameObject tree_death_particles;

    void Start()
    {
        Invoke("destroy", life_time);
    }

    private void OnTriggerEnter(Collider other)
    {
        damage_text.transform.GetChild(0).GetComponent<Text>().text = damage.ToString();
        if (other.tag == "Enemy")
        {
            other.GetComponent<Enemy>().current_health -= damage;
            Instantiate(damage_text, other.transform.position, Quaternion.identity);
        }
    }

    void destroy()
    {
        Instantiate(tree_death_particles, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
