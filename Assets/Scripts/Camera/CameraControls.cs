using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] float speed = 5f;
    [SerializeField] Camera[] cameras;

    private float m_Zoom = 1;

    // Update is called once per frame
    void Update()
    {
        m_Zoom -= Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetAxis("Mouse ScrollWheel") != 0)
            Debug.Log(m_Zoom);

        if (m_Zoom < 0.6f)
            m_Zoom = 0.6f;
        else if (m_Zoom > 5f)
            m_Zoom = 5f;

        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) // || Input.mousePosition.y >= Screen.height - 2))
        {
            transform.position += new Vector3(Time.deltaTime, 0, Time.deltaTime) * speed * m_Zoom;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) // || Input.mousePosition.y <= 2)
        {
            transform.position -= new Vector3(0.5f * Time.deltaTime, 0, 0.5f * Time.deltaTime) * speed * m_Zoom;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) // || Input.mousePosition.x >= Screen.width - 2)
        {
            transform.position += transform.right * Time.deltaTime * speed * m_Zoom;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) // || Input.mousePosition.x <= 2)
        {
            transform.position -= transform.right * Time.deltaTime * speed * m_Zoom;
        }

        SetZoom();

    }

    private void SetZoom()
    {
        for(int i = 0; i < cameras.Length; i++)
        {
            cameras[i].orthographicSize = 10 * m_Zoom;
        }
    }
}
