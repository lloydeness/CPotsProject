using UnityEngine;
using System.Collections;

public class MyCamera : MonoBehaviour
{


    public static GameObject other;
    private Camera altCam;
    private Camera MainCam;

    public float smooth = 2.0F;
    public float tiltAngle = 30.0F;

    private void awake()
    {

        RemoveOtherCameras();


    }
    // Use this for initialization
    void Start()
    {
        other = GameObject.Find("Camera");
        MainCam = other.GetComponent<Camera>();



    }

    // Update is called once per frame
    void Update()
    {
        float tiltAroundZ = Input.GetAxis("Horizontal") * tiltAngle;
        float tiltAroundX = Input.GetAxis("Vertical") * tiltAngle;
        Debug.Log(tiltAroundX);
        Debug.Log(tiltAroundZ);
        Quaternion target = Quaternion.Euler(tiltAroundX, 0, tiltAroundZ);
        MainCam.transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * smooth);

        RemoveOtherCameras();



    }

    void RemoveOtherCameras()
    {
        GameObject temp;
        temp = GameObject.Find("Main Camera");
        if (temp != null)
        {


            Destroy(temp);

        }
    }
}
