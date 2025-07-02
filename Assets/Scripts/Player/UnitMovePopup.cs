using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Input;
using TilemapLayer;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class UnitMovePopup:MonoBehaviour
    {
        [SerializeField] private GameObject _movePopup;
        [SerializeField] private Button _confirmYes;
        [SerializeField] private Button _confirmNo;
        private UnitModel _unitModel;
        private UnitPopUpController _unitPopUpController;
        private TurnBaseSystem _turnBaseSystem;

        public void Show(UnitPopUpController unitPopUpController)
        {
            _unitPopUpController = unitPopUpController;
            _movePopup.SetActive(true);
            
            _turnBaseSystem = TurnBaseSystem.Instance;
            _turnBaseSystem.SetActiveUnit(unitPopUpController.CurrentItem);
            _turnBaseSystem.StartPreview();
            
            _confirmYes.onClick.RemoveAllListeners();
            _confirmNo.onClick.RemoveAllListeners();

            _confirmYes.onClick.AddListener(() => OnConfirmYes());
            _confirmNo.onClick.AddListener(() => OnConfirmNo());
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
            _movePopup.SetActive(false);

        }

        public void ShowConfirmPanel()
        {
            gameObject.SetActive(true);
            _movePopup.SetActive(false);
        }
        private void OnConfirmYes()
        {
            Debug.Log("Yes Move");
            _turnBaseSystem.OnMovePerformed();
            HidePanel();
        }

        private void OnConfirmNo()
        {
            Debug.Log("No Move");
            HidePanel();
            _unitPopUpController.ShowActionPanel();
            _turnBaseSystem.OnMoveCanceled();
        }
    }
}