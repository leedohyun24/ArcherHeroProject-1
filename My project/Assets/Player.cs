using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    public CharacterData template;
    private PlayerData data;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        data = new PlayerData(template, template.BaseStats.hp, template.BaseStats.attack);
        Debug.Log($"{template.CharacterName} : 캐릭터 이름 , 시작 HP : {template.BaseStats.hp}, " +
            $"시작 공격력: {template.BaseStats.attack}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
