using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waste : MonoBehaviour
{
    public GameObject spawn_particles;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<PlayerController>().nb_waste++;
            Instantiate(spawn_particles, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
