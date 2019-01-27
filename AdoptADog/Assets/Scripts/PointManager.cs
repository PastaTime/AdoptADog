using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class PointManager
{
    private static PointManager _pointManager = null;
    private Dictionary<int, PlayerScoreBar> _registeredPlayers;
    private Dictionary<int, float> _score;

    public float PushPoints = 10;
    public float PosePoints = 15;
    public float WinningPoints = 100;

    private PointManager() {
        _registeredPlayers = new Dictionary<int, PlayerScoreBar>();
        _score = new Dictionary<int, float>();
    }

    public static PointManager GetSingleton() {
        if (_pointManager == null) {
            _pointManager = new PointManager();
        }
        return _pointManager;
    }

    public void Reset()
    {
        _registeredPlayers.Clear();
        _score.Clear();
    }

    public void Register(int player, PlayerScoreBar score) {
        _registeredPlayers.Add(player, score);
        _score.Add(player, 0);
    }

    public void AddPushPoints(int player) {
        if (_score[player] <= PushPoints) {
            _registeredPlayers[player].UpdatePoints(0f);
        } else {
            _score[player] -= PushPoints;
            _registeredPlayers[player].UpdatePoints(_score[player]);
        }
    }

    public void AddPosePoints(int player, float dt) {
        if (_score[player] + PosePoints * dt >= WinningPoints) {
            _registeredPlayers[player].UpdatePoints(WinningPoints);
            EndGame(player);
        } else {
            _score[player] += PosePoints * dt;
            _registeredPlayers[player].UpdatePoints(_score[player]);
        }
    }

    private void EndGame(int player)
    {
        _registeredPlayers[player].PlayerWon();
        Controller.GetSingleton().Enabled = false;
        GameManager.Instance.FinishGame(player);
        Reset();
    }
}
