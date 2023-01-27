using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public KeyCode left;
    public KeyCode right;

    public List<GameObject> hearts;

    [SerializeField] private Animator m_animator = null;
    private bool m_wasGrounded;
    private bool m_isGrounded;
    private List<Collider> m_collisions = new List<Collider>();

    public float speed;
    public float minX;
    public float maxX;

    bool canAnswer = false;
    bool isMoving = false;
    bool isReturning = false;
    int answer = 0;
    int life;

    void Start()
    {
        answer = 0;
        life = hearts.Count;
    }

    public int getAnswer() {
        return answer;
    }

    public void startQuestion() {
        answer = 0;
        if (transform.position.x > 0.05) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
                isReturning = true;
                m_animator.SetFloat("MoveSpeed", speed);
        }
        else if (transform.position.x < -0.05) {
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            isReturning = true;
            m_animator.SetFloat("MoveSpeed", speed);
        }
        canAnswer = true;
        isMoving = false;
    }

    public void defaultAnswer() {
        canAnswer = false;
        if (answer == 0) {
            answer = Random.Range(1,3);
            isMoving = true;
            if (answer == 1) {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);
            }
            else
            {
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);
            }

            m_animator.SetFloat("MoveSpeed", speed);
        }
    }

    public void wrongAnswer() {
        var heartRenderer = hearts[hearts.Count-life].GetComponent<Renderer>();
        heartRenderer.material.SetColor("_Color", new Color(0.3676471f, 0.2703287f, 0.2703287f, 1.0f));

        isMoving = false;

        life--;
        //Color : 0.3676471f, 0.2703287f, 0.2703287f, 1.0f

        //falls
    }

    public bool isDead() {
        if (life == 0) {
            GetComponent<CapsuleCollider>().enabled = false;
            return true;
        }
        return false;
    }

    void Update()
    {
        m_animator.SetBool("Grounded", m_isGrounded);

        if (Input.GetKeyDown(left)) {
            if (canAnswer && cantAnswer() != 1) {
                answer = 1;
                isMoving = true;
                isReturning = false;

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, -90, transform.eulerAngles.z);

                m_animator.SetFloat("MoveSpeed", speed);
            } 
        }
        if (Input.GetKeyDown(right)) {
            if (canAnswer && cantAnswer() != 2) {
                answer = 2;
                isMoving = true;
                isReturning = false;

                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 90, transform.eulerAngles.z);

                m_animator.SetFloat("MoveSpeed", speed);

            } 
        }

        if (isReturning) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (transform.position.x < 0.05 && transform.position.x > -0.05) {
                isReturning = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
                m_animator.SetFloat("MoveSpeed", 0);
            }
        }

        if (isMoving) {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
            if (transform.position.x < minX || transform.position.x > maxX) {
                isMoving = false;
                transform.eulerAngles = new Vector3(transform.eulerAngles.x, 180, transform.eulerAngles.z);
                m_animator.SetFloat("MoveSpeed", 0);
            }
        }

        Landing();

        m_wasGrounded = m_isGrounded;

    }

    int cantAnswer() {
        if (transform.position.x >= minX && transform.position.x <= maxX)
            return 0;
        if (transform.position.x > 0)
            return 2;
        return 1;
    }

    private void Landing()
    {
        if (!m_wasGrounded && m_isGrounded)
        {
            m_animator.SetTrigger("Land");
        }

        if (!m_isGrounded && m_wasGrounded)
        {
            m_animator.SetTrigger("Jump");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                if (!m_collisions.Contains(collision.collider))
                {
                    m_collisions.Add(collision.collider);
                }
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        ContactPoint[] contactPoints = collision.contacts;
        bool validSurfaceNormal = false;
        for (int i = 0; i < contactPoints.Length; i++)
        {
            if (Vector3.Dot(contactPoints[i].normal, Vector3.up) > 0.5f)
            {
                validSurfaceNormal = true; break;
            }
        }

        if (validSurfaceNormal)
        {
            m_isGrounded = true;
            if (!m_collisions.Contains(collision.collider))
            {
                m_collisions.Add(collision.collider);
            }
        }
        else
        {
            if (m_collisions.Contains(collision.collider))
            {
                m_collisions.Remove(collision.collider);
            }
            if (m_collisions.Count == 0) { m_isGrounded = false; }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (m_collisions.Contains(collision.collider))
        {
            m_collisions.Remove(collision.collider);
        }
        if (m_collisions.Count == 0) { m_isGrounded = false; }
    }
}
