using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CompassHandler : MonoBehaviour
{
    
    private Transform cam;
    private Transform axes;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);
        cam = transform.GetChild(0);
        axes = transform.GetChild(1);
        axes.transform.position = this.transform.position;
        cam.transform.position = new Vector3(0,0.05f,-1f);
        cam.transform.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
