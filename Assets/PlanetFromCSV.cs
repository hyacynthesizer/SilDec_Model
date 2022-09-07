using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetFromCSV
{
    private string id;
    enum BodyType {Star, Rocky, Gas, Moon};
    private double axis;
    enum AxisUnits {AU, Km}; 
    private double ecc;
    private double incl;
    private double loan;
    private double aop;
    private double ta;
    private double orbper;
    private double orbvel;
    private double rotper;
    private double radius;
    enum RadiusScale {Sun, Earth, Jupiter, Moon};
    
    public GameObject Comvert(string data) // take in row of text, parse out parameters, return instance of planetary body
    {
        // GameObject newBody;
        // newBody = Instantiate(starPrefab);
        
        // temp empty return For Now
        return new GameObject();
    }
}
