using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbit : MonoBehaviour // to be attached to each planet and prefab
{
    public string id;
    enum BodyType {Star, Rocky, Gas, Moon};
    public double axis;
    enum AxisUnits {AU, Km}; 
    public double ecc;
    public double incl;
    public double loan;
    public double aop;
    public double ta;
    public double orbper;
    public double orbvel;
    public double rotper;
    public double radius;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
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
