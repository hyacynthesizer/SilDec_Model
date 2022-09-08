using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BodiesHandler : MonoBehaviour
{
    public TextAsset data;
    
    private GameObject star;
    private GameObject rocky;
    private GameObject gassy;
    private GameObject moon;
    
    public double worldTime;
    
    // dictionary to hold all the planets' ID's and each planet's script
    // essentially, manipulate planets via their script functions
    private IDictionary<string, Orbit> allPlanets = new Dictionary<string, Orbit>();
    
    
    void Start()
    {
        transform.position = new Vector3(0,0,0); // no more positioning jank, please!
        
        // find the prefabs
        star = Resources.Load<GameObject>("Prefabs/starPrefab");
        rocky = Resources.Load<GameObject>("Prefabs/rockyPrefab");
        gassy = Resources.Load<GameObject>("Prefabs/gassyPrefab");
        moon = Resources.Load<GameObject>("Prefabs/moonPrefab");
        
        // read in CSV of raw planetary data
        string csv = data.ToString();
        string[] rows = csv.Split('\n');
        for (int i = 2; i < rows.Length; i++)
        {
            GameObject addBody = PlanetCSV(rows[i]);
            addBody.transform.SetParent(this.transform); // set to be child of bodiesHandler
            addBody.SetActive(true);
            string name = addBody.GetComponent<Orbit>().id;
            addBody.name = name; // rename instance
            addBody.GetComponent<Orbit>().parent = addBody.transform;
            
            // set moons to child of planet
            if (name[0] == 'M')
            {
                string match = name.Substring(name.Length - 2); // extract planet name
//                print(match);
                Orbit parentPlanet = allPlanets[match]; // get planet from dictionary
//                print(parentPlanet.parent.name);
                addBody.transform.SetParent(parentPlanet.parent); // attach to planet's transform
            }
            allPlanets.Add(addBody.name,addBody.GetComponent<Orbit>());
        }
        
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
        // null initial object in case the csv prefab tag is broken
        GameObject newBody = new GameObject();
        newBody.AddComponent<Orbit>();
        string[] stats = data.Split(',');
        
        // pick correct prefab based on tag on radius
        char planetTypeID = stats[10][0];
        var tagList = new(char ID, GameObject BodyType)[]
        {
            ('S', star),
            ('E', rocky),
            ('J', gassy),
            ('M', moon)
        };
        
        foreach (var tag in tagList)
        {
            if (planetTypeID == tag.ID)
            {
                Destroy(newBody);
                newBody = Instantiate(tag.BodyType) as GameObject;
            }
        }
        
        
        Orbit details = newBody.GetComponent<Orbit>();
        // set values of newBody to data from the string[]
        details.id = stats[0];
        // logic to extract enum type from planetTypeID
        // ~enum bullshit~
        // also set axisunits to Km if it's a moon
//        print(stats[1]);
        details.axis = Double.Parse(stats[1]);
        details.ecc = Double.Parse(stats[2]);
        details.incl = Double.Parse(stats[3]);
        details.loan = Double.Parse(stats[4]);
        details.aop = Double.Parse(stats[5]);
        details.ta = Double.Parse(stats[6]);
        details.orbper = Double.Parse(stats[7]);
        details.orbvel = Double.Parse(stats[8]);
        details.rotper = Double.Parse(stats[9]);
        details.radius = Double.Parse(stats[10].Remove(0,1));
        return newBody;
    }
    
}