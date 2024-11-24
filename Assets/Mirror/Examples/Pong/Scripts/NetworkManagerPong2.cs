using UnityEngine;
namespace Mirror.Examples.Pong
{
    public class NetworkManagerPong2 : NetworkManager
    {
        public Transform left, right;
        GameObject ball;

        public override void OnServerAddPlayer(NetworkConnectionToClient conn)
        {
            Transform startPoint = numPlayers == 0 ? left : right;
            var player = Instantiate(playerPrefab, startPoint);
            NetworkServer.AddPlayerForConnection(conn, player);
            if (numPlayers == 2)
            {
                ball = Instantiate(spawnPrefabs.Find(t => t.name == "ball"));
                NetworkServer.Spawn(ball);
            }
        }

    }
}
