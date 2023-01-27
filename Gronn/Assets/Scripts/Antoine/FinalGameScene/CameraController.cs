using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Transform player;

    public Vector3 offset;

    [SerializeField]
    float smooth_time;

    Vector3 velocity = Vector3.zero;

    void Start()
    {
        offset = transform.position - player.position;
    }

    void Update()
    {
        Vector3 target_position = player.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, target_position, ref velocity, smooth_time);
        transform.LookAt(player);
    }
}
