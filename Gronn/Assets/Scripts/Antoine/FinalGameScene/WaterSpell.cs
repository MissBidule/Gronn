using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterSpell : Spell
{
    [SerializeField]
    GameObject water_splash_particles;

    ParticleSystem ps;
    List<ParticleCollisionEvent> collisionEvents;

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        damage_text.transform.GetChild(0).GetComponent<Text>().text = damage.ToString();
        int numCollisionEvents = ps.GetCollisionEvents(other, collisionEvents);

        int i = 0;

        while (i < numCollisionEvents)
        {
            Vector3 pos = collisionEvents[i].intersection;
            if (other.tag == "Enemy")
            {
                other.GetComponent<Enemy>().current_health -= damage;
                
                Instantiate(damage_text, other.transform.position, Quaternion.identity);
            }
            Instantiate(water_splash_particles, pos, Quaternion.identity);
            i++;
        }
    }
}
