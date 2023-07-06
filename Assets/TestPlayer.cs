using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Oculus.Interaction;
//using UnityEngine.XR.
using Photon.Pun;


/*
 * Replace the map position of the XRNode.hands with oculus hand tracking, 
 * not sure which library needs to be included or what function though
 * 
 * right now it's getting the position of the (controllers/ hands) at the xr nodes 
 * but it's working with controllers, I need to get the hand tracking hand positions
 */

public class TestPlayer : MonoBehaviour
{
    public Transform head;
    public Transform leftHand;
    public Transform rightHand;

    public Transform testHead;
    public Transform testLeftGlove;
    public Transform testRightHand;

    private PhotonView photonView;

    // Start is called before the first frame update
    void Start()
    {
        photonView = GetComponent<PhotonView>();

        head = GameObject.Find("OVRCameraRig").transform.Find("TrackingSpace").Find("CenterEyeAnchor").transform;
        leftHand = GameObject.Find("OVRCameraRig").transform.Find("TrackingSpace").Find("LeftHandAnchor").transform;
        rightHand = GameObject.Find("OVRCameraRig").transform.Find("TrackingSpace").Find("RightHandAnchor").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            testHead.gameObject.SetActive(false);
            testLeftGlove.gameObject.SetActive(false);
            testRightHand.gameObject.SetActive(false);

            MapPosition(testHead, head);
            MapPosition(testLeftGlove, leftHand);
            MapPosition(testRightHand, rightHand);
        }

        //MapPosition(testHead, XRNode.Head);
        
        
        //MapPosition(testLeftGlove, XRNode.LeftHand);
        //MapPosition(testRightHand, XRNode.RightHand);
        
        //MapPosition(leftGlove, XRNode.LeftHand);
        //MapPosition(rightHand, XRNode.RightHand);
    }

    //void MapPosition(Transform target, XRNode node)
    void MapPosition(Transform target, Transform origin)
    {
        Vector3 position = origin.transform.position;
        Quaternion rotation = origin.transform.rotation;

        //InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        //InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = position;
        target.rotation = rotation;
    }
}
