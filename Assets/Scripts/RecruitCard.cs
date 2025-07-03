using System;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class RecruitCard : MonoBehaviour
{
    [SerializeField]
    private List<Vector2> _selectedCardPosition;

    [SerializeField]
    private List<CardSO> _recruitCardOption;

    public CardUI _cardPrefab;

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

        Dictionary<CardType, int> cardCounts = CardManager.Instance.GetPlayerCardData();
        var validCards = _recruitCardOption
            .Where(card => !cardCounts.ContainsKey(card.cardType) || cardCounts[card.cardType] < 2)
            .ToList();

        for (var i = 0; i < 4; i++)
        {
            CardUI spawnCard = Instantiate(_cardPrefab, _centerPanel);
            spawnCard.cardSO = validCards[UnityEngine.Random.Range(0, validCards.Count)];
        }

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

    public void FinishRecruit(Action onComplete)
    {
        _centerPanel
            .DOAnchorPosX(-800, 0.3f)
            .OnComplete(() =>
            {
                _bottomPanel
                    .DOAnchorPosX(800, 0.3f)
                    .OnComplete(() =>
                    {
                        foreach (CardSO item in _selectedCard)
                        {
                            CardManager.Instance.AddCard(item);
                        }
                        onComplete?.Invoke();
                    });
            });
    }
}
