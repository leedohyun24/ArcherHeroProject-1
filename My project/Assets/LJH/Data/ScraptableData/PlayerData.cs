using UnityEngine;

public class PlayerData
{
    public BaseData intrinscData;  // Data의 복사본 ..바뀌지않을 값들
    public BaseData currentData; //실시간 버프 반영 값 
    public int Xp;
    public int Level;
    public int currentHp;
    public PlayerData(CharacterData baseStats, int customhp, int customAttack)
    {
        //생성자에서 캐릭터 hp지정 ..
        intrinscData = baseStats.BaseStats;
        currentData = baseStats.BaseStats;
        currentHp = baseStats.BaseStats.hp;
        Level = 1;
        Xp = 0;
   
    }
    public void DamageUp(int damageStats)
    {
        currentData.attack += damageStats;
    }
    public void HpUp(int hpStats)
    {
        currentData.hp += hpStats;
    }
}
