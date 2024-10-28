using System;

//플레이어의 정보를 담아둠. 
public class CharacterManager : Singleton<CharacterManager>
{
    public Player player;

    protected override void Awake()
    {
        base.Awake();
        player = FindObjectOfType<Player>();
    }
    
}