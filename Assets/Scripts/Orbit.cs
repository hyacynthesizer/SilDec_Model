using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math; // to make all the equations less bad
using System; // for number type properties, etc

public class Orbit : MonoBehaviour // to be attached to each planet and prefab
{
    // set of properties assigned by CSV parser (or just in the prefab lol)
    public string id;
    public enum BodyType {Star, Rocky, Gassy, Moon};
    public BodyType type = BodyType.Star;
    public double axis;
    public enum AxisUnits {AU, Km}; 
    public AxisUnits units = AxisUnits.AU;
    public double ecc;
    public double incl;
    public double loan;
    public double aop;
    public double ta;
    public double orbper;
    public double orbvel;
    public double rotper;
    public double radius;
    public Transform parent;
    
    private Vector3d truePosition;
    private BodiesHandler master;
    private double worldTime;
    private double currentTime;
    
    // converting distances and sizes to unity units
    private static double AxisScale = 250d; // AU to units
    private static double RadiusScale = 0.00001d; // km to units
    
    // constants for how big shit is
    private static double SunRadius = 696342; // in kilometers
    private static double EarthRadius = 6371; // in kilometers
    private static double JupiterRadius = 69909; // in kilometers
    private static double MoonRadius = 1737.4; // in kilometers
    private static double AUinKM = 149597870.7;
    
    private Quaterion qTotal; // parsing the orbital elements rotation
    
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform; // the transform of the planetary body
        truePosition = Vector3d.zero;
        master = (BodiesHandler)FindObjectOfType(typeof(BodiesHandler));
        worldTime = master.worldTime;
        currentTime = worldTime;
        
        if (units == AxisUnits.Km) // scaling the position properly, from AU/Km to system units
        {
            axis /= AUinKM; // moons are still ending up in the middle of nowhere
        }
        
        // convert to radians
        loan *= PI/180d;
        aop *= PI/180d;
        ta *= PI/180d;
        
        // initialize rotation bullshit! (should only need to crunch the quaternion once
        Quaternion qo = new Quaternion(0,0,Sin(aop/2),Cos(aop/2));
        Quaternion qi = new Quaternion(Sin(incl/2),0,0,Cos(incl/2));
        Quaternion qO = new Quaternion(0,0,Sin(loan/2),Cos(loan/2));
        Quaternion qFix = new Quaternion(Sin(PI/2),0,0,Cos(PI/2));
        qTotal = ((qo * qi) * qO) * qFix; // qFix to flip the axes conventions
        qTotal.x *= -1; // mirror the x-axis, since unity is Stupid like that
        print(qTotal.ToString()); // just make sure it's working nicely
        
        SetPosition(GetPosition(0d)); // initial location at t=0
        SetRadius(radius, type, parent); // adjust size of object
    }

    // Update is called once per frame
    void Update()
    {
        worldTime = GetTime(master); // ping the world time every frame
        if (Mathd.Abs(worldTime - currentTime) >= Double.Epsilon) // only update if the time variable is meaningful
        {
            currentTime = worldTime; // update the body's time to be with the world
            // call GetPosition and then move the GameObject accordingly
            Vector3d newPos = GetPosition(currentTime);
            SetPosition(newPos); // already takes care of updating the true position stores
            
            // get trueRotation
            // update accordingly
            
        }
    }
    
    // get the global world time from the bodies handler
    double GetTime(BodiesHandler source) { return master.worldTime; }
    
    void SetRadius(double radius, BodyType type, Transform destination)
    {
        double tempRadius = 1;
        var matchRadius = new(BodyType name, double size)[]
        {
            (BodyType.Star, SunRadius),
            (BodyType.Rocky, EarthRadius),
            (BodyType.Gassy, JupiterRadius),
            (BodyType.Moon, MoonRadius)
        };
        
        foreach (var kind in matchRadius){ if (type == kind.name){ tempRadius = kind.size; } } // match the radius n shit
        
        tempRadius *= RadiusScale;
        float rad = (float)tempRadius;
        destination.localScale = new Vector3(rad, rad, rad);
    }
    
    void SetRotation()
    {
        // handle all the axial tilt bullshit
    }
    
    // here be all the functions and stuff
    Vector3d GetPosition(double time) // where the orbit equation happens, scaling time n shit
    {
        
        time *= (PI * 2) / 100; // so that 1 period completes in 100 time
        // i know that all this scaling isnt super good
        // also need to scale by orbital period in terms of worldTime steps
        // time /= [orbital period], plus however time ends up being scaled, aaaaaa
        Vector3d newPosition = new Vector3d();
        
        // quaternion * Vector3d point gives a rotated point
        // q1 * q2 takes q1 first then applies q2
        // Quaternion rot = qo * qi * qO, then multiply that by the Vector3d point
        // either Vector3d point needs to be correct for unity's axes and qTotal needs to be rotated,
        // or point is in mathematica's axes, and then correction at the very end 
        // (let's assume that Vector3d is calculated properly, so change axes before rotating point)
        
        // run the actual ellipse equations now
        newPosition.y = 0;
        newPosition.x = axis * Cos(time + ta) - axis * ecc;
        newPosition.z = axis * Sqrt(1d - Pow(ecc, 2)) * Sin(time + ta);
        
        return (qTotal * newPosition) * AxisScale;
    }
    
    void SetPosition(Vector3d update) // downgrades to float position for rendering, etc
    {
        truePosition = update;
        parent.localPosition = (Vector3)truePosition; // move the transform relative to body handler
        // it should actually be intended behavior that moons are placed on a plane relative to their parent
        // since they follow the axial tilt and all that
    }
    
    public void ResetPosition(Orbit body) // to be called from the body handler
    {
        SetPosition(body.GetPosition(0d));
        body.worldTime = body.currentTime = 0d; // i stg if this causes access violations
    }
    
}
