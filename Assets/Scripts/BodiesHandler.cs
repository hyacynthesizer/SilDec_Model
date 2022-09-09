using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using static System.Math;

public class BodiesHandler : MonoBehaviour
{
    public TextAsset data;
    
    private GameObject star;
    private GameObject rocky;
    private GameObject gassy;
    private GameObject moon;
    
    public GameObject self;
    public double worldTime;
    private Vector3d truePos;
    
    // dictionary to hold all the planets' ID's and each planet's script
    // essentially, manipulate planets via their script functions
    private IDictionary<string, Orbit> allPlanets = new Dictionary<string, Orbit>();
    
    // dictionary to hold all the increments that time can be updated
    private IDictionary<string, double> timeSteps = new Dictionary<string, double>();
    
    void Start()
    {
        transform.position = new Vector3(0,0,0); // no more positioning jank, please!
        truePos = Vector3d.zero;
        // find the prefabs
        star = Resources.Load<GameObject>("Prefabs/starPrefab");
        rocky = Resources.Load<GameObject>("Prefabs/rockyPrefab");
        gassy = Resources.Load<GameObject>("Prefabs/gassyPrefab");
        moon = Resources.Load<GameObject>("Prefabs/moonPrefab");
        
        // initialize dictionary of time increments
        timeSteps.Add("arcsecond", 1d/36d/36d);
        timeSteps.Add("arcminute", 1d/36d);
        timeSteps.Add("archour", 1d);
        timeSteps.Add("arcshift", 36d);
        timeSteps.Add("arcday", 1296d);
        timeSteps.Add("arcmonth", 37584d);
        timeSteps.Add("arcyear", 338256d);
        timeSteps.Add("indiction", 1353024);
        
        // read in CSV of raw planetary data
        string csv = data.ToString();
        string[] rows = csv.Split('\n');
        for (int i = 2; i < rows.Length; i++) // generate planet for each row of CSV data
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
                addBody.transform.SetParent(parentPlanet.parent, false); // attach to planet's transform
            }
            allPlanets.Add(addBody.name,addBody.GetComponent<Orbit>());
        }
        
    }

    
    void Update()
    {
        // process to adjust planet positions as a function of time
        // keeping track of world time in terms of earth days and system calendar
    }
    
    void ResetAll()
    {
        // iterate over all bodies, moving them to their t=0 positions
        foreach (var pair in allPlanets)
        {
            pair.Value.ResetPosition();
        }
    }
    
    public static void TranslateAll(Vector3 move)
    {
        GameObject bodies = GameObject.Find("Stellar Bodies");
        bodies.transform.Translate(move);
        Vector3d updatePos = bodies.GetComponent<BodiesHandler>().truePos;
        updatePos = updatePos + (Vector3d)move;
    }
    
    // take in row of text, parse out parameters, return instance of planetary body
    private GameObject PlanetCSV(string data)
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
                Destroy(newBody); // get rid of the null object
                newBody = Instantiate(tag.BodyType) as GameObject;
            }
        }
        
        
        Orbit details = newBody.GetComponent<Orbit>();
        // set values of newBody to data from the string[]
        details.id = stats[0]; // yes this is gross but i can't think of a better way to do it
        details.axis = Double.Parse(stats[1]);
        details.ecc = Double.Parse(stats[2]);
        details.incl = Double.Parse(stats[3]);
        details.loan = Double.Parse(stats[4]);
        details.aop = Double.Parse(stats[5]);
        details.ta = Double.Parse(stats[6]);
        details.orbper = Double.Parse(stats[7]);
        details.orbvel = Double.Parse(stats[8]);
        details.rotper = Double.Parse(stats[9]);
        details.radius = Double.Parse(stats[10].Remove(0,1)); // enum type handling is done inside the prefabs
        return newBody;
    }
    
}