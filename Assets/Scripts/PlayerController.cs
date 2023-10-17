using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float walk_Speed;
    public float rotate_Speed;
    public Camera cam;
    public float cameraZoom;

    // Start is called before the first frame update
    void Start()
    {
        //cam.transform.Rotate(70f, 90f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        cam.transform.position = transform.position + new Vector3(-7f-(cameraZoom*0.233f),30f+cameraZoom,1f + cameraZoom * (1/30));

        if (Input.GetKey(KeyCode.W)) {
            transform.position += transform.forward * (Time.deltaTime * walk_Speed);
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Rotate(Vector3.up, rotate_Speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(Vector3.up, -rotate_Speed * Time.deltaTime);

        }
    }
}
