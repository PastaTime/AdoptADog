using UnityEngine;
using XInputDotNetPure;

public class PlayerManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public RuntimeAnimatorController[] animationControllers;
    private RectTransform _playerBounds;

    private void Start()
    {
        _playerBounds = GameObject.FindGameObjectWithTag("PlayerBounds").GetComponent<RectTransform>();

        foreach (var player in GameState.ActivePlayers)
        {
            SpawnPlayer(player);
        }
    }

    public void SpawnPlayer(PlayerIndex playerIndex)
    {
        GameObject player = Instantiate(playerPrefab);
        player.name = "Player" + playerIndex;
        var playerController = player.GetComponent<PlayerController>();
        playerController.playerNumber = playerIndex;
        var animator = player.GetComponent<Animator>();
        animator.runtimeAnimatorController = animationControllers[(int)playerIndex];
        player.transform.localPosition = Utils.RandomPoint(_playerBounds.position, _playerBounds.rect);

        Debug.Log("Spawning Player: " + playerIndex);
    }

    public void DespawnPlayer(PlayerIndex playerIndex)
    {
        var player = GameObject.Find("Player" + playerIndex);
        Destroy(player);
        GameState.ActivePlayers.Remove(playerIndex);
    }

}
