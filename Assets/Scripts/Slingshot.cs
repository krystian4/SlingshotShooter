using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    static private Slingshot S;
    [Header("Defined in inspector")]
    public GameObject prefabProjectile;
    public float velocityMultiplier = 8f;

    [Header("Dynamically defined")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    private Rigidbody projectileRigidbody;
    public bool aimingMode;

    static public Vector3 LAUNCH_POS {
        get {
            if (S == null) return Vector3.zero;
            return S.launchPos;
        }
    }

    private void Awake() {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    private void OnMouseEnter() {
        //print("Slingshot OnMouseEnter()");
        launchPoint.SetActive(true);

    }

    private void OnMouseExit() {
        //print("Slingshot OnMouseExit()");
        launchPoint.SetActive(false);

    }

    private void OnMouseDown() {
        aimingMode = true;
        projectile = Instantiate(prefabProjectile) as GameObject;
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    private void Update() {
        if (!aimingMode) return;

        //get mouse position
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        //count vec difference between launchPos and mousePos3D
        Vector3 mouseDelta = mousePos3D - launchPos;

        //limit mouseDelta value to slingshot collider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude) {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        //Move projectile to new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0)) {
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMultiplier;
            FollowCam.POI = projectile;
            projectile = null;
            SlingshotShooter.ShotFired();
            ProjectileLine.S.poi = projectile;
        }
    }
}
