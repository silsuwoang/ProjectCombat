using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [System.Serializable]
    public struct SkillSlot
    {
        public KeyCode KeyCode;
        public string TargetSkillKey;
    }
    
    [SerializeField] private LayerMask ignoreLayers;    // 마우스 위치 체크 제외 레이어
    
    [SerializeField] private Unit receiver;
    
    [SerializeField] private SkillSlot baseSkillSlot;   // Mouse 0 슬롯
    [SerializeField] private List<SkillSlot> skillSlots;

    private Vector3 _inputDir;
    
    private Action _baseKeyEvent;   // baseSkillSlot 입력 이벤트
    
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

    private void FixedUpdate()
    {
        if (_inputDir.sqrMagnitude < 0.01f)
        {
            receiver.Stop();
            return;
        }
        
        receiver.Move(_inputDir, Time.fixedDeltaTime);
    }

    private void MoveInput()
    {
        _inputDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        _inputDir.Normalize();
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
}
