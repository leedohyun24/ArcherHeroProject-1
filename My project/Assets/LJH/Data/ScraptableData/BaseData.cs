using UnityEngine;

[System.Serializable]
public struct BaseData
{
    public int hp;
    public int attack;
    public float speed;

    public BaseData(int hp, int atk, float spd)
    {
        this.hp = hp;
        this.attack = atk;
        this.speed = spd;
    }
}
