using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class BodiesHandler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0,0,0);
        
        // read in CSV of raw planetary data
        // feed into dictionary of bodies and attributes
    }

    // Update is called once per frame
    void Update()
    {
        // command to reset all planets to their base positions
        // process for relocating world when camera-origin distance will cause float issues
        // process to adjust planet positions as a function of time
        // keeping track of world time in terms of earth days and system calendar
    }
}
