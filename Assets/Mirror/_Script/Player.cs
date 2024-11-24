using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private Vector3 rotate = new Vector3(0, 0, 1);
    private int direction = 1;
    private Vector3 degree;


    public void FixedUpdate()
    {
        if (!isLocalPlayer) { return; }
        if (!Input.GetKey(KeyCode.W)) { return; }
        CmdMove();
    }

    [Command]
    private void CmdMove()
    {
        RpcMove();
    }

    [ClientRpc]
    private void RpcMove()
    {
        transform.Translate(Vector2.up * .1f);
    }

}
