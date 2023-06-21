using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using Oculus.Interaction;
//using UnityEngine.XR.


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
    public Transform leftGlove;
    public Transform rightHand;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MapPosition(head, XRNode.Head);
        MapPosition(leftGlove, XRNode.LeftHand);
        MapPosition(rightHand, XRNode.RightHand);
        //MapPosition(leftGlove, XRNode.LeftHand);
        //MapPosition(rightHand, XRNode.RightHand);
    }

    void MapPosition(Transform target, XRNode node)
    {
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 position);
        InputDevices.GetDeviceAtXRNode(node).TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rotation);

        target.position = position;
        target.rotation = rotation;
    }
}
