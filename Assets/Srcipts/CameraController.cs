using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensityvity = 200f;
    private Transform parent;

    // Start is called before the first frame update
    void Start()
    {
        parent = transform.parent;
        Cursor.lockState = CursorLockMode.Locked;// lock the cursor when game start
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
    }

    //Rotate the camera when mouse move
    void Rotate()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensityvity * Time.deltaTime;
        parent.Rotate(Vector3.up, mouseX);
    }
}
