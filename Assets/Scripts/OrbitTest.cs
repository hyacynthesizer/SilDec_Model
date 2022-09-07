using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent (typeof(LineRenderer))]
public class OrbitTest : MonoBehaviour
 {
     public Vector2 radius = new Vector2(1f, 1f);
     public float width = 0.2f;
     public float rotationAngle = 45;
     public int resolution = 500;
 
     private Vector3[] positions;
     private LineRenderer self_lineRenderer;
     
     
     void OnValidate()
     {
         UpdateEllipse();
     }
     
     public void UpdateEllipse()
     {
         if ( self_lineRenderer == null)
             self_lineRenderer = GetComponent<LineRenderer>();
             
         self_lineRenderer.positionCount = (resolution+1);
         
         self_lineRenderer.startWidth = self_lineRenderer.endWidth = width;
         
         for (int i = 0; i <= resolution; i++) 
         {
             AddPointToLineRenderer((float)i / (float)(resolution) * 2.0f * Mathf.PI, i);
         }

     }
     
     void AddPointToLineRenderer(float angle, int index)
     {
         Quaternion pointQuaternion = Quaternion.AngleAxis (rotationAngle, Vector3.forward);
         Vector3 pointPosition;
         
         pointPosition = new Vector3(radius.x * Mathf.Cos (angle), radius.y * Mathf.Sin (angle), 0.0f);
         pointPosition = pointQuaternion * pointPosition;
         
         self_lineRenderer.SetPosition(index, pointPosition);        
     }
 }