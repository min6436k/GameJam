using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum SlotColor
{
    Able,
    Unable,
    Disable,
    None
}

public class Slot
{
    public GameObject SlotObj;

    public Item InItemObj;

    public Slot ParentSlot;
    public List<Slot> ChildSlot = new List<Slot>();
    public SpriteRenderer SpriteRenderer;

    public Color Color
    {
        get => SpriteRenderer.color;
        set => SpriteRenderer.color = value;
    }

    public void Clear()
    {
        ChildSlot.Clear();
        ParentSlot = null;
    }

    public bool IsEmpty => ParentSlot == null && Disable == false;
    public bool Disable = false;
}


public class GridSystem : MonoBehaviour
{
    public Vector3 RectOffSet => transform.position;

    public Slot[,] Slots;
    public Rect storageBounds;

    public GameObject slotObj;
    public GridData gridData;

    private RectInt _gridBounds;
    private SpriteRenderer _spriteRenderer;
    private List<Vector2Int> _lastSelectSlots = new();
    private bool _isSetAble;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _gridBounds = new RectInt(0, 0, (int)_spriteRenderer.size.x, (int)_spriteRenderer.size.y);

        Slots = new Slot[_gridBounds.xMax, _gridBounds.yMax];

        for (int y = 0; y < _gridBounds.yMax; y++)
        {
            for (int x = 0; x < _gridBounds.xMax; x++)
            {
                Slot instance = Slots[x, y] = new Slot();
                instance.SlotObj = Instantiate(slotObj, transform);
                instance.SlotObj.transform.localPosition = new Vector3(x, y, -0.1f);
                instance.SpriteRenderer = instance.SlotObj.GetComponent<SpriteRenderer>();
                
                //2차원 배열의 인덱스와 유니티 좌표계는 상하가 반전됨. 
                if (gridData.grid[x + (_gridBounds.yMax - 1 - y) * _gridBounds.yMax])
                {
                    instance.Disable = true;
                    instance.Color = Color.gray;
                }
            }
        }
    }
    
    void OnGUI()
    {
        for (int x = 0; x < _gridBounds.xMax; x++)
        {
            for (int y = 0; y < _gridBounds.yMax; y++)
            {
                Vector2 a = GameManager.Instance.MainCamera.WorldToScreenPoint(Slots[x,y].SlotObj.transform.position+new Vector3(0,1));
                a.y = Screen.height - a.y;
                GUI.Label(new Rect(a.x, a.y, 300, 20), Slots[x,y].IsEmpty.ToString());
            }
        }
    }

    public Vector3 SetSlot(Vector3 target, List<Vector2Int> childSlots, Item itemInfo)
    {
        SetSlotColor(SlotColor.None);

        Vector3Int temp = Vector3Int.RoundToInt(target - RectOffSet);

        Slots[temp.x + childSlots[0].x, temp.y + childSlots[0].y].InItemObj = itemInfo;

        foreach (Vector2Int childSlot in childSlots)
        {
            Slots[temp.x + childSlots[0].x, temp.y + childSlots[0].y].ChildSlot
                .Add(Slots[temp.x + childSlot.x, temp.y + childSlot.y]);

            Slots[temp.x + childSlot.x, temp.y + childSlot.y].ParentSlot =
                Slots[temp.x + childSlots[0].x, temp.y + childSlots[0].y];
        }

        return temp + RectOffSet;
    }

    public void UnSetSlot(Vector3 target, List<Vector2Int> childSlots)
    {
        Vector3Int temp = Vector3Int.RoundToInt(target - RectOffSet);

        foreach (Vector2Int childSlot in childSlots)
        {
            Slots[temp.x + childSlots[0].x, temp.y + childSlots[0].y].Clear();

            Slots[temp.x + childSlot.x, temp.y + childSlot.y].ParentSlot = null;
        }
    }

    public bool CheckGridBound(Vector3 target, List<Vector2Int> childSlots)
    {
        SetSlotColor(SlotColor.None);

        Vector3Int offsetTarget = Vector3Int.RoundToInt(target - RectOffSet);

        childSlots = childSlots.Select(x => x + (Vector2Int)offsetTarget).ToList();

        _isSetAble = childSlots.All(x => _gridBounds.Contains(x) && Slots[x.x, x.y].IsEmpty);

        _lastSelectSlots = childSlots.Where(x => _gridBounds.Contains(x)).ToList();

        SetSlotColor(_isSetAble ? SlotColor.Able : SlotColor.Unable);
        return _isSetAble;
    }

    public bool CheckStorageBound(Vector3 target, List<Vector2Int> childSlots)
    {
        List<Vector2> normalizeList = childSlots.Select(v => v + (Vector2)target).ToList();

        return normalizeList.All(x => storageBounds.Contains(x));
    }

    public void SetSlotColor(SlotColor slotColor)
    {
        _lastSelectSlots.ForEach(x =>
        {
            if (Slots[x.x, x.y].Disable) return;
        
            Slots[x.x, x.y].Color = slotColor switch
            {
                SlotColor.Able => Color.green,
                SlotColor.Unable => Color.red,
                SlotColor.None => Color.clear
            };
        });
    }
}