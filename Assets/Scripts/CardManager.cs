using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CardManager : PersistentSingleton<CardManager>
{
    [SerializeField]
    private List<CardSO> _starterCard;

    [SerializeField]
    private int _maxCard;

    private List<CardSO> _playerCard = new List<CardSO>();
    public CardUI _cardPrefab;
    public ChooseCard _chooseCardPrefab;
    public RecruitCard _recruitCardPrefab;
    public Canvas canvas;

    private ChooseCard _chooseCard;
    private RecruitCard _recruitCard;

    void Start()
    {
        _playerCard.AddRange(_starterCard);
    }

    public void AddCard(CardSO card)
    {
        if (!IsFull())
            _playerCard.Add(card);
    }

    public bool IsFull()
    {
        return _playerCard.Count == _maxCard;
    }

    public void RemoveCard(CardSO card)
    {
        _playerCard.Remove(card);
    }

    public void SpawnCard(Transform cardParent)
    {
        foreach (var item in _playerCard)
        {
            CardUI spawnedCard = Instantiate(_cardPrefab, cardParent);
            spawnedCard.cardSO = item;
        }
    }

    public void SpawnChooseCard()
    {
        _chooseCard = Instantiate(_chooseCardPrefab, canvas.transform);
    }

    public void DestroyChooseCard()
    {
        Destroy(_chooseCard.gameObject);
    }

    public void SpawnRecruitCard()
    {
        _recruitCard = Instantiate(_recruitCardPrefab, canvas.transform);
    }

    public void DestroyRecruitCard()
    {
        Destroy(_recruitCard.gameObject);
    }
}
