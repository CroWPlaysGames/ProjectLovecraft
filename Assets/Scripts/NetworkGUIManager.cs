using Unity.Netcode;
using UnityEngine;
    public class NetworkGUIManager : MonoBehaviour
    {

        public bool GUI =true;
        public GameObject GameManager;
        void Awake()
        {
            Cursor.visible = true;
        }
        
        void OnGUI()
        {
            if (GUI = true){ 
                GUILayout.BeginArea(new Rect(10, 10, 300, 300));
                if (!NetworkManager.Singleton.IsClient && !NetworkManager.Singleton.IsServer)
                {
                    if (GUILayout.Button("Server")){NetworkManager.Singleton.StartServer(); NetworkManager.Singleton.ConnectionApprovalCallback = ApprovalCheck;} 
                    if (GUILayout.Button("Host")){NetworkManager.Singleton.StartHost(); } 
                    if (GUILayout.Button("Client")){NetworkManager.Singleton.StartClient();} 
                }
                else
                {
                    if (GUILayout.Button("Ready"))
                    {
                        NetworkLog.LogInfoServer("A player is ready");
                        GameManager.SetActive(true);
                    }
                    if (GUILayout.Button("Disconnect"))
                    {
                        NetworkLog.LogInfoServer("A player has disconnected");
                        NetworkManager.Singleton.Shutdown();            
                    }
                }
                GUILayout.EndArea();
            }else if (GUI = false)
            {
                GameManager.SetActive(true);
            }
        }

        private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest request, NetworkManager.ConnectionApprovalResponse response)
        {
            // The client identifier to be authenticated
            var clientId = request.ClientNetworkId;

            // Additional connection data defined by user code
            var connectionData = request.Payload;

            // Your approval logic determines the following values
            response.Approved = true;
            response.CreatePlayerObject = true;

            // The prefab hash value of the NetworkPrefab, if null the default NetworkManager player prefab is used
            response.PlayerPrefabHash = null;

            // Position to spawn the player object (if null it uses default of Vector3.zero)
            response.Position = Vector3.zero;

            // Rotation to spawn the player object (if null it uses the default of Quaternion.identity)
            response.Rotation = Quaternion.identity;

            // If additional approval steps are needed, set this to true until the additional steps are complete
            // once it transitions from true to false the connection approval response will be processed.
            response.Pending = false;
        }
    }
    