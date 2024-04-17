using Photon.Pun;
using UnityEngine.UI;
using Valve.VR.InteractionSystem;

public interface IGameTrigger
{
    float timeCount { get; set; }
    int scoreCount  { get; set; }
    bool isGameStart { get; set; }
    void OnGameStart();
    void OnGameTimeUp();
}