using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AxisSpin : MonoBehaviour
{
    
    public GameObject source;
    
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);   
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = source.transform.rotation;
    }
}
