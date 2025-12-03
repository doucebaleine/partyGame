using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance;
    public string JoinCode { get; private set; }

    private Allocation allocation;
    private JoinAllocation joinAllocation;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    // --- Host ---
    public async Task StartHostAsync()
    {
        allocation = await RelayService.Instance.CreateAllocationAsync(1); // 1 client max
        JoinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);

        Debug.Log($"Host Relay Join Code: {JoinCode}");

        // Configure transport
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetRelayServerData(AllocationUtils.ToRelayServerData(allocation, "dtls"));

        // Start Host
        NetworkManager.Singleton.StartHost();
    }

    // --- Client ---
    public async Task StartClientAsync(string joinCode)
    {
        joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);

        // Configure transport
        var unityTransport = NetworkManager.Singleton.GetComponent<UnityTransport>();
        unityTransport.SetRelayServerData(AllocationUtils.ToRelayServerData(joinAllocation, "dtls"));


        // Start Client
        NetworkManager.Singleton.StartClient();
    }
}