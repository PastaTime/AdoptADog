using UnityEngine;

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

    public void SpawnPlayer(int playerNumber)
    {
        PlayerPrefs.SetInt("Player" + (playerNumber + 1), 1);
        GameObject player = Instantiate(playerPrefab);
        player.name = "Player" + (playerNumber + 1);
        var playerController = player.GetComponent<PlayerController>();
        playerController.playerNumber = playerNumber + 1;
        var animator = player.GetComponent<Animator>();
        animator.runtimeAnimatorController = animationControllers[playerNumber];
        player.transform.localPosition = Utils.RandomPoint(_playerBounds.position, _playerBounds.rect);

        Debug.Log("Spawning Player: " + playerNumber);
    }


    public void DespawnPlayer(int playerNumber)
    {
        PlayerPrefs.SetInt("Player" + (playerNumber + 1), 0);
        var player = GameObject.Find("Player" + (playerNumber + 1));
        Destroy(player);
        GameState.ActivePlayers.Remove(playerNumber);
    }

}
