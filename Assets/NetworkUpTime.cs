using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Assertions;
public class NetworkUptime : NetworkBehaviour
{
    private NetworkVariable<float> ServerUptimeNetworkVariable = new NetworkVariable<float>();
    private float last_t = 0.0f;


    [SerializeField] private TextMeshPro text;




    private void Start()
    {
        Assert.IsNotNull(text);
    }
    public override void OnNetworkSpawn()
    {
        if (IsServer)
        {
            ServerUptimeNetworkVariable.Value = 0.0f;
            Debug.Log("Server's uptime var initialized to: " + ServerUptimeNetworkVariable.Value);
        }
    }


    void Update()
    {
        var t_now = Time.time;
        if (IsServer)
        {
            ServerUptimeNetworkVariable.Value = ServerUptimeNetworkVariable.Value + 0.1f;
            if (t_now - last_t > 0.5f)
            {
                last_t = t_now;
                Debug.Log("Server uptime var has been updated to: " + ServerUptimeNetworkVariable.Value);
            }
        }


        if (!IsServer)
        {
            text.text = ServerUptimeNetworkVariable.Value.ToString();
        }
    }
}
