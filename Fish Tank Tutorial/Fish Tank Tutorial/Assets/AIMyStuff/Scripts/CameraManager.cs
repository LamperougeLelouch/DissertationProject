using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera[] cameras;

    public void cameraOne()
    {
        cameras[0].enabled = true;
        cameras[1].enabled = false;
        cameras[2].enabled = false;
        cameras[3].enabled = false;
        cameras[4].enabled = false;
        cameras[5].enabled = false;
    }

    public void cameraTwo()
    {
        cameras[0].enabled = false;
        cameras[1].enabled = true;
        cameras[2].enabled = false;
        cameras[3].enabled = false;
        cameras[4].enabled = false;
        cameras[5].enabled = false;
    }

    public void cameraThree()
    {
        cameras[0].enabled = false;
        cameras[1].enabled = false;
        cameras[2].enabled = true;
        cameras[3].enabled = false;
        cameras[4].enabled = false;
        cameras[5].enabled = false;
    }

    public void cameraFour()
    {
        cameras[0].enabled = false;
        cameras[1].enabled = false;
        cameras[2].enabled = false;
        cameras[3].enabled = true;
        cameras[4].enabled = false;
        cameras[5].enabled = false;
    }

    public void cameraFive()
    {
        cameras[0].enabled = false;
        cameras[1].enabled = false;
        cameras[2].enabled = false;
        cameras[3].enabled = false;
        cameras[4].enabled = true;
        cameras[5].enabled = false;
    }

    public void cameraSix()
    {
        cameras[0].enabled = false;
        cameras[1].enabled = false;
        cameras[2].enabled = false;
        cameras[3].enabled = false;
        cameras[4].enabled = false;
        cameras[5].enabled = true;
    }

}
