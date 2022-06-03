using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallDetection : MonoBehaviour
{
    //Walls are diferent parts and each side is under same parent
    //Must add Layer,assing to this gameobject as "WallDetection"
    //Must add Layer,assing layer to walls gameobject "Walls"

    //Save wall parent used later for checks and change shader of children
    public Transform detectedWallParent;
    //radius of gizmo and used to set collider as well
    public float radius=2;


    // Start is called before the first frame update
    private void Awake()
    {
        detectedWallParent = null;
        SphereCollider sc = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
        sc.isTrigger = true;
        sc.radius = radius;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (detectedWallParent!=other.transform.parent)
        {
            detectedWallParent = other.transform.parent;
            Debug.Log("Check");
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
