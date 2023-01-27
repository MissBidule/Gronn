using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EarthController : MonoBehaviour
{
    [SerializeField]
    Camera main_camera;

    [SerializeField]
    float zoom_min, zoom_max, rotation_speed;
    public float grab_force;

    [SerializeField]
    GameObject earthPosition;
    [SerializeField]
    List<GameObject> earthList;
    Rigidbody earth_rb;
    Vector3 mouse_direction;
    Vector3 defaultScale = new Vector3(50,50,50);

    int index;
    [SerializeField]
    GameObject earth;
    
    Ray ray;
    RaycastHit hit;
    Transform target;

    [SerializeField]
    GameObject short_info_prefab;
    GameObject short_info;
    Vector3 screen_pos;

    public bool can_display_short_info = true;

    [SerializeField]
    RectTransform canvas;

    [SerializeField]
    float rotate_target_speed;

    Transform clicked_target;
    Vector3 target_direction;
    Vector3 camera_direction;
    Vector3 directions_perp;
    float directions_angle;

    public bool earth_rotating_target = false;

    [SerializeField]
    GameObject long_info_play_prefab, long_info_prefab;
    GameObject long_info;

    Button btn;
    Button play_btn;

    bool can_display_long_info = true;
    bool shift = false;

    [SerializeField]
    NatureGod nature_god;

    void Start()
    {
        target = null;
        selectPlanet();
        earth_rb = earth.GetComponent<Rigidbody>();
    }

    void Update()
    {
        cheat_code();
        ray = main_camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity) && earth_rotating_target == false && nature_god.nature_god_info == null)
        {
            target = hit.collider.transform;
            Info target_info = target.GetComponent<Info>();
            if (target.gameObject.layer == 6) //6 corresponds to info layer
            {
                if (can_display_short_info)
                {
                    screen_pos = main_camera.WorldToScreenPoint(target.position);
                    short_info = Instantiate(short_info_prefab);
                    short_info.transform.SetParent(canvas, false);
                    short_info.transform.position = screen_pos;
                    short_info.transform.Find("ImageMask/Image").GetComponent<Image>().sprite = target_info.img;
                    short_info.transform.Find("TextBackground/Text").GetComponent<Text>().text = target_info.short_desc;
                    can_display_short_info = false;
                }
            }
            else
            {
                target = null;
                Destroy(short_info);
                can_display_short_info = true;
            }
        }
        else
        {
            target = null;
            Destroy(short_info);
            can_display_short_info = true;
        }

        mouse_direction = new Vector3(Input.GetAxis("Mouse Y"), -Input.GetAxis("Mouse X"), 0f);
        if (Input.GetMouseButton(0) && earth_rotating_target == false && nature_god.nature_god_info == null)
        {
            can_display_short_info = false;
            earth_rb.AddTorque(mouse_direction * grab_force * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (target)
            {
                clicked_target = target;
                earth_rotating_target = true;
            }
        }

        if (can_display_short_info)
        {
            earth_rb.angularVelocity = Vector3.zero;
            earth.transform.Rotate(Vector3.up * (rotation_speed * Time.deltaTime));
        } else
        {
            earth_rb.angularVelocity = Vector3.zero;
        }

        if (can_display_short_info)
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0 && main_camera.fieldOfView > zoom_max)
            {
                main_camera.fieldOfView--;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0 && main_camera.fieldOfView < zoom_min)
            {
                main_camera.fieldOfView++;
            }
        }
        
        if (earth_rotating_target && clicked_target)
        {
            rotation_speed = 0;
            target_direction = clicked_target.position - earth.transform.position;
            camera_direction = main_camera.transform.position - earth.transform.position;
            directions_perp = Vector3.Cross(target_direction, camera_direction);
            directions_angle = Vector3.Angle(camera_direction, target_direction);
            earth.transform.RotateAround(earth.transform.position, directions_perp, directions_angle * Time.deltaTime * rotate_target_speed);

            if (directions_angle < 5)
            {
                if (can_display_long_info)
                {
                    if (clicked_target.GetComponent<GameInfo>())
                    {
                        long_info = Instantiate(long_info_play_prefab);
                        play_btn = long_info.transform.Find("PlayButton").GetComponent<Button>();
                        play_btn.onClick.AddListener(on_click_info_play);
                    }
                    else
                    {
                        long_info = Instantiate(long_info_prefab);
                    }

                    long_info.transform.SetParent(canvas);
                    Transform long_info_img = long_info.transform.Find("ImageMask/Image");
                    Info clicked_target_info = clicked_target.GetComponent<Info>();
                    long_info_img.GetComponent<Image>().sprite = clicked_target_info.img;
                    long_info_img.GetComponent<RectTransform>().sizeDelta *= 3;
                    long_info.transform.Find("TextBackground/Text").GetComponent<Text>().text = clicked_target_info.long_desc;
                    btn = long_info.transform.Find("ReturnButton").GetComponent<Button>();
                    btn.onClick.AddListener(on_click_info_disappear);
                    can_display_long_info = false;
                }
            }
        }
        
    }

    public void on_click_info_disappear()
    {
        rotation_speed = 5;
        clicked_target = null;
        earth_rotating_target = false;
        can_display_long_info = true;
        Destroy(long_info);
    }

    public void on_click_info_play()
    {
        string s = clicked_target.GetComponent<GameInfo>().scene_name;
        if(s != "FinalGameLauncher" || GameManager.instance.nb_finished_games == 4)
        {
            GameManager.instance.load_scene(clicked_target.GetComponent<GameInfo>().scene_name);
        }
    }

    void cheat_code()
    {
        if (Input.GetKeyDown(KeyCode.RightShift)) shift = true;
        if (Input.GetKeyUp(KeyCode.RightShift)) shift = false;

        if (shift)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                GameManager.instance.set_finished_game(3);
                GameManager.instance.load_scene("EarthScene");
            }

            if (Input.GetKeyDown(KeyCode.T))
            {
                GameManager.instance.set_finished_game(2);
                GameManager.instance.load_scene("EarthScene");
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.instance.set_finished_game(0);
                GameManager.instance.load_scene("EarthScene");
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                GameManager.instance.set_finished_game(1);
                GameManager.instance.load_scene("EarthScene");
            }
        }
    }
    
    void selectPlanet()
    {
        if (GameManager.instance.nb_finished_games == 0)
        {
            index = 16;
        }
        else if (GameManager.instance.nb_finished_games == 5)
        {
            index = 0;
        }
        else if (GameManager.instance.nb_finished_games == 4)
        {
            index = 1;
        }
        else if (GameManager.instance.nb_finished_games == 3)
        {
            if (!GameManager.instance.finished_games[1])
                index = 2;
            if (!GameManager.instance.finished_games[2])
                index = 8;
            if (!GameManager.instance.finished_games[3])
                index = 5;
            if (!GameManager.instance.finished_games[0])
                index = 11;
        }
        else if (GameManager.instance.nb_finished_games == 2)
        {
            if (GameManager.instance.finished_games[1] && GameManager.instance.finished_games[2])
                index = 15;
            if (GameManager.instance.finished_games[3] && GameManager.instance.finished_games[1])
                index = 12;
            if (GameManager.instance.finished_games[3] && GameManager.instance.finished_games[2])
                index = 3;
            if (GameManager.instance.finished_games[0] && GameManager.instance.finished_games[1])
                index = 9;
            if (GameManager.instance.finished_games[0] && GameManager.instance.finished_games[2])
                index = 6;
            if (GameManager.instance.finished_games[3] && GameManager.instance.finished_games[0])
                index = 14;
        }
        else
        {
            if (GameManager.instance.finished_games[1])
                index = 13;
            if (GameManager.instance.finished_games[2])
                index = 7;
            if (GameManager.instance.finished_games[3])
                index = 4;
            if (GameManager.instance.finished_games[0])
                index = 10;
        }
        //STEAD 32014

        GameObject child = Instantiate(earthList[index], earthPosition.transform.position, earthPosition.transform.rotation);
        child.transform.parent = earth.transform;
        child.transform.localScale = defaultScale;
    }
}

