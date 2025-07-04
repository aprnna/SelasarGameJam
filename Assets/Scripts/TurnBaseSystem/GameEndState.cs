using UnityEngine;

namespace Turnbase_System
{
    public class GameEndState:BattleState
    {
        public GameEndState(TurnBaseSystem turnBaseSystem) : base(turnBaseSystem)
        {
            
        }
        public override void OnEnter()
        {
            Debug.Log("GAMEEE ENDDD "+ TurnBaseSystem.BattleResult);
        }

        public override void OnUpdate()
        {
        }

        public override void OnExit()
        {
        }
    }
}