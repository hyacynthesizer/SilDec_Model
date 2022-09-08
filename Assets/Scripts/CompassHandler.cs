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
        cam = transform.GetChild(0);
        axes = transform.GetChild(1);
        textbox = transform.GetChild(2);
        
        axes.transform.position = this.transform.position;
        cam.transform.position = new Vector3(0,0.05f,-1f);
        cam.transform.rotation = textbox.transform.rotation = Quaternion.identity;
        textbox.transform.position = new Vector3(0.3f, 0.2f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
  
}
