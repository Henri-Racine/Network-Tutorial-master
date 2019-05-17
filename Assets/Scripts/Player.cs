using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Player : NetworkBehaviour
{
    public float movementSpeed = 10.0f;
    public float rotationSpeed = 10.0f;
    public float jumpheight = 5.0f;
    private bool isGrounded = false;
    private Rigidbody rigid;
    // Use this for initialization
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        // get audio listener from camera 
        AudioListener audiolistener = GetComponentInChildren<AudioListener>();
        // get camera 
        Camera camera = GetComponentInChildren<Camera>();

        // if the current instance is the local player 
        if (isLocalPlayer)
        {
            camera.enabled = true;
            audiolistener.enabled = true;
        }
        else // otherwise
        {
            camera.enabled = false;
            audiolistener.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            HandleInput();
        }
    }

    void Move(KeyCode _key)
    {
        Vector3 position = rigid.position;
        Quaternion rotation = rigid.rotation;
        switch (_key)
        {
            case KeyCode.W:
                position += transform.forward * movementSpeed * Time.deltaTime;
                break;
            case KeyCode.S:
                position += transform.position * movementSpeed * Time.deltaTime;
                break;
            case KeyCode.A:
                rotation *= Quaternion.AngleAxis(-rotationSpeed, Vector3.up);
                break;
            case KeyCode.D:
                rotation *= Quaternion.AngleAxis(rotationSpeed, Vector3.up);
                break;
            case KeyCode.Space:
                if (isGrounded)
                {
                    rigid.AddForce(Vector3.up * jumpheight, ForceMode.Impulse);
                    isGrounded = false;
                }
                break;
        }
        rigid.MovePosition(position);
        rigid.MoveRotation(rotation);
    }

    private void HandleInput()
    {
        KeyCode[] keys =
        {
            KeyCode.W,
            KeyCode.S,
            KeyCode.A,
            KeyCode.D,
            KeyCode.Space
        };

        foreach( var key in keys)
        {
            if (Input.GetKey(key))
            {
                Move(key);
            }
        }
    }

    private void OnCollisionEnter(Collision _col)
    {
        isGrounded = true;
    }

}
