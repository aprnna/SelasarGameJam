using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RecruitCard : MonoBehaviour
{
[SerializeField]
    private List<Vector2> _selectedCardPosition;

    [SerializeField]
    private Vector2 _centerPanelTargetPos;

    private Dictionary<Vector2, bool> _selectedCardPositionValue = new Dictionary<Vector2, bool>();

    private List<CardSO> _selectedCard = new List<CardSO>();

    [SerializeField]
    private RectTransform _centerPanel;

    [SerializeField]
    private RectTransform _bottomPanel;

    void Start()
    {
        for (var i = 0; i < _selectedCardPosition.Count; i++)
        {
            _selectedCardPositionValue.Add(_selectedCardPosition[i], false);
        }

        CardManager.Instance.SpawnCard(_centerPanel);

        _centerPanel
            .DOAnchorPosX(_centerPanelTargetPos.x, 0.4f)
            .OnComplete(() =>
            {
                foreach (Transform item in _centerPanel)
                {
                    CardUI card = item.GetComponent<CardUI>();
                    card.SetOriginalPosition();
                    card.SetRecruitCard();
                    
                    HorizontalLayoutGroup layoutGroup =
                        _centerPanel.GetComponent<HorizontalLayoutGroup>();
                    layoutGroup.enabled = false;
                }
            });
    }

    public Transform GetCenterPanel()
    {
        return _centerPanel;
    }

    public Transform GetBottomPanel()
    {
        return _bottomPanel;
    }

    public Vector2 GetAvailablePosition()
    {
        Vector2 position = new Vector2();
        foreach (var item in _selectedCardPositionValue)
        {
            if (!item.Value)
                position = item.Key;
        }
        return position;
    }

    public bool IsFull()
    {
        return _selectedCard.Count == 3;
    }

    public void AddCard(Vector2 targetPos, CardSO card)
    {
        _selectedCardPositionValue[targetPos] = true;
        _selectedCard.Add(card);
    }

    public void RemoveCard(Vector2 targetPos, CardSO card)
    {
        _selectedCardPositionValue[targetPos] = false;
        _selectedCard.Remove(card);
    }
}
