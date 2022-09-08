using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

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
    private Vector3 position;
    private BodiesHandler master;
    private double worldTime;
    private double currentTime;
    
    // converting distances and sizes to unity units
    public static double AxisScale = 1; // AU to units
    public static double RadiusScale = 1; // km to units
    
    // constants for how big shit is
    private static double SunRadius = 696342; // in kilometers
    private static double EarthRadius = 6371; // in kilometers
    private static double JupiterRadius = 69909; // in kilometers
    private static double MoonRadius = 1737.4; // in kilometers
    private static double AUinKM = 149597870.7;
    
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform;
        truePosition = Vector3d.zero;
        position = (Vector3)truePosition;
        master = (BodiesHandler)FindObjectOfType(typeof(BodiesHandler));
        worldTime = master.worldTime;
        currentTime = worldTime;
        
        if (units == AxisUnits.Km) // scaling the position properly, from AU/Km to system units
        {
            axis /= AUinKM;
        }
        
        // convert to radians
        loan =* PI/180d;
        aop =* PI/180d;
        ta =* PI/180d;
        
    }

    // Update is called once per frame
    void Update()
    {
        worldTime = GetTime(master);
        if (Mathd.Abs(worldTime - currentTime) >= Mathd.Epsilon)
        {
            currentTime = worldTime;
            // call GetPosition and then move the GameObject accordingly
            // get truePosition
            // get trueRotation
            // update accordingly
        }
    }
    
    double GetTime(BodiesHandler source)
    {
        return master.worldTime;
    }
    
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
    
    Vector3d GetPosition(double time) // where the orbit equation happens, scaling time n shit
    {
        static double Sin2(double d) {
            return Pow(Sin(d), 2d);
        }
        
        static double Cos2(double d) {
            return Pow(Cos(d), 2d);
        }
        
        time *= (PI * 2); // so that 1 period completes in 1 time
        // also need to scale by orbital period in terms of worldTime steps
        // time /= [orbital period], plus however time ends up being scaled, aaaaaa
        Vector3d newPosition = new Vector3d();
        
        // 3d orbit equations from mathematica
        // this is going to be gross, Sorry
        {
            newPosition.x = ((-(axis*ecc) + axis*Cos(time))*(Cos(aop)*Cos2(incl)*Cos(loan) + Cos(aop)*Cos(loan)*Sin2(incl) - Cos(incl)*Sin(aop)*Sin(loan)))/ (Cos2(aop)*Cos2(incl)*Cos2(loan) + Cos2(incl)*Cos2(loan)*Sin2(aop) + Cos2(aop)*Cos2(loan)*Sin2(incl) + Cos2(loan)*Sin2(aop)*Sin2(incl) +  Cos2(aop)*Cos2(incl)*Sin2(loan) + Cos2(incl)*Sin2(aop)*Sin2(loan) + Cos2(aop)*Sin2(incl)*Sin2(loan) + Sin2(aop)*Sin2(incl)*Sin2(loan)) + (axis*Sqrt(1 - Pow(ecc,2))*(-(Cos2(incl)*Cos(loan)*Sin(aop)) - Cos(loan)*Sin(aop)*Sin2(incl) - Cos(aop)*Cos(incl)*Sin(loan))*Sin(time))/(Cos2(aop)*Cos2(incl)*Cos2(loan) + Cos2(incl)*Cos2(loan)*Sin2(aop) + Cos2(aop)*Cos2(loan)*Sin2(incl) + Cos2(loan)*Sin2(aop)*Sin2(incl) + Cos2(aop)*Cos2(incl)*Sin2(loan) + Cos2(incl)*Sin2(aop)*Sin2(loan) + Cos2(aop)*Sin2(incl)*Sin2(loan) + Sin2(aop)*Sin2(incl)*Sin2(loan));
            // i am so sorry to anyone who has to read this for any reason
            // i did this initial matrix baking in mathematica
            newPosition.y = ((-(axis*ecc) + axis*Cos(time))*(Cos(incl)*Cos(loan)*Sin(aop) + Cos(aop)*Cos2(incl)*Sin(loan) + Cos(aop)*Sin2(incl)*Sin(loan)))/(Cos2(aop)*Cos2(incl)*Cos2(loan) + Cos2(incl)*Cos2(loan)*Sin2(aop) + Cos2(aop)*Cos2(loan)*Sin2(incl) + Cos2(loan)*Sin2(aop)*Sin2(incl) + Cos2(aop)*Cos2(incl)*Sin2(loan) + Cos2(incl)*Sin2(aop)*Sin2(loan) + Cos2(aop)*Sin2(incl)*Sin2(loan) + Sin2(aop)*Sin2(incl)*Sin2(loan)) + (axis*Sqrt(1 - Pow(ecc,2d))*(Cos(aop)*Cos(incl)*Cos(loan) - Cos2(incl)*Sin(aop)*Sin(loan) - Sin(aop)*Sin2(incl)*Sin(loan))*Sin(time))/(Cos2(aop)*Cos2(incl)*Cos2(loan) + Cos2(incl)*Cos2(loan)*Sin2(aop) + Cos2(aop)*Cos2(loan)*Sin2(incl) + Cos2(loan)*Sin2(aop)*Sin2(incl) + Cos2(aop)*Cos2(incl)*Sin2(loan) + Cos2(incl)*Sin2(aop)*Sin2(loan) + Cos2(aop)*Sin2(incl)*Sin2(loan) + Sin2(aop)*Sin2(incl)*Sin2(loan));
            // if you're reading this for debugging i apologize
            newPosition.z = ((-(axis*ecc) + axis*Cos(time))*(Cos2(loan)*Sin(aop)*Sin(incl) + Sin(aop)*Sin(incl)*Sin2(loan)))/(Cos2(aop)*Cos2(incl)*Cos2(loan) + Cos2(incl)*Cos2(loan)*Sin2(aop) + Cos2(aop)*Cos2(loan)*Sin2(incl) + Cos2(loan)*Sin2(aop)*Sin2(incl) + Cos2(aop)*Cos2(incl)*Sin2(loan) + Cos2(incl)*Sin2(aop)*Sin2(loan) + Cos2(aop)*Sin2(incl)*Sin2(loan) + Sin2(aop)*Sin2(incl)*Sin2(loan)) + (axis*Sqrt(1 - Pow(ecc,2d))*(Cos(aop)*Cos2(loan)*Sin(incl) + Cos(aop)*Sin(incl)*Sin2(loan))*Sin(time))/(Cos2(aop)*Cos2(incl)*Cos2(loan) + Cos2(incl)*Cos2(loan)*Sin2(aop) + Cos2(aop)*Cos2(loan)*Sin2(incl) + Cos2(loan)*Sin2(aop)*Sin2(incl) + Cos2(aop)*Cos2(incl)*Sin2(loan) + Cos2(incl)*Sin2(aop)*Sin2(loan) + Cos2(aop)*Sin2(incl)*Sin2(loan) + Sin2(aop)*Sin2(incl)*Sin2(loan));
        }
        
        
        return newPosition * AxisScale;
    }
    
    void SetPosition(Vector3d update) // downgrades to float position for rendering, etc
    {
        parent.position = (Vector3)update;
    }
    
}
