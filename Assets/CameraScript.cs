using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
	private Camera camera;
    public float wasdSpeed = 10;
    public float mouseSpeed = 0.01f;
    public float zoomSpeed = 4f;
    public int mouseDragButton = 2;
    private Vector2 pastMouse;
    // Start is called before the first frame update
    void Start()
    {
        pastMouse = Input.mousePosition;
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 deltaMouse = pastMouse - new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        float dt = Time.deltaTime;
        camera.orthographicSize = camera.orthographicSize + (Input.GetAxis("Mouse ScrollWheel") * zoomSpeed);
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            moveAlongPlane(Input.GetAxis("Horizontal") * dt * wasdSpeed, Input.GetAxis("Vertical") * dt * wasdSpeed);
        }
        if (Input.GetMouseButton(mouseDragButton))
        {
            moveAlongPlane(deltaMouse * mouseSpeed);
        }
        pastMouse = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
    }
    void moveAlongPlane(float x, float y)
    {
        transform.Translate(new Vector3(x, y, 0));
    }
    void moveAlongPlane(Vector2 vec)
    {
        transform.Translate(new Vector3(vec.x, vec.y, 0));
    }
}