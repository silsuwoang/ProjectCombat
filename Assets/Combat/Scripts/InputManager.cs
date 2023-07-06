using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private Unit unit;
    
    private void Update()
    {
        MoveInput();

        if (Input.GetMouseButtonDown(0))
        {
            unit.CastSkill(0);
        }
    }

    private void MoveInput()
    {
        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir.Normalize();

        if (dir.sqrMagnitude < 0.01f)
        {
            // unit.AnimationController.SetParameter("Speed", 0f);
            unit.AnimationController.SetApplyRootMotion(true);
            return;
        }
        
        // unit.AnimationController.SetParameter("Speed", 1f);
        unit.AnimationController.SetApplyRootMotion(false);
        unit.Move(dir);
    }
}
