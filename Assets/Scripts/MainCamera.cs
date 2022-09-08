using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    public float horizVel;
    public float vertVel;
    public float panVel;
    public float pitchVel;
    public float ascendVel;
    public float descendVel;
    public float floorHeight; // lowest camera height
    private float slowDown = 0.75f;
    
    public float maxOriginDistance = 75f; // make this private once testing isnt necessary
    
    private BodiesHandler bodies;
    
    void Start()
    {
        horizVel = 5f; // just override these in the editor to test things
        float vertVel = 5f;
        float panVel = 30f;
        float pitchVel = 30f;
        float ascendVel = 2f;
        float descendVel = 1f;
        float floorHeight = 0.01f;
        
        transform.position = new Vector3(0,0.4f,-2f);
        transform.rotation = Quaternion.identity;
        bodies = (BodiesHandler)FindObjectOfType(typeof(BodiesHandler)); // hook to all bodies that aren't UI
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
        
        if (Input.GetKey(KeyCode.Space)) { ySpeed += ascendVel; } // I GET UP! 
        
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) { ySpeed -= descendVel; } // FUCK IT, DESCEND
        
        var dt = Time.deltaTime; // scale to framerate, etc
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
            var anglesAdjust = transform.rotation.eulerAngles; // aaaa quaternions (4 a's, see :p )
            anglesAdjust.y = 0; // make no change to y rotation
            anglesAdjust *= -1; // invert rotation
//            print(transform.rotation.eulerAngles);
//            print(anglesAdjust);
            transform.Rotate(anglesAdjust);
        }     
        
        // shifting origin
        if (transform.position.magnitude >= maxOriginDistance)
        {
            ShiftAll(transform.position, bodies);
        }
    }
    
      
    void ShiftAll(Vector3 position, BodiesHandler bodies) // handle all the stuff necessary for shifting the origin and objects
    {
        this.transform.position = Vector3.zero;
        BodiesHandler.TranslateAll(-position);
    }
}
