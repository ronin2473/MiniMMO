using Unity.Netcode;
using UnityEngine;


public class MovementNetworkController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new(writePerm: NetworkVariableWritePermission.Server);
    private NetworkVariable<string> UserName = new NetworkVariable<string>();


    private void Awake()
    {
        Position.OnValueChanged += OnPositionChanged;
    }


    private void OnDestroy()
    {
        Position.OnValueChanged -= OnPositionChanged;
    }


    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            string localUserName = PlayerPrefs.GetString("Username");
            SetUsernameServerRpc(localUserName);
        }
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(Vector3 position)
    {
        Position.Value = position;
    }

    [ServerRpc]
    private void SetUsernameServerRpc(string newUsername)
    {
        UserName.Value = newUsername; // Update the network variable
    }



    private void Update()
    {
        if (IsOwner)
        {
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");


            Vector3 movement = new Vector3(moveX, 0f, moveZ) * 5f * Time.deltaTime;
            transform.Translate(movement, Space.World);
            // Send the new position to the server
            SubmitPositionRequestServerRpc(transform.position);
        }


        if (!IsOwner && !IsServer)
        {
            // Ensure the position is updated for non-owners
            transform.position = Position.Value;
        }
    }


    private void OnPositionChanged(Vector3 oldPosition, Vector3 newPosition)
    {
        if (!IsOwner)
        {
            transform.position = newPosition;
        }
    }

    public string GetUserName()
    {
        return UserName.Value;
    }
}