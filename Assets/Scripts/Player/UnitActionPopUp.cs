using System;
using TilemapLayer;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class UnitActionPopUp:MonoBehaviour
    {
        [SerializeField] private Button _buttonMove;
        [SerializeField] private Button _buttonStay;
        [SerializeField] private Button _buttonAttack;
        private UnitPopUpController _unitPopUpController;
        private TurnBaseSystem _turnBaseSystem;
        private void Start()
        {
            _turnBaseSystem = TurnBaseSystem.Instance;
        }
        public void Show(UnitPopUpController unitPopUpController)
        {
            gameObject.SetActive(true);
            _unitPopUpController = unitPopUpController;

            _buttonMove.onClick.RemoveAllListeners();
            _buttonStay.onClick.RemoveAllListeners();
            _buttonAttack.onClick.RemoveAllListeners();

            _buttonMove.onClick.AddListener(() => OnMoveAction());
            _buttonStay.onClick.AddListener(() => OnStayAction());
            _buttonAttack.onClick.AddListener(() => OnAttackAction());
        }
        public void HidePopUp()
        {
            gameObject.SetActive(false);
        }
        private void OnMoveAction()
        {
            Debug.Log("Move Action " + _unitPopUpController.CurrentItem.UnitData.Name);
            HidePopUp();
            _turnBaseSystem.ShowPlayerMove(_unitPopUpController.CurrentItem);
            _unitPopUpController.ShowMovePanel();
        }

        private void OnStayAction()
        {
            Debug.Log("Stay Action " + _unitPopUpController.CurrentItem.UnitData.Name);
            HidePopUp();
        }

        private void OnAttackAction()
        {
            Debug.Log("Attack Action " + _unitPopUpController.CurrentItem.UnitData.Name);
            HidePopUp();
            _unitPopUpController.ShowAttackPanel();
        }
    }
}