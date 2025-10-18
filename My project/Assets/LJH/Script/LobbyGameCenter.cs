using Roguelike.Define;
using System;
using UnityEngine;
using UnityEngine.UI;

public class LobbyGameCenter : MonoBehaviour
{
    PlatformAdapter platformAdapter;

    [SerializeField]
    private Canvas mainCanvas;

    public Image SelectedCharacterImage;
    public Image SelectedChapterImage;

    public Button CharacterSelectBtn;
    public Button ChapterSelectBtn;
    public Button GameStartBtn;


    public ChapterData[] chapterDatas { get; private set; }

    public CharacterData[] characterDatas { get; private set; }


    [SerializeField]
    private ChapterData selectedChapter;

    [SerializeField]
    private CharacterData selectedCharacter;

    private int ChapterScrollSelectionOffset = 0;
    private int CharacterScrollSelectionOffset = 0;



    void Start()
    {
        platformAdapter = new PlatformAdapter();

        if (platformAdapter != null)
        {
            platformAdapter.LinkToPlatform();

            chapterDatas = platformAdapter.chapterDatas;
            characterDatas = platformAdapter.characterDatas;

            int savedLastSeletedChapter = platformAdapter.LastSeletedChapter;
            int savedLastSeletedCharacter = platformAdapter.LastSeletedCharacter;

            if ((savedLastSeletedChapter > 0 && savedLastSeletedCharacter > 0) &&
                savedLastSeletedChapter < chapterDatas.Length && savedLastSeletedCharacter < characterDatas.Length)
            {
                SetPastGameData(savedLastSeletedChapter, savedLastSeletedCharacter);
            }

            ChapterScrollSelectionOffset = savedLastSeletedChapter < 0 ? 0 : savedLastSeletedChapter;
            CharacterScrollSelectionOffset = savedLastSeletedCharacter < 0 ? 0 : savedLastSeletedCharacter;
        }

        if (mainCanvas == null)
        {
            GameObject obj = GameObject.FindWithTag("Lobby_MainCanvas");
            mainCanvas = obj.GetComponent<Canvas>();
        }

        InitLobbyUIElement();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetSelectedData(DisplayableDataType dataType, int index)
    {
        switch (dataType)
        {
            case DisplayableDataType.None:
                Debug.LogError("Set SelectedData Fail!!");
                return;

            case DisplayableDataType.ChapterData:
                selectedChapter = chapterDatas[index];
                SelectedChapterImage.sprite = chapterDatas[index].GetDisplayableImage();
                ChapterScrollSelectionOffset = index;
                break;

            case DisplayableDataType.CharacterData:
                selectedCharacter = characterDatas[index];
                SelectedCharacterImage.sprite = characterDatas[index].GetDisplayableImage();
                CharacterScrollSelectionOffset = index;
                break;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created


    private void OnCharacterSelect()
    {
        ScrollAblePanel scrollAblePanel = mainCanvas.GetComponentInChildren<ScrollAblePanel>(true);
        scrollAblePanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Horizontal);
        scrollAblePanel.OpenPanel(characterDatas, DisplayableDataType.CharacterData, CharacterScrollSelectionOffset);
    }

    private void OnChapterSelect()
    {
        ScrollAblePanel scrollAblePanel = mainCanvas.GetComponentInChildren<ScrollAblePanel>(true);
        scrollAblePanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Horizontal);
        scrollAblePanel.OpenPanel(chapterDatas, DisplayableDataType.ChapterData, ChapterScrollSelectionOffset);
    }

    private void OnGameStart()
    {
        Debug.Log("게임 시작 버튼 클릭!");

        if (selectedChapter == null || selectedCharacter == null)
        {
            Debug.Log("Selection Not Ready!");
            return;
        }

        platformAdapter.UploadSelectionData(selectedChapter, selectedCharacter);

        platformAdapter.LoadGameScene();
    }

    void InitLobbyUIElement()
    {
        {
            if (CharacterSelectBtn && ChapterSelectBtn && GameStartBtn)
            {
                CharacterSelectBtn.onClick.AddListener(OnCharacterSelect);
                ChapterSelectBtn.onClick.AddListener(OnChapterSelect);
                GameStartBtn.onClick.AddListener(OnGameStart);
            }

            else
            {
                Debug.LogError("(LRGameCenter) Check Selection Btn");
            }
        }

        //차후 Lobby UI가 많아지면 작업
        //{
        //    Image[] images = mainCanvas.GetComponentsInChildren<Image>(true);

        //    foreach (var img in images)
        //    {
        //        if (img.CompareTag("SelectedChapterImage"))
        //        {
        //            SelectedChapterImage = img;
        //            continue;
        //        }
        //    }
        //}


    }

    void SetPastGameData(int savedLastSeletedChapter, int savedLastSeletedCharacter)
    {
        SelectedChapterImage.sprite = chapterDatas[savedLastSeletedChapter].GetDisplayableImage();
        SelectedCharacterImage.sprite = characterDatas[savedLastSeletedCharacter].GetDisplayableImage();
   
        selectedChapter = chapterDatas[savedLastSeletedChapter];
        selectedCharacter = characterDatas[savedLastSeletedCharacter];
    }
}
