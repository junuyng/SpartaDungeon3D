using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "PlayerDataSO" , menuName = "default/PlayerData")]
class PlayerDataSO : ScriptableObject
{
    public int maxHp;
    public float moveSpeed;
    public float jumpPower;
}