using TMPro;
using UnityEngine.SceneManagement;

namespace UNO.Multiplayer
{
    using Riptide;
    using Riptide.Utils;
    using UnityEngine;

    public enum ServerToClientMessageId : ushort
    {
        StartGame = 1,
        Cards
    }

    public enum ClientToServerMessageId : ushort
    {
        Name = 100,
    }

    public class NetworkManager : MonoBehaviour
    {
        public Server Server => server;
        
        private Server server;

        private Client client;

        [SerializeField] private ushort port = 63235;
        [SerializeField] private ushort maxPlayerCount = 4;

        private int playerCount;
        [SerializeField] private TextMeshProUGUI playerCountText;

        [SerializeField] private GameObject startGameButton;

        private void Awake()
        {
            RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);
        }

        private void Update()
        {
            if (playerCount != 0)
            {
                playerCountText.text = $"Player count: {playerCount}";
            }
        }

        public void StartServer()
        {
            DontDestroyOnLoad(gameObject);
            
            server = new Server();
            server.Start(port, maxPlayerCount);
            server.ClientConnected += (sender, args) =>
            {
                playerCount++;
            };
            server.ClientDisconnected += (sender, args) =>
            {
                playerCount--;
            };
        
            client = new Client();
            client.Connect($"127.0.0.1:{port}");

            startGameButton.SetActive(true);
        }

        public void StartClient(string ip)
        {
            DontDestroyOnLoad(gameObject);
            
            client = new Client();
            client.Connect($"{ip}:{port}");
            client.Disconnected += (sender, args) =>
            {
                SceneManager.LoadScene(0);
            };
        }

        private void FixedUpdate()
        {
            if(server != null)
                server.Update();
            if(client != null)
                client.Update();
        }

        private void OnApplicationQuit()
        {
            if(server != null)
                server.Stop();
            if(client != null)
                client.Disconnect();
        }

        public void StartGame()
        {
            Message message = Message.Create(MessageSendMode.Reliable, ServerToClientMessageId.StartGame);
            
            server.SendToAll(message, client.Id);
            
            SceneManager.LoadScene(1);
        }

        [MessageHandler((ushort)ServerToClientMessageId.StartGame)]
        private static void StartGameMessage(Message message)
        {
            SceneManager.LoadScene(2);
        }
    }
   
}