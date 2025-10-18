using JetBrains.Annotations;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;



[CreateAssetMenu(fileName = "ChapterData", menuName = "Scriptable Objects/ChapterData")]
public class ChapterData : ScriptableObject, IDisplayable
{
    [SerializeField]
    private string chapterName;

    [SerializeField]
    private Sprite chapterImage;

    [SerializeField]
    private int[] owningMapData;

    private int canSeletable = 1;
    public string CharacterName;
    public Sprite CharacterPreview;
    public GameObject CharacterPrefab;



    public int[] MapDatas => owningMapData;
    public string GetDisplayableName() { return chapterName; }
    public Sprite GetDisplayableImage() { return chapterImage; }
    public void SetDisplayableImage(Sprite image) { chapterImage = image; }

    public int GetExtraInfo() { return canSeletable; }

    public void SetExtraInfo(int extraInfo) { canSeletable = extraInfo; }
}
