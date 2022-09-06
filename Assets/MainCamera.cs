using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    public float horizVel = 10f;
    public float vertVel = 10f;
    public float panVel = 20f;
    public float pitchVel = 20f;
    
    void Start()
    {
        transform.position = new Vector3(0,2,-2);
        transform.rotation = Quaternion.identity;
    }

    void Update()
    {
        // read keys
        float horizTrans = Input.GetAxis("Vertical") * horizVel; // forward and back
        float vertTrans = Input.GetAxis("Horizontal") * vertVel; // will need to include up-down motion later
        float horizRot = Input.GetAxis("Pan") * panVel; // controlled with J & L
        float vertRot = Input.GetAxis("Pitch") * pitchVel; // controlled with I & K
        float reset = Input.GetAxis("Fire1"); // snap camera to no X or Z rotation
        
        var dt = Time.deltaTime;
        horizTrans *= dt;
        vertTrans *= dt;
        horizRot *= dt;
        vertRot *= dt;
        
        transform.Translate(vertTrans, 0, horizTrans);
        transform.Rotate(-vertRot, horizRot, 0);
        
        if (reset>0f)
        {
            var anglesAdjust = transform.rotation.eulerAngles;
            anglesAdjust.y = 0;
            anglesAdjust *= -1;
//            print(transform.rotation.eulerAngles);
//            print(anglesAdjust);
            transform.Rotate(anglesAdjust);
        }
        
    }
}
