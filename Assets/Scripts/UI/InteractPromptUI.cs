using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractPromptUI : MonoBehaviour
{
    
    private TextMeshProUGUI text;
     
    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        CharacterManager.Instance.player.interactionController.OnInteractorEvent += SetPromptUI;
    }
    
    
    private void SetPromptUI(string str)
    {
        text.text = str;
    }

  
}