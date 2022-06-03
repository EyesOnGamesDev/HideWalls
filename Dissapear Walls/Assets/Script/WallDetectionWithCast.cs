using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WallDetectionWithCast : MonoBehaviour
{
    /// <summary>
    /// Script used to hide walls when player is close to them. Top Down camera
    /// Script is placed on gameobject under camera
    /// In order to work Gameobject of wall must be set to specific layer and variable should be set _mask_layers
    /// A)
    /// Walls can be disactivate(hide mesh renderer).Each wall gameobject has a lower child.If main wall disactivate child is still visible
    /// Note child-Wall must have diffent layer in order not to be detected
    /// B)
    /// Walls can be changed to material with lower opacity(See throught Wall)
    /// </summary>

    
    //Not Used anymore
    private float DissapearTimer = 3;

    [Header("Hide Wall Script: child sub-walls")]
    //Used for activating lower walls. It need child GameObject below wall in order to work.Its the default option
    public bool activateSecondWall=true;

    //Change Shader Method,dont disactivate just change material with lower opacity
    public bool changeShaderMethod;

    [HideInInspector]
    public Material originalMat;
    [HideInInspector]
    public Material changeMat;

    //radius of gizmo and used to radius of SphereCast
    public float radius = 2;

    //Player transform Position
    public Transform _trans_Player;
    //Wall Layers
    public LayerMask _mask_Layers;

    //Save wall parent used later for checks if current wall is the shame as previous one
    private Transform detectedWallParent;

    //Gather all parent walls to be hiden at each case
    private List<Transform> hidenWalls;

    //Gather all parents that are checked in order not to check them again
    private List<Transform> CheckedParent;

    private void Awake()
    {
        CheckedParent = new List<Transform>();
        hidenWalls = new List<Transform>();
        //_trans_Player = player.transform;
    }

    private void FixedUpdate()
    {
        DetectWalls();
    }

    //Debuging Draws
    /*
    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_trans_Player.position, radius);
    }
    */

    private void DetectWalls()
    {

        Vector3 fromPosition = transform.position;
        Vector3 toPosition = _trans_Player.transform.position;
        Vector3 direction = toPosition - fromPosition;
        float distance = Vector3.Distance(fromPosition, toPosition);

        RaycastHit hit;
        if (Physics.Raycast(fromPosition, direction, out hit, distance,_mask_Layers))
        {
            if(hit.transform.parent != detectedWallParent)
            {

                //StartCoroutine(ReAppearWalls(detectedWallParent));
                detectedWallParent = hit.transform.parent;

                if (activateSecondWall)
                {
                    HideWalls();
                }
            }
            //Debug.DrawLine(fromPosition, toPosition,Color.red);
        }
        else
        {
            if(detectedWallParent!=null)
            {
                if (activateSecondWall)
                {
                    ShoWalls(detectedWallParent);
                    //StartCoroutine(ReAppearWalls(detectedWallParent));
                }
                detectedWallParent = null;
                //Debug.DrawLine(fromPosition, toPosition, Color.green);
            }

        }
    }

    IEnumerator ReAppearWalls(Transform wall)
    {
        //yield return new WaitForSeconds(DissapearTimer);
        if(wall!=detectedWallParent)
        {
            ShoWalls(wall);
        }
        yield return null;
    }

    void DetecteWalls(Vector3 center, float radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(center, radius,_mask_Layers);
        

        foreach (var hitCollider in hitColliders)
        {
            
            bool hideWallsCheck = true;

            //If we havent checked parent check it
            if(!CheckedParent.Contains(hitCollider.transform.parent))
            {
                foreach (Transform child in hitCollider.transform.parent)
                {
                    if (child.position.z > _trans_Player.position.z)
                    {
                        hideWallsCheck = false;
                        break;
                    }
                }
                CheckedParent.Add(hitCollider.transform);
            }
            

            //Check if each wall on parent is 
            
            if(hideWallsCheck)
            {
                var parent = hitCollider.transform.parent;
                if (!hidenWalls.Contains(parent))
                {
                    hidenWalls.Add(parent);
                }
            }
            
        }
        CheckedParent.Clear();

    }

    private void HideWalls()
    {
        if(detectedWallParent!=null)
        {
            DetecteWalls(_trans_Player.position, radius);
            foreach(Transform obj in hidenWalls)
            {
                foreach (Transform child in obj)
                {
                    child.GetComponent<MeshRenderer>().enabled = false;
                }
            }   
        }
    }

    //Method that hides main walls, based on give parent
    private void ShoWalls(Transform wall)
    {
        DetecteWalls(_trans_Player.position, radius);

        foreach (Transform obj in hidenWalls)
        {
            foreach (Transform child in obj)
            {
                child.GetComponent<MeshRenderer>().enabled = true;
            }
        }
        hidenWalls.Clear();
    }

    
    //Used to make two materials appear on inspector
#if UNITY_EDITOR
    [CustomEditor(typeof(WallDetectionWithCast))]
    public class RandomScript_Editor : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector(); // for other non-HideInInspector fields

            WallDetectionWithCast script = (WallDetectionWithCast)target;


            // draw checkbox for the bool
            //script.changeShaderMethod = EditorGUILayout.Toggle("Start Temp", script.changeShaderMethod);

            if (script.changeShaderMethod) // if bool is true, show other fields
            {
                script.originalMat = EditorGUILayout.ObjectField("I Field", script.originalMat, typeof(Material), true) as Material;
                script.changeMat = EditorGUILayout.ObjectField("Template", script.changeMat, typeof(Material), true) as Material;
            }

            if(script.changeShaderMethod)
            {
                script.activateSecondWall = false;
            }
            else
            {
                script.activateSecondWall = true;
            }
        }
    }
#endif
    

}
