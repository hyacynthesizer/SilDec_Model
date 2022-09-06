using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CompassLock : MonoBehaviour
{

    public Transform target;
    public GameObject following;
    public float distance = 1.5f;
    
//    private Vector3 offset;
//    private Vector3 clamp;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, -distance);
    }

    // Update is called once per frame
    void Update()
    {
//        offset  = following.transform.position;
//        clamp = Vector3.Normalize(offset);
//        transform.position = clamp * distance;
        transform.LookAt(target);
    }
}
