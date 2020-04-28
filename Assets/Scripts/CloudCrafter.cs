using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudCrafter : MonoBehaviour
{
    [Header("Defined in inspector")]
    public int numClouds = 40;
    public GameObject cloudPrefab;
    public Vector3 cloudPosMin = new Vector3(-50, 5, 10);
    public Vector3 cloudPosMax = new Vector3(150, 100, 10);
    public float cloudScaleMin = 1;
    public float cloudScaleMax = 3;
    public float cloudSpeedMult = 0.5f;

    private GameObject[] cloudInstances;

    private void Awake() {
        cloudInstances = new GameObject[numClouds];
        //look for a parent object - CloudAnchor
        GameObject anchor = GameObject.Find("CloudAnchor");
        GameObject cloud;
        for(int i = 0; i < numClouds; i++) {
            cloud = Instantiate<GameObject>(cloudPrefab);
            //determine position of cloud
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            //define the size of cloud
            float scaleU = Random.value;
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU);
            //smaller cloud should be lower
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU);
            //smaller cloud should be further
            cPos.z = 100 - 90 * scaleU;
            //set transform to cloud
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;

            //make cloud a child of anchor object
            cloud.transform.SetParent(anchor.transform);
            cloudInstances[i] = cloud;

        }
    }

    private void Update() {
        foreach(GameObject cloud in cloudInstances) {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            //bigger louds move faster
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult;

            if(cPos.x <= cloudPosMin.x) {
                cPos.x = cloudPosMax.x;
            }

            cloud.transform.position = cPos;
        }
    }

}
