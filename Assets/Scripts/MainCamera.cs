using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    public float horizVel = 5f;
    public float vertVel = 5f;
    public float panVel = 30f;
    public float pitchVel = 30f;
    public float ascendVel = 2f;
    public float descendVel = 1f;
    public float floorHeight = 0.01f; // lowest camera height
    private float slowDown = 0.75f;
    
    void Start()
    {
        transform.position = new Vector3(0,0.4f,-2f);
        transform.rotation = Quaternion.identity;
    }

    void Update() // add in support for different camera modes? like overhead for measuring, etc
    {
        // read keys
        float horizTrans = Input.GetAxis("Vertical") * horizVel; // forward and back
        float vertTrans = Input.GetAxis("Horizontal") * vertVel;
        float horizRot = Input.GetAxis("Pan") * panVel; // controlled with J & L
        float vertRot = Input.GetAxis("Pitch") * pitchVel; // controlled with I & K
        float ySpeed = 0f;
        bool reset = Input.GetKey(KeyCode.R); // snap camera to no X or Z rotation
        
        if (Input.GetKey(KeyCode.Space))
        {
            ySpeed += ascendVel; // I GET UP!
        }
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
        {
            ySpeed -= descendVel; // FUCK IT, DESCEND
        }
        
        var dt = Time.deltaTime;
        dt *= slowDown;
        horizTrans *= dt;
        vertTrans *= dt;
        horizRot *= dt;
        vertRot *= dt;
        ySpeed *= dt;
        
        // do the movement things
        transform.Translate(vertTrans, 0, horizTrans);
        if (transform.position.y <= floorHeight && ySpeed < 0)
        {
            ySpeed = 0f;
        }
        transform.Translate(0, ySpeed, 0, Space.World);
        transform.Rotate(-vertRot, horizRot, 0);
        
        if (reset)
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
