using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Camera Camera;

    private Vector2 _offset;
    // Start is called before the first frame update
    void Start()
    {
        _offset = Camera.transform.position - transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        var v2 = (Vector2)transform.position + _offset;
        Camera.transform.position = new Vector3(v2.x, v2.y, Camera.transform.position.z);
    }
}
