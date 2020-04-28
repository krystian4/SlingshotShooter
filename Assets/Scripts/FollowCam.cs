using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Defined in inspector")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Dynamically defined")]
    public float camZ;

    private void Awake() {
        camZ = this.transform.position.z;
    }

    private void FixedUpdate() {
        //if (POI == null) return;
        //Vector3 destination = POI.transform.position;

        Vector3 destination;
        if(POI == null) {
            destination = Vector3.zero;
        }
        else {
            //get POI position
            destination = POI.transform.position;
            //if POI is a projectile check if its moving
            if (POI.tag == "Projectile") {
                if (POI.GetComponent<Rigidbody>().IsSleeping()) {
                    POI = null;
                    return;
                }
            }
        }

        //limit min x and y
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //interpolate from current pos to POI
        destination = Vector3.Lerp(transform.position, destination, easing);
        //force z position to prevent zooming
        destination.z = camZ;
        //set camera
        transform.position = destination;
        //set orthographicSize so ground is visible
        Camera.main.orthographicSize = destination.y + 10;
    }

}
