using UnityEngine.Events;
using Roguelike.Define;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameCenter : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCanvas;

    private GameObject gameResultPanel;

    PlatformAdapter platformAdapter;

    [SerializeField]
    private ChapterData chapterData;

    [SerializeField]
    private CharacterData characterData;

    private ItemData[] spawnableItemDatas;
    private Dictionary<ItemData, int> gainItem = new Dictionary<ItemData, int>();

    private StageController stageController;

    private bool gameClear = false;

    //Temp Test Button
    public Button AddItem1Btn;
    public Button AddItem2Btn;
    public Button AddItem3Btn;
    public Button AddTestItemBtn;

    public Button ClearGameBtn;

    public Button Confirm;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platformAdapter = new PlatformAdapter();

        if (platformAdapter != null)
        {
            platformAdapter.LinkToPlatform();
            platformAdapter.LoadSpawnableItemData();

            LoadInGameData();

        }

        {
            stageController = GameObject.FindFirstObjectByType<StageController>();
            if (stageController == null)
            {
                Debug.LogError("Fail To Find StageController!");
            }

            else
            {
                stageController.onStageClear.AddListener(GameClear);
            }
        }
 
        InitInGameUIElement();

        //Temp
        AddItem1Btn.onClick.AddListener(AddItem1);
        AddItem2Btn.onClick.AddListener(AddItem2);
        AddItem3Btn.onClick.AddListener(AddItem3);

        AddTestItemBtn.onClick.AddListener(AddTestItem);

        ClearGameBtn.onClick.AddListener(GameClear);

        Confirm.onClick.AddListener(UploadGameResult);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadInGameData()
    {
        LoadSelectionData();
        LoadSpawnableItemData();
    }

    void LoadSelectionData()
    {
        if(platformAdapter.LoadSelectionData())
        {
            chapterData = platformAdapter.selectedChapter;
            characterData = platformAdapter.selectedCharacter;
        }

        else
        {
            Debug.LogError("Fail To Load Selection Data");
        }
    }

    void LoadSpawnableItemData()
    {
        if(platformAdapter.LoadSpawnableItemData())
        {
            spawnableItemDatas = platformAdapter.spawnableItemDatas;
        }

        else
        {
            Debug.LogWarning("SpawnableItemData is Empty!");
        }
    }

    void InitInGameUIElement()
    {
        if(mainCanvas == null)
        {
            GameObject obj = GameObject.FindWithTag("InGame_MainCanvas");
        }

        gameResultPanel = GameObject.FindWithTag("InGame_GameResultPanel");
        if(gameResultPanel == null)
        {
            Debug.LogError("Can't Find InGame ResultPanel by Tag");
            return;
        }
        gameResultPanel.SetActive(false);

    }

    public void AddItem(ItemData pickedItem)
    {
        if (!gainItem.ContainsKey(pickedItem))
        {
            gainItem[pickedItem] = 1;
        }

        else
        {
            gainItem[pickedItem]++;
        }
    }

    void AddItem1()
    {
        AddItem(spawnableItemDatas[0]);
    }

    void AddItem2()
    {
        AddItem(spawnableItemDatas[1]);
    }

    void AddItem3()
    {
        AddItem(spawnableItemDatas[2]);
    }

    void AddTestItem()
    {
        ItemData TestItem = ScriptableObject.CreateInstance<ItemData>();
        TestItem.SetDisplayableImage(spawnableItemDatas[2].GetDisplayableImage());

        AddItem(TestItem);
    }

    void GameClear()
    {
        Debug.Log("클리어 버튼");
        gameResultPanel.SetActive(true);

        DynamicJoystick joysitck = mainCanvas.GetComponentInChildren<DynamicJoystick>();
        joysitck.gameObject.SetActive(false);

        ShowGameResult();


        gameClear = true;

    }

    void ShowGameResult()
    {
        ScrollAblePanel scrollAblePanel = mainCanvas.GetComponentInChildren<ScrollAblePanel>(true);
        scrollAblePanel.SetScrollPanelType(ScrollRect.MovementType.Elastic, LayoutDirection.Grid);

        IDisplayable[] gainedItems = MakeGainItemArray();

        scrollAblePanel.OpenPanel(gainedItems, DisplayableDataType.ItemData);
    }

    void UploadGameResult()
    {
        platformAdapter.ApplyGameResult(gainItem, chapterData, characterData, gameClear);
    }

    IDisplayable[] MakeGainItemArray()
    {
        int count = gainItem.Count;
        IDisplayable[] gainedItem = new IDisplayable[count];

        int index = 0;
        foreach (var pair in gainItem)
        {
            var itemData = pair.Key;
            var value = pair.Value;

            gainedItem[index] = itemData;
            gainedItem[index].SetExtraInfo(value);

            index++;
        }

        return gainedItem;
    }
}
