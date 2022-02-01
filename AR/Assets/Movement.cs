using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;

public class Movement : MonoBehaviour
{

    //public float angle;
    //public bool angleDetected = false;
    public VuforiaCamera cam;

    // Start is called before the first frame update
    void Start()
    {
        //cam = GetComponent<VuforiaCamera>();
    }

    void Update()
    {
        Debug.Log(cam.alpha);
        float a = (float)cam.alpha;
        transform.eulerAngles = new Vector3(90.0f, a*180.0f/3.1416f, 0.0f);
    }
}
