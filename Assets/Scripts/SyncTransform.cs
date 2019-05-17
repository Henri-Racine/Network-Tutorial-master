using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SyncTransform : NetworkBehaviour
{
    // speed of lerping roation and positon
    public float lerpRate = 15;

    //threshold for when to send commands 
    public float positionThreshold = 0.5f;
    public float rotationThreshold = 0.5f;

    private Vector3 lastPosition;
    private Quaternion lastRotation;

    //vars to be synced across the network
    [SyncVar] private Vector3 syncPosition;
    [SyncVar] private Quaternion syncRotation;

    // obtain rigidbody
    private Rigidbody rigid;
    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void LerpPosition()
    {
        rigid.position = Vector3.Lerp(rigid.position, syncPosition, Time.deltaTime * lerpRate);
    }

    void LerpRotation()
    {
        rigid.rotation = Quaternion.Lerp(rigid.rotation, syncRotation, Time.deltaTime * lerpRate);
    }

    [Command]
    void CmdSendPositionToServer(Vector3 _position)
    {
        syncPosition = _position;
        Debug.Log("position command");
    }

    [Command]
    void CmdSendRotationToServer(Quaternion _rotation)
    {
        syncRotation = _rotation;
        Debug.Log("rotation Command");
    }

    [Client]
    void TransmitPosition()
    {
        CmdSendPositionToServer(rigid.position);
        lastPosition = rigid.position;
    }

    [Client]
    void TransmitRotation()
    {
        CmdSendRotationToServer(rigid.rotation);
        lastRotation = rigid.rotation;
    }
    private void FixedUpdate()
    {
        if (isLocalPlayer)
        {
            TransmitPosition();
            TransmitRotation();
        }
        else
        {
            LerpPosition();
            LerpRotation();
        }
    }

}
