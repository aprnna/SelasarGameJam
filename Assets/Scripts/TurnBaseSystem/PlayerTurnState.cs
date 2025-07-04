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

        }

        public override void OnExit()
        {
        }
    }
}