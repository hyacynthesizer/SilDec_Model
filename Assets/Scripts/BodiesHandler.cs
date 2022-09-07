using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class BodiesHandler : MonoBehaviour
{
    public TextAsset data;
    
    private GameObject star;
    private GameObject rocky;
    private GameObject gassy;
    private GameObject moon;
    
    // dictionary to hold all the planets' ID's and GameObjects that have the psotion scripts/data
    private IDictionary<string, GameObject> allPlanets = new Dictionary<string, GameObject>();
    
    
    void Start()
    {
        transform.position = new Vector3(0,0,0); // no more positioning jank, please!
        
        // find the prefabs
        star = Resources.Load<GameObject>("starPrefab");
        
        // read in CSV of raw planetary data
        string csv = data.ToString();
        string[] rows = csv.Split('\n');
        foreach (var row in rows)
        {
            GameObject addBody = PlanetCSV(row);
            addBody.transform.SetParent(this.transform);
            addBody.SetActive(true);
        }
        // foreach row in rows, run planetfromcsv, and shove the result planet and its object into the dictionary
        // make the new objects be children of Stellar Bodies, and make moons be children of their planets
        // after this, activate the new object, so its position isnt janked
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
    
    private GameObject PlanetCSV(string data) // take in row of text, parse out parameters, return instance of planetary body
    {
        string[] parameters = data.Split(',');
        
        GameObject newBody = Instantiate(star) as GameObject;
        
        // set values of newBody to data from the string[]
        
        return newBody;
    }
    
}