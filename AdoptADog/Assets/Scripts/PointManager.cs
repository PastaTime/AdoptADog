using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointManager
{
    private static PointManager _pointManager = null;
    private Dictionary<int, object> _registeredPlayers;
    private Dictionary<int, int> _score;

    public int PushPoints = 10;
    public int PosePoints = 15;
    public int WinningPoints = 50;

    private PointManager() {
        _registeredPlayers = new Dictionary<int, object>();
        _score = new Dictionary<int, int>();
    }

    public static PointManager getSingleton() {
        if (pointManager == null) {
            pointManager = new PointManager();
        }
        return pointManager;
    }

    public void Register(int player, Object score) {
        _registeredPlayers.Add(player, score);
        _score.Add(player, 0);
    }

    public void AddPushPoints(int player) {
        if (_score[player] <= PushPoints) {
            //set score to 0
        } else {
            _score[player] -= PushPoints;
            //pass player score to ui
        }
    }

    public void AddPosePoints(int player) {
        if (_score[player] + PosePoints >= WinningPoints) {
            EndGame();
        } else {
            _score[player] += PosePoints;
            //pass player score to ui
        }
    }

    private void EndGame() {
        //call new scene
    }
}
