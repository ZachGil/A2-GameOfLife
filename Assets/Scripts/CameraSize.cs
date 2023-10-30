using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSize : MonoBehaviour
{
    public Camera cam;
    public float zoom;
    public float minZoom;
    public float maxZoom;

    public void ZoomIn()
    {
        float newSize = cam.orthographicSize - zoom;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

    public void Zoomouut()
    {
        float newSize = cam.orthographicSize + zoom;
        cam.orthographicSize = Mathf.Clamp(newSize, minZoom, maxZoom);
    }

}
