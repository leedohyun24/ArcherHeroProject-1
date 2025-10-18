using UnityEngine;

[CreateAssetMenu(fileName = "CharacterData", menuName = "Scriptable Objects/CharacterData")]
public class CharacterData : ScriptableObject, IDisplayable
{
    [SerializeField]
    private string characterName;

    [SerializeField]
    private Sprite characterPreviewImage;

    [SerializeField]
    private int prefab;

    public int CharacterPrefab => prefab;

    private int canSeletable = 1;

    public string GetDisplayableName() { return characterName; }
    public Sprite GetDisplayableImage() { return characterPreviewImage; }

    public void SetDisplayableImage(Sprite image) { characterPreviewImage = image; }

    public int GetExtraInfo() { return canSeletable; }

    public void SetExtraInfo(int extraInfo) { canSeletable = extraInfo; }

}
