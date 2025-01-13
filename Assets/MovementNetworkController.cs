using Unity.Netcode;
using UnityEngine;


public class MovementNetworkController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new();


    [Rpc(SendTo.Server)]
    void SubmitPositionRequestServerRpc(Vector3 position, RpcParams rpcParams = default) => Position.Value = position;


    void Update()
    {
        if (IsOwner && !IsServer)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");


            Vector3 movement = new Vector3(moveX, 0f, moveZ) * 5 * Time.deltaTime;
            transform.Translate(movement, Space.World);
            SubmitPositionRequestServerRpc(transform.position);
        }


        if (IsServer)
            transform.position = Position.Value;
    }
}
