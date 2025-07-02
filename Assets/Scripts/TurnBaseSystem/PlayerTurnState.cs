using Input;
using UnityEngine;

namespace Turnbase_System
{
    public class PlayerTurnState:BattleState
    {
        private Vector2 _mousePos;
        public PlayerTurnState(TurnBaseSystem turnBaseSystem) : base(turnBaseSystem)
        {
            
        }
        public override void OnEnter()
        {
            Debug.Log("PLAYER TURN");
            
        }

        public override void OnUpdate()
        {
            _mousePos = InputManager.Instance.PlayerInput.MousePos.Get();
            TurnBaseSystem.ShowPreview(_mousePos);
        }

        public override void OnExit()
        {
        }
    }
}