using System;
using TilemapLayer;
using UnityEngine;

namespace Player
{
    public class UnitController:MonoBehaviour
    {
        private UnitModel _unitModel;
        private TurnBaseSystem _turnBaseSystem;

        private void Start()
        {
            _turnBaseSystem = TurnBaseSystem.Instance;
        }

        public void Initialize(UnitModel unitModel)
        {
            _unitModel = unitModel;
            var sprite = GetComponent<SpriteRenderer>();
            sprite.sprite = unitModel.UnitData.UnitSprite;
        }
    }

}