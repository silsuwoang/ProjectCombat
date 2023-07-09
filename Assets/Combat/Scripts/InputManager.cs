using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class InputManager : MonoBehaviour
{
    [System.Serializable]
    public struct SkillSlot
    {
        public KeyCode KeyCode;
        public string TargetSkillKey;
    }

    [SerializeField] private LayerMask ignoreLayers;
    
    
    [SerializeField] private Unit receiver;
    
    [SerializeField] private SkillSlot baseSkillSlot;
    [SerializeField] private List<SkillSlot> skillSlots;

    private Action _baseKeyEvent;
    
    public void Init()
    {
        ResetBaseKeyEvent();
    }
    
    private void Update()
    {
        MoveInput();

        if (Input.GetKeyDown(baseSkillSlot.KeyCode))
        {
            _baseKeyEvent?.Invoke();
        }
        
        foreach (var skillSlot in skillSlots)
        {
            if (Input.GetKeyDown(skillSlot.KeyCode))
            {
                receiver.CastSkill(skillSlot.TargetSkillKey);
            }
        }
    }

    private void MoveInput()
    {
        var dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        dir.Normalize();

        if (dir.sqrMagnitude < 0.01f)
        {
            receiver.Stop();
            return;
        }
        
        receiver.Move(dir);
    }

    public bool GetMouseWorldPosition(out Vector3 pos)
    {
        var layer = (1 << ignoreLayers);
        var mainCam = GameManager.Instance.MainCam;
        var ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out var hit, Mathf.Infinity, layer))
        {
            pos = hit.point;
            return true;
        }

        pos = Vector3.zero;
        return false;
    }

    public void SetBaseKeyEvent(Action onClick)
    {
        _baseKeyEvent = onClick;
    }
    
    public void ResetBaseKeyEvent()
    {
        _baseKeyEvent = () =>
        {
            receiver.CastSkill(baseSkillSlot.TargetSkillKey);
        };
    }
}
