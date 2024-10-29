using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField]private RectTransform heartPanel;

    [Header("UI Settings")]
    [SerializeField]private float marginOffset;
    [SerializeField]private GameObject heartPrefab;

    
    private List<GameObject> hearts = new List<GameObject>(); 
    
    
    private void Start()
    {
        Init();
        CharacterManager.Instance.player.condition.OnDamgeEvent += UpdateHearts;
        CharacterManager.Instance.player.condition.OnHealEvent += UpdateHearts;

    }


    private void Init()
    {
        
        float heartImgSize = heartPrefab.GetComponent<RectTransform>().sizeDelta.x;  
        int heartImgCount = CharacterManager.Instance.player.statHandler.maxHp;

        heartPanel.sizeDelta = new Vector2(heartImgSize * heartImgCount + marginOffset, heartPanel.sizeDelta.y);

        for (int i = 0; i < CharacterManager.Instance.player.statHandler.maxHp; i++)
        {
            hearts.Add(Instantiate(heartPrefab, transform.position, Quaternion.identity, heartPanel));
        }
        
    }

    private void UpdateHearts()
    {
        int curHp = CharacterManager.Instance.player.condition.CurHp;

        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].SetActive(i < curHp);  
        }
    }
    

}