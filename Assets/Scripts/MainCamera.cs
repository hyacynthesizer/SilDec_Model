using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    
    private float horizVel;
    private float vertVel;
    private float panVel;
    private float pitchVel;
    private float ascendVel;
    private float descendVel;
    private float floorHeight; // lowest camera height
    private float slowDown = 0.75f;
    
    public float maxOriginDistance = 100f; // make this private once testing isnt necessary
    
    private BodiesHandler bodies;
    
    void Start()
    {
        horizVel = 50f;
        vertVel = 50f;
        panVel = 50f;
        pitchVel = 50f;
        ascendVel = 10f;
        descendVel = 5f;
        
        transform.position = new Vector3(0,0.4f,-2f);
        transform.rotation = Quaternion.identity;
        bodies = (BodiesHandler)FindObjectOfType(typeof(BodiesHandler)); // hook to all bodies that aren't UI
    }

    void Update() // add in support for different camera modes? like overhead for measuring, etc
    {
        // read keys (yes i know they're misnamed for the axes)
        float horizTrans = Input.GetAxis("Vertical") * horizVel; // forward and back
        float vertTrans = Input.GetAxis("Horizontal") * vertVel; // side to side
        float horizRot = Input.GetAxis("Pan") * panVel; // controlled with J & L
        float vertRot = Input.GetAxis("Pitch") * pitchVel; // controlled with I & K
        float ySpeed = 0f;
        bool reset = Input.GetKeyDown(KeyCode.R); // snap camera to no X or Z rotation
        
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
        transform.Translate(0, ySpeed, 0, Space.World);
        transform.Rotate(-vertRot, horizRot, 0);
        
        if (reset){ ResetTilt(); }     
        
        // shifting origin
        if (transform.position.magnitude >= maxOriginDistance){ ShiftAll(transform.position, bodies); }
        
        // make this way of snapping to target be better at a later time
        if (Input.GetKey(KeyCode.Alpha1)) {transform.LookAt(GameObject.Find("P1").transform.position);}
        if (Input.GetKey(KeyCode.Alpha2)) {transform.LookAt(GameObject.Find("P2").transform.position);}
        if (Input.GetKey(KeyCode.Alpha3)) {transform.LookAt(GameObject.Find("P3").transform.position);}
        if (Input.GetKey(KeyCode.Alpha4)) {transform.LookAt(GameObject.Find("P4").transform.position);}
        if (Input.GetKey(KeyCode.Alpha5)) {transform.LookAt(GameObject.Find("P5").transform.position);}
        if (Input.GetKey(KeyCode.Alpha6)) {transform.LookAt(GameObject.Find("P6").transform.position);}
        if (Input.GetKey(KeyCode.Alpha7)) {transform.LookAt(GameObject.Find("M1P2").transform.position);}
        if (Input.GetKey(KeyCode.Alpha8)) {transform.LookAt(GameObject.Find("M2P2").transform.position);}
        if (Input.GetKey(KeyCode.Alpha0)) {transform.LookAt(GameObject.Find("Stellar Bodies").transform.position);}
        // this is just being shitty for testing purposes i swear
    }
    
      
    void ShiftAll(Vector3 position, BodiesHandler bodies) // handle all the stuff necessary for shifting the origin and objects
    {
        this.transform.position = Vector3.zero;
        BodiesHandler.TranslateAll(-position);
    }
    
    void ResetTilt()
    {
        var anglesAdjust = transform.rotation.eulerAngles; // aaaa quaternions (4 a's, see :p )
            anglesAdjust.y = 0; // make no change to y rotation
            anglesAdjust *= -1; // invert rotation
//            print(transform.rotation.eulerAngles);
//            print(anglesAdjust);
        transform.Rotate(anglesAdjust);
    }
}
