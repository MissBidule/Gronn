using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGmanager : MonoBehaviour
{
    public float speed;
    public float height;

    private bool play = false;
    private float previousY;

    public void startRoutine() {
        play = true;
        previousY = transform.position.y;
    }

    void Update() {
        if (play) {
            transform.Translate(Vector3.up * speed * Time.deltaTime);
            if (transform.position.y > previousY + height) {
                play = false;
            }
        }
    }
}
