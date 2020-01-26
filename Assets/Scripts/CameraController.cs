using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script to control the camera, smoothly goes to positions
public class CameraController : MonoBehaviour
{
    public float zoomAmmount = 20;
    public float zoomSmooth = 0.125f;

    public float moveAmmount = 50;
    public float moveSmooth = 10f;

    public float maxZoom;
    public float minZoom;

    private Vector2 desired2d;

    private float desiredZoom;

    // Start is called before the first frame update
    void Start()
    {
        desired2d = position2d;
        desiredZoom = zoom;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float smoothUpdateMove = moveSmooth * Time.deltaTime;
        position2d = Vector2.Lerp(position2d, desired2d, smoothUpdateMove);


        float smoothUpdateZoom = zoomSmooth * Time.deltaTime;
        zoom = Mathf.Lerp(zoom, desiredZoom, smoothUpdateZoom);
    }

    private Vector2 position2d {
        get {return new Vector2(transform.position.x, transform.position.z); }
        set { Vector3 position = transform.position; position.x = value.x; position.z = value.y; transform.position = position; }
    }

    private float zoom
    {
        get { return transform.position.y; }
        set { Vector3 position = transform.position; position.y = value; transform.position = position; }
    }

    public void ZoomIn()
    {
        desiredZoom -= zoomAmmount;
        desiredZoom = Mathf.Clamp(desiredZoom, minZoom, maxZoom);
    }

    public void ZoomOut()
    {
        desiredZoom += zoomAmmount;
        desiredZoom = Mathf.Clamp(desiredZoom, minZoom, maxZoom);
    }

    public void MoveUp()
    {
        MoveIn2dDir(Vector2.up * moveAmmount);
    }

    public void MoveDown()
    {
        MoveIn2dDir(Vector2.down * moveAmmount);
    }

    public void MoveLeft()
    {
        MoveIn2dDir(Vector2.left * moveAmmount);
    }

    public void MoveRight()
    {
        MoveIn2dDir(Vector2.right * moveAmmount);
    }

    public void MoveIn2dDir(Vector2 dir)
    {
        desired2d += dir * Time.deltaTime * (zoom / 100);
    }

}
