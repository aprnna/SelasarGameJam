using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CardSO", menuName = "Scriptable Objects/CardSO")]
public class CardSO : ScriptableObject
{
    public Sprite cardSprite;
    public string descripion;
    public CardType cardType;
}
