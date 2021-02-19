using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    private GameObject _player;
    
    void Start()
    {
        _player = GameObject.FindWithTag("Player");
    }
}
