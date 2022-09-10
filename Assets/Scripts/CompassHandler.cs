using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[ExecuteInEditMode]
public class CompassHandler : MonoBehaviour
{
    
    private Transform cam;
    private Transform axes;
    private Transform textbox;
    
    void Start()
    {
        transform.position = new Vector3(0,0,0); // keep reference in place
        
        // assign children from list, in order (WILL BREAK IF REORDERED)
        cam = GameObject.Find("Compass Camera").transform;
        axes = GameObject.Find("Axis Center").transform;
        
        axes.position = Vector3.zero;
        cam.position = new Vector3(0,0.05f,-1f);
        cam.rotation = Quaternion.identity;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
}
