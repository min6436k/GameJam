using System;
using System.Collections.Generic;
using System.Linq;
using cmdwtf.UnityTools.Dynamics;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSystem : MonoBehaviour
{
    public Item itemSO;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _spriteCenter;
    private Vector3 _startPos;
    private Vector3 Pos => transform.position;
    private FollowPoint _followPoint;
    private DynamicsTransform _dynamicsTransform;
    
    
    //로직 선택 미스로 인한 하드코딩
    private List<Vector2Int> TestRotateChildSlot => itemSO.childSlots.Select(x => RotatePoints(x, RotateState)).ToList();
    private int RotateState => (int)transform.rotation.eulerAngles.z;
    
    
    private bool _onGrid = false;


    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteCenter = new Vector2(itemSO.childSlots.Max(x=>x.x)+1,itemSO.childSlots.Max(x=>x.y)+1)/2;
        _followPoint = GameManager.Instance.followPoints.GetComponent<FollowPoint>();
        _dynamicsTransform = GetComponent<DynamicsTransform>();
        _dynamicsTransform.SetTarget(GameManager.Instance.followPoints);
    }
    
    public void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        _startPos = Pos;
        //레이어(z축) 변경 => 이동 중에 다른 모든 아이템보다 위에 렌더링
        transform.position -= Vector3.forward/10;

        _followPoint.Move(GameManager.Instance.MousePos-Pos);
        _dynamicsTransform.ResetDynamics();
        _dynamicsTransform.enabled = true;
        
        if (_onGrid)  GameManager.Instance.grid.UnSetSlot(Pos, TestRotateChildSlot);

        transform.DOScale(Vector3.one * 1.2f, 0.04f).SetEase(Ease.OutBack).OnComplete(() =>
            transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutBack));
    }

    public void OnMouseDrag()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        GameManager.Instance.grid.CheckGridBound(Pos, TestRotateChildSlot);
        if (Input.mouseScrollDelta.y > 0 || Input.mouseScrollDelta.y < 0)
        {
            transform.Rotate(0, 0, 90 * Mathf.Sign(Input.mouseScrollDelta.y));
            FollowPointUpdate();
            _dynamicsTransform.ResetDynamics();
        }
        else
        {
            FollowPointUpdate();
        }

    }
    
    public void OnMouseUp()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        
        Vector3 targetPos;
        //-2.1인 z축 -2로 레이어 원상복구
        transform.position += Vector3.forward/10;
        if (GameManager.Instance.grid.CheckGridBound(Pos, TestRotateChildSlot))
        {
            _onGrid = true;
            targetPos = GameManager.Instance.grid.SetSlot(Pos, TestRotateChildSlot,itemSO);
        }
        else if (GameManager.Instance.grid.CheckStorageBound(Pos, TestRotateChildSlot))
        {
            targetPos = Pos;
            _onGrid = false;
        }else
        {
            targetPos = _startPos;
            GameManager.Instance.grid.SetSlotColor(SlotColor.None);
            if(_onGrid) targetPos = GameManager.Instance.grid.SetSlot(targetPos, TestRotateChildSlot,itemSO);
        }

        _dynamicsTransform.enabled = false;
        
        transform.DOMove(targetPos, 0.15f).SetEase(Ease.OutQuart);
    }

    private void FollowPointUpdate()
    {
        Vector3 centerPos = RotatePoints(_spriteCenter,RotateState);
        _followPoint.Move(centerPos);
    }
    
    //로직 선택 미스로 인한 하드코딩2
    private Vector3 RotatePoints(Vector3 point, int angle)
    {
        // 0 ~ 360 정규화

        Vector3 rotatedDir = angle switch
            {
                0 => new Vector3(point.x, point.y),
                90 => new Vector3(-point.y, point.x),
                180 => new Vector3(-point.x, -point.y),
                270 => new Vector3(point.y, -point.x)
            };
    
        return rotatedDir;
    }
    private Vector2Int RotatePoints(Vector2Int point, int angle)
    {
        // 0 ~ 360 정규화

        Vector2Int rotatedDir = angle switch
        {
            0 => new Vector2Int(point.x, point.y),
            90 => new Vector2Int(-point.y-1, point.x),
            180 => new Vector2Int(-point.x-1, -point.y-1),
            270 => new Vector2Int(point.y, -point.x-1)
        };
    
        return rotatedDir;
    }

    //현재 사용되지 않음
    // private float GetMouseSpeed()
    // {
    //     Vector3 currentMousePos = Input.mousePosition;
    //     float distance = Vector3.Distance(currentMousePos, _lastMousePos);
    //
    //     _lastMousePos = currentMousePos;
    //     return distance / Time.deltaTime;
    // }
}
