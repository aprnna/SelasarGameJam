using UnityEngine;

namespace Player
{
    [CreateAssetMenu(fileName = "UnitData", menuName = "Create Unit Data")]
    public class UnitData : ScriptableObject
    {
        [SerializeField] private string _name;
        [SerializeField] private UnitSide _unitSide;
        [SerializeField] private Sprite _unitSprite;
        [SerializeField] private Vector2 _attackRange;
        [SerializeField] private int _moveRange;
        public string Name => _name;
        public UnitSide UnitSide => _unitSide;
        public Sprite UnitSprite => _unitSprite;
        public Vector2 AttackRange => _attackRange;
        public int MoveRange => _moveRange;
    }
    
    public enum UnitSide
    {
        Enemy,
        Player
    }
}