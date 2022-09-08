using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public double worldTime;
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
        
        
    }

    // Update is called once per frame
    void Update()
    {
        worldTime = GetTime(master);
        if (Mathd.Abs(worldTime - currentTime) >= Mathd.Epsilon)
        {
            currentTime = worldTime;
            // call GetPosition and then move the GameObject accordingly
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
        
        foreach (var kind in matchRadius)
        {
            if (type == kind.name)
            {
                tempRadius = kind.size;
            }
        }
        
        tempRadius *= RadiusScale;
        float rad = (float)tempRadius;
        destination.localScale = new Vector3(rad, rad, rad);
    }
    
    Vector3d GetPosition(double time) // where the orbit equation happens, scaling time n shit
    {
        // temp return statement
        return truePosition * AxisScale;
    }
    
    
}
