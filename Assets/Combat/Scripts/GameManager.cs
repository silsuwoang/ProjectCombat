using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private InputManager inputManager;
    public InputManager InputManager => inputManager;

    [SerializeField] private Camera mainCam;
    public Camera MainCam => mainCam;
    
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        BattleManager.Init();
    }
    
}
