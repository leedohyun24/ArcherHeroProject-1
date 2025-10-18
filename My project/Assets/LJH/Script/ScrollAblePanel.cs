using Roguelike.Define;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Searcher.SearcherWindow.Alignment;
using static UnityEngine.UI.ContentSizeFitter;
using static UnityEngine.UI.ScrollRect;


public class ScrollAblePanel : MonoBehaviour
{
    [SerializeField]
    private GameObject displayableBtnPrefab;

    [SerializeField]
    private GameObject displayableImagePrefab;

    private ScrollRect scrollRect;
    private Transform contentParent;

    private int displayOffset = 0;

    private IDisplayable[] displayedData;

    private DisplayableDataType calledDataType = DisplayableDataType.None;

    private LayoutDirection layoutDirection = LayoutDirection.None;

    void Start()
    {
        gameObject.SetActive(false);

        //사전 설정 필수
        scrollRect = GetComponentInChildren<ScrollRect>();


        if (scrollRect == null)
        {
            Debug.LogWarning("Can't Load ScrollAblePanel !!");
        }
    }

    public void SetScrollPanelType(MovementType movementType, LayoutDirection layout, int spacking = 20)
    {
        if (scrollRect != null)
        {
            bool canScrollVertic = false;
            bool canScrollHorizon = false;

            layoutDirection = layout;

            contentParent = scrollRect.content;

            RectTransform rect = contentParent.GetComponent<RectTransform>();

            switch (layout)
            {
                case LayoutDirection.Vertical:
                    canScrollVertic = true;
                    if (!contentParent.gameObject.TryGetComponent<VerticalLayoutGroup>(out var existvertic))
                    {
                        existvertic = contentParent.gameObject.AddComponent<VerticalLayoutGroup>();
                        SetContentLayoutToVertic(existvertic, rect, spacking);
                    }
                    break;

                case LayoutDirection.Horizontal:
                    canScrollHorizon = true;
                    if (!contentParent.gameObject.TryGetComponent<HorizontalLayoutGroup>(out var existhorizon))
                    {
                        existhorizon = contentParent.gameObject.AddComponent<HorizontalLayoutGroup>();
                        SetContentLayoutToHorizon(existhorizon, rect, spacking);
                    }

                    break;

                case LayoutDirection.Grid:
                    canScrollVertic = true;
                    if (!contentParent.gameObject.TryGetComponent<GridLayoutGroup>(out var existGrid))
                    {
                        existGrid = contentParent.gameObject.AddComponent<GridLayoutGroup>();
                        SetContentLayoutToGrid(existGrid, rect, spacking);
                    }
                    break;
            }

            scrollRect.movementType = movementType;

            scrollRect.vertical = canScrollVertic;
            scrollRect.horizontal = canScrollHorizon;

            ContentSizeFitter SizeFitter = contentParent.AddComponent<ContentSizeFitter>();

            SizeFitter.verticalFit = canScrollVertic ? FitMode.PreferredSize : FitMode.Unconstrained;
            SizeFitter.horizontalFit = canScrollHorizon ? FitMode.PreferredSize : FitMode.Unconstrained;
        }

        else
        {
            Debug.LogWarning("Can't Load ScrollAblePanel !!");
        }
    }

    public void OpenPanel(IDisplayable[] dataArray, DisplayableDataType dataType, int offset = 0)
    {
        gameObject.SetActive(true);
        displayedData = dataArray;
        calledDataType = dataType;

        displayOffset = offset;

        GenerateContent();

    }

    private void GenerateContent()
    {
        if (displayedData == null)
        {
            Debug.LogWarning("ScrollAblePanel GenerateButton Fail!");
            return;
        }

        EraseContents();

        if (calledDataType == DisplayableDataType.ChapterData ||
            calledDataType == DisplayableDataType.CharacterData)
        {
            GenerateButtons();
        }

        else if (calledDataType == DisplayableDataType.ItemData)
        {
            GenerateItemImages();
        }

        PostLayoutSetting();

    }

    void GenerateButtons()
    {
        for (int i = 0; i < displayedData.Length; i++)
        {
            int index = i;

            GameObject button = Instantiate(displayableBtnPrefab, contentParent);
            DisplayableButton displayableDataButton = button.GetComponent<DisplayableButton>();

            if (displayableDataButton == null)
            {
                Debug.LogError("Cast CustumBtn Fail!");
                continue;
            }

            displayableDataButton.SetDisplayableImage(displayedData[index].GetDisplayableImage());
            displayableDataButton.BindingFunction(() => OnItemSelected(index));

        }
    }
    void GenerateItemImages()
    {
        for (int i = 0; i < displayedData.Length; i++)
        {
            int index = i;

            GameObject Itembox = Instantiate(displayableImagePrefab, contentParent);
            DisplayableImageBox displayAbleItemBox = Itembox.GetComponent<DisplayableImageBox>();

            if (displayAbleItemBox == null)
            {
                Debug.LogError("Cast ImageBox Fail!");
                continue;
            }

            displayAbleItemBox.SetDisplayableImage(displayedData[index].GetDisplayableImage());

            Transform textTransform = Itembox.transform.Find("numText");

            string itemAmount = displayedData[index].GetExtraInfo().ToString();

            textTransform.GetComponent<TextMeshProUGUI>().SetText(itemAmount);
        }
    }

    private void GenerateDisplayContent()
    {
        if (displayedData == null)
        {
            Debug.LogWarning("ScrollAblePanel GenerateButton Fail!");
            return;
        }

        EraseContents();

        for (int i = 0; i < displayedData.Length; i++)
        {
            int index = i;

            GameObject button = Instantiate(displayableBtnPrefab, contentParent);
            DisplayableButton displayableDataButton = button.GetComponent<DisplayableButton>();

            if (displayableDataButton == null)
            {
                Debug.LogError("Cast CustumBtn Fail!");
                continue;
            }

            displayableDataButton.SetDisplayableImage(displayedData[index].GetDisplayableImage());
            displayableDataButton.BindingFunction(() => OnItemSelected(index));

        }
    }

    private void SetHolizonScrollOffset(int offset)
    {
        float buttonSize = displayableBtnPrefab.GetComponent<RectTransform>().rect.width;

        if (offset < 0 || offset >= displayedData.Length) return;

        RefreshPanel();

        float spacing = contentParent.GetComponent<HorizontalLayoutGroup>().spacing;
        float viewportWidth = scrollRect.viewport.rect.width;
        float contentWidth = contentParent.GetComponent<RectTransform>().rect.width;

        float buttonCenterX = offset * (buttonSize + spacing) + buttonSize / 2;

        float normalizedX = (buttonCenterX - viewportWidth / 2) / (contentWidth - viewportWidth);
        scrollRect.horizontalNormalizedPosition = Mathf.Clamp01(normalizedX);
    }

    private void OnItemSelected(int selectedIndex)
    {
        LobbyGameCenter lobbyGameCenter = FindFirstObjectByType<LobbyGameCenter>();

        if (lobbyGameCenter == null)
        {
            Debug.LogError("LRGameCenter Is Null");
            return;
        }

        lobbyGameCenter.SetSelectedData(calledDataType, selectedIndex);

        calledDataType = DisplayableDataType.None;
        EraseContents();
        contentParent.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        gameObject.SetActive(false);

    }

    void SetContentLayoutToVertic(VerticalLayoutGroup vertical, RectTransform rect, int spacing)
    {
        vertical.spacing = spacing;
        vertical.childAlignment = TextAnchor.MiddleLeft;

        vertical.childControlHeight = false;
        vertical.childControlWidth = false;

        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
    }

    void SetContentLayoutToHorizon(HorizontalLayoutGroup horizontal, RectTransform rect, int spacing)
    {
        horizontal.spacing = spacing;
        horizontal.childAlignment = TextAnchor.MiddleLeft;

        horizontal.childControlHeight = false;
        horizontal.childControlWidth = false;

        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = Vector2.zero;
    }

    void SetContentLayoutToGrid(GridLayoutGroup existGrid, RectTransform rect, int spacking)
    {
        Vector2 prefabSize = displayableImagePrefab.GetComponent<RectTransform>().rect.size;

        existGrid.cellSize = prefabSize;
        existGrid.spacing = new Vector2(spacking, spacking);
        existGrid.startAxis = GridLayoutGroup.Axis.Horizontal;
        existGrid.startCorner = GridLayoutGroup.Corner.UpperLeft;
        existGrid.childAlignment = TextAnchor.MiddleCenter;
        existGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;

        existGrid.constraintCount = 3;

        //float viewportWidth = scrollRect.viewport.rect.width;
        //int maxColumns = Mathf.FloorToInt((viewportWidth + spacking) / (prefabSize.x + spacking));
        //maxColumns = Mathf.Max(1, maxColumns);

        //existGrid.constraintCount = maxColumns;

        //StartCoroutine(ApplyGridConstraintAfterDelay(existGrid, prefabSize, spacking));
    }

    void PostLayoutSetting()
    {
        switch (layoutDirection)
        {
            case LayoutDirection.Horizontal:
                {
                    SetHolizonScrollOffset(displayOffset);
                    break;
                }
        }


    }

    //private IEnumerator ApplyGridConstraintAfterDelay(GridLayoutGroup grid, Vector2 prefabSize, int spacking)
    //{
    //    yield return new WaitForSeconds(0.1f);

    //    float viewportWidth = scrollRect.viewport.rect.width;
    //    int maxColumns = Mathf.FloorToInt((viewportWidth + spacking) / (prefabSize.x + spacking));
    //    maxColumns = Mathf.Max(1, maxColumns);

    //    grid.constraintCount = maxColumns;
    //}

    void RefreshPanel()
    {
        scrollRect.horizontalNormalizedPosition = 0.0f;
    }


    void EraseContents()
    {
        foreach (Transform preBtn in contentParent)
        {
            Destroy(preBtn.gameObject);
        }
    }
}
