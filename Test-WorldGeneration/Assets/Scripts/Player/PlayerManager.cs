using UnityEngine;
using System.Collections.Generic;

public class PlayerManager : MonoBehaviour
{
    public List<GameObject> Players;
    public static PlayerManager Instance;

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
}
