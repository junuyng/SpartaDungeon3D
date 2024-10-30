using System;
using TMPro;
using UnityEngine;

public class TreasureChest : MonoBehaviour, IInteractable
{
    [Header("TreasureChest Components")]
    [SerializeField]private GameObject InfoUI;
    [SerializeField]private TextMeshProUGUI text;
    [SerializeField] private Item item;
    private void Awake()
    {
        if(text == null)
              text = InfoUI.GetComponentInChildren<TextMeshProUGUI>();
    }

    public string GetInfo()
    {
        return $"보물 상자입니다.";
    }

    //TODO Player InputController에서 E키를 눌렸을 때와 연동해 작업 처리 
    public void OnInteract()
    {
        CharacterManager.Instance.player.inventory.AddItem(item);
    }
    
    public void InteractBySearch()
    {
        text.text = "E키를 눌러 열기";
        InfoUI.SetActive(true);
    }

    public void UnInteractBySearch()
    {
        InfoUI.SetActive(false);
    }


}