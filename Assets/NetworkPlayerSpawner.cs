using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class NetworkPlayerSpawner : MonoBehaviourPunCallbacks
{
    private GameObject spawnedPlayerPrefab;

    //private PhotonView photonView = spawnedpla
    //private GameObject originalPlayer; //used for referencing head and hand locations
    /*void Start()
    {
        photonView = GetComponent<PhotonView>();
    }*/
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        //if (!photonView.IsMine)
        {
            spawnedPlayerPrefab = PhotonNetwork.Instantiate("TestPlayer", transform.position, transform.rotation);
        }
        //spawnedPlayerPrefab = PhotonNetwork.Instantiate("TestPlayer", transform.position, transform.rotation);
        //spawns

        //spawnedPlayerPrefab.GetComponent<>.head = originalPlayer.GetComponent<GameObject>
        //spawnedPlayerPrefab = PhotonNetwork.Instantiate("OVRPHOTONTESTRIG", transform.position, transform.rotation);
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left room");
        base.OnLeftRoom();
        PhotonNetwork.Destroy(spawnedPlayerPrefab);
    }

}
