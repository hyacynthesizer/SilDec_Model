using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour // to be attached to each planet and prefab
{
    public string id;
    public enum BodyType {Star, Rocky, Gas, Moon};
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
    
    
    // Start is called before the first frame update
    void Start()
    {
        parent = gameObject.transform;
        if (units == AxisUnits.Km) // scaling the position properly, from AU/Km to system units
        {
            
        }
        
        if (type == BodyType.Rocky)
        {
            
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    Vector3d GetPosition(double time) // where the orbit equation happens, scaling time n shit
    {
        // temp return statement
        return Vector3d.zero;
    }
}
