using Unity.Collections;
using Unity.Netcode;
using UnityEngine;


public class MovementNetworkController : NetworkBehaviour
{
    public NetworkVariable<Vector3> Position = new(writePerm: NetworkVariableWritePermission.Server);
    private NetworkVariable<FixedString32Bytes> UserName = new();
    private DisplayUserName displayUserName;
    private Camera playerCamera;


    private void Awake()
    {
        Position.OnValueChanged += OnPositionChanged;
        UserName.OnValueChanged += OnUsernameChanged;
        displayUserName = GetComponentInChildren<DisplayUserName>();
    }


    private void OnDestroy()
    {
        Position.OnValueChanged -= OnPositionChanged;
        UserName.OnValueChanged -= OnUsernameChanged;
    }

    private void Start()
    {
        playerCamera = GetComponentInChildren<Camera>();
        if (IsOwner && playerCamera != null ) 
        {
            playerCamera.gameObject.SetActive(true);
        }
        else if(playerCamera != null)
        {   
            playerCamera.gameObject.SetActive(false);
        }
    }

    public override void OnNetworkSpawn()
    {
        if(IsOwner)
        {
            string localUserName = PlayerPrefs.GetString("Username");
            SetUsernameServerRpc(localUserName);
        }
        OnUsernameChanged(string.Empty, UserName.Value);
    }

    [ServerRpc]
    void SubmitPositionRequestServerRpc(Vector3 position)
    {
        Position.Value = position;
    }

    [ServerRpc]
    private void SetUsernameServerRpc(string newUsername)
    {
        UserName.Value = newUsername;
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

    private void OnUsernameChanged(FixedString32Bytes oldValue, FixedString32Bytes newValue)
    {
        if (!IsOwner)
        {
            if (displayUserName != null)
            {
                displayUserName.UpdateUserNameDisplay(newValue.ToString());
            }
        }
    }

    public string GetUserName()
    {
        return UserName.Value.ToString();
    }
}