using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AxisSpin : MonoBehaviour
{
    
    public Transform source;
    public Transform target;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,-1.3f);
        source = GameObject.Find("Main Camera").transform;
        target = GameObject.Find("Axis Center").transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = source.rotation;
        transform.position = source.forward * -1.4f;
    }
}
