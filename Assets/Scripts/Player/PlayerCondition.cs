using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChangeHpType
{
    Damage,
    Heal,
    InstantDeath
}

public class PlayerCondition : MonoBehaviour
{
    private StatHandler statHandler;
    public int CurHp { get; private set; }


    public event Action OnDamgeEvent;
    public event Action OnHealEvent;
    public event Action OnDeathEvent;


    private void Awake()
    {
        statHandler = GetComponent<StatHandler>();
    }

    private void Start()
    {
        CurHp = statHandler.maxHp;
    }

    [ContextMenu("Change HP (Test)")]
    public void ChangeHpTest()
    {
        ChangeHp(ChangeHpType.Damage); 
    }


    public void ChangeHp(ChangeHpType type)
    {
        switch (type)
        {
            case ChangeHpType.Damage:
                CurHp = Mathf.Max(CurHp - 1, 0);
                OnDamgeEvent?.Invoke();
                if (!IsAlive())
                    OnDeathEvent.Invoke();
                break;
            case ChangeHpType.Heal:
                CurHp = Mathf.Min(CurHp + 1, statHandler.maxHp);
                OnHealEvent?.Invoke();
                break;
            case ChangeHpType.InstantDeath:
                CurHp = 0;
                OnDeathEvent?.Invoke();
                break;
        }
    }

    private bool IsAlive()
    {
        if (CurHp < 0)
            return false;
        
        return true;
    }
}