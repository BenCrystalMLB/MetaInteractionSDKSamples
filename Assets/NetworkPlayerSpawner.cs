using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        spawnedPlayerPrefab = PhotonNetwork.Instantiate("TestPlayer", transform.position, transform.rotation);
        //spawnedPlayerPrefab = PhotonNetwork.Instantiate("OVRPHOTONTESTRIG", transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room");
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }

}
