using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private TableContainer tableContainer;
    public TableContainer TableContainer => tableContainer;
    
    [SerializeField] private InputManager inputManager;
    public InputManager InputManager => inputManager;

    [SerializeField] private Camera mainCam;
    public Camera MainCam => mainCam;

    [SerializeField] private PositionSelector positionSelector;
    public PositionSelector PositionSelector => positionSelector;
    
    [SerializeField] private List<Unit> units;
    
    
    private void Start()
    {
        Application.targetFrameRate = 60;
        
        tableContainer.Init();
        BattleManager.Init();
        inputManager.Init();

        foreach (var unit in units)
        {
            unit.Init();
        }
    }
    
}
