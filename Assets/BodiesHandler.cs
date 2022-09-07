using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[ExecuteInEditMode]
public class BodiesHandler : MonoBehaviour
{
    
    public TextAsset data;
    // dictionary to hold all the planets' ID's and GameObjects that have the psotion scripts/data
    IDictionary<string, GameObject> allPlanets = new Dictionary<string, GameObject>();
    
    
    void Start()
    {
        transform.position = new Vector3(0,0,0); // no more positioning jank, please!
        
        // read in CSV of raw planetary data
        string csv = data.ToString();
        string[] rows = csv.Split('\n');
//        foreach (var row in rows){
//        print(row);
//        }
        // foreach row in rows, run planetfromcsv, and shove the result planet and its object into the dictionary
        // feed into dictionary of bodies and attributes
    }

    
    void Update()
    {
        // process for relocating world when camera-origin distance will cause float issues
        // process to adjust planet positions as a function of time
        // keeping track of world time in terms of earth days and system calendar
    }
    
    void ResetAll()
    {
        // iterate over all bodies, moving them to their t=0 positions
    }
    
    
}
