using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> Players = new List<GameObject>();
    public static PlayerManager Instance;
    private int PlayerCount;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Players.Add(playerInput.gameObject);

        Vector3 SpwanPoint = Vector3.zero;

        playerInput.transform.position = SpwanPoint;
        PlayerCount++;
    }
}
