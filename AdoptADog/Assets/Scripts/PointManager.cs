using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using XInputDotNetPure;

public class PointManager : MonoBehaviour
{
    public float PushPoints = 10;
    public float PosePoints = 15;
    public float WinningPoints = 100;
    
    private static PointManager _pointManager = null;
    private Dictionary<PlayerIndex, PlayerScoreBar> _registeredPlayers = new Dictionary<PlayerIndex, PlayerScoreBar>();
    private Dictionary<PlayerIndex, float> _score = new Dictionary<PlayerIndex, float>();
    private ControllerManager _controllerManager;

    void Start()
    {
        _pointManager = FindObjectOfType<PointManager>();
        _controllerManager = FindObjectOfType<ControllerManager>();
    }

    public void Reset()
    {
        _registeredPlayers.Clear();
        _score.Clear();
    }

    public void Register(PlayerIndex player, PlayerScoreBar score) {
        Debug.Log("Registering Player: " + player + "Score: " + score.ToString());
        _registeredPlayers.Add(player, score);
        _score[player] = 0f;
    }

    public void AddPushPoints(PlayerIndex player) {
        
        if (_score[player] <= PushPoints) {
            _registeredPlayers[player].UpdatePoints(0f);
        } else {
            _score[player] -= PushPoints;
            _registeredPlayers[player].UpdatePoints(_score[player]);
        }
    }

    public void AddPosePoints(PlayerIndex player, float dt) {
        Debug.Log("Adding to: " + player);
        Debug.Log("Length Score: " + _score.Count);
        Debug.Log("Length Players: " + _registeredPlayers.Count);
        if (_score[player] + PosePoints * dt >= WinningPoints) {
            _registeredPlayers[player].UpdatePoints(WinningPoints);
            EndGame(player);
        } else {
            _score[player] += PosePoints * dt;
            _registeredPlayers[player].UpdatePoints(_score[player]);
        }
    }

    private void EndGame(PlayerIndex player)
    {
        _registeredPlayers[player].PlayerWon();
        _controllerManager.Enabled = false;
        GameManager.Instance.FinishGame(player);
        Reset();
    }
}
