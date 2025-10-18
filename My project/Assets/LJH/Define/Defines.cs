
namespace Roguelike.Define
{
    public enum DisplayableDataType
    {
        None,
        CharacterData,
        ChapterData,
        ItemData
    }

    public static class RoguelikeScene
    {
        public const string LobbyScene = "LobbyScene";
        public const string GameScene = "InGameScene";
        public const string ResultScene = "ResultScene";
    }

    public enum LayoutDirection
    {
        None,
        Horizontal,
        Vertical,
        Grid
    }

    public enum BuffType
    {
        None,

        AddAtkLow,
        AddAtkMiddle,
        AddAtkHigh,

        AddSpeed,

        AddMaxHp,

        Max
    }
}

