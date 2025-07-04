using Player;
using TilemapLayer;
using UnityEngine;
using UnityEngine.Serialization;

namespace Turnbase_System
{
    public class UIManagerBattle:MonoBehaviour
    {
        [SerializeField] private GameObject _playerCards;
        [SerializeField] private UnitPopUpController _unitPopUpController;
        [SerializeField] private GameObject _explosivePS;
        public UnitPopUpController UnitController => _unitPopUpController;
        public void ShowPlayerCards()
        {
            _playerCards.SetActive(true);
        }
        public void HidePlayerCards()
        {
            _playerCards.SetActive(false);
        }

        public void HideUnitAction()
        {
            _unitPopUpController.HidePopUp();
        }
        public void ShowUnitAction(UnitModel unitModel,Vector3 position)
        {
            _unitPopUpController.ShowPopUpAction(unitModel, position);
        }

        public void ShowConfirmMove()
        {
            _unitPopUpController.ShowConfirmMovePanel();
        }

        public void StartVFXExplosive(Vector3 position)
        {
            Instantiate(_explosivePS, position, Quaternion.identity);
        }
    }
}