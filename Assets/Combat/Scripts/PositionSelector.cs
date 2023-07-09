using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionSelector : MonoBehaviour
{
    private InputManager _inputManager;
    
    private void Update()
    {
        if (!_inputManager)
        {
            _inputManager = GameManager.Instance.InputManager;
        }

        if (_inputManager.GetMouseWorldPosition(out var pos))
        {
            transform.position = pos;
        }
    }

    public void Enable(Vector3 size)
    {
        transform.localScale = size;
        gameObject.SetActive(true);
    }

    public void Disable()
    {
        gameObject.SetActive(false);
    }
}
