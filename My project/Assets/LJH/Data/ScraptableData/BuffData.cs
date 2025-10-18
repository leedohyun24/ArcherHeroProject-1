using UnityEngine;
using Roguelike.Define;

[CreateAssetMenu(fileName = "BuffData", menuName = "Scriptable Objects/BuffData")]
public class BuffData : ScriptableObject, IDisplayable
{
    private string buffName;

    [SerializeField]
    private BuffType type = BuffType.None;

    [SerializeField]
    private Sprite buffImage;

    [SerializeField]
    int maxObtainable = 0;



    private int canSeletable = 1;
    public string GetDisplayableName()
    {
        return null;
    }
    public Sprite GetDisplayableImage()
    {
        return buffImage;
    }
    public void SetDisplayableImage(Sprite image)
    {
        buffImage = image;
    }
    public int GetExtraInfo()
    {
        return maxObtainable;
    }

    public void SetExtraInfo(int extraInfo)
    {
        maxObtainable = extraInfo;
    }
}
