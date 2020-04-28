using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S; //singleton

    [Header("Defined in inspector")]
    public float minDist = 0.1f;

    private LineRenderer line;
    private GameObject _poi;
    private List<Vector3> points;

    private void Awake() {
        S = this;
        line = GetComponent<LineRenderer>();
        line.enabled = false;
        points = new List<Vector3>();
    }

    public GameObject poi {
        get {
            return (_poi);
        }
        set {
            _poi = value;
            if(_poi != null) {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint() {
        //add point to line
        Vector3 pt = _poi.transform.position;
        if(points.Count > 0 && (pt-lastPoint).magnitude < minDist) {
            return;//if not far enough
        }
        if(points.Count == 0) {
            Vector3 launchPosDiff = pt - Slingshot.LAUNCH_POS; //to define
            //add aiming 
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true;
        }
        else {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count - 1, lastPoint);
            line.enabled = true;
        }
    }

    //return pos of last added point
    public Vector3 lastPoint {
        get {
            if (points == null) {
                return Vector3.zero;
            }
            return points[points.Count - 1];
        }
    }

    private void FixedUpdate() {
        if (poi == null) {
            //if not defined llok for him
            if (FollowCam.POI != null) {
                if (FollowCam.POI.tag == "Projectile") {
                    poi = FollowCam.POI;
                }
                else {
                    return;
                }
            }
            else {
                return;
            }
        }
        //if poi exists add his position
        AddPoint();
        if(FollowCam.POI == null) {
            poi = null;
        }
    }

}
