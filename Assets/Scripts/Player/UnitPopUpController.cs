using System;
using TilemapLayer;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Player
{
    public class UnitPopUpController:MonoBehaviour
    {
        [SerializeField] private UnitActionPopUp _popUpAction;
        [SerializeField] private UnitMovePopup _unitMovePopup;
        [SerializeField] private UnitAttackPopup _unitAttackPopup;

        public UnitModel CurrentItem { get; private set; }
        private Vector3 _worldPosition;
        public Vector3 ScreenPos{ get; private set; }
        private Camera _mainCamera;
        private TurnBaseSystem _turnBaseSystem;
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
        }

        public void ShowPopUp()
        {
            gameObject.SetActive(true);
        }

        public void HidePopUp()
        {
            gameObject.SetActive(false);
        }

        private void InitializePopUp(UnitModel item, Vector3 position)
        {
            HideAttackPanel();
            HideMovePanel();
            
            CurrentItem = item;
            _worldPosition = position;
            gameObject.SetActive(true);
            Vector3 screenPos = _mainCamera.WorldToScreenPoint(position);
            screenPos.y += 100f; 
            screenPos.x += 120f;
            ScreenPos = screenPos;
            gameObject.transform.position = screenPos;
            _turnBaseSystem = TurnBaseSystem.Instance;
        }
        public void ShowPopUpAction(UnitModel item, Vector3 position)
        {
            InitializePopUp(item, position);
            _popUpAction.Show(this);
        }

        public void ShowActionPanel()
        {
            _popUpAction.Show(this);
        }
        public void HideActionPanel()
        {
            _popUpAction.HidePopUp();
        }

        public void ShowAttackPanel()
        {
            _unitAttackPopup.Show(this);
        }

        public void HideAttackPanel()
        {
            _unitAttackPopup.HidePanel();
        }
        public void ShowMovePanel()
        {
            _unitMovePopup.Show(this);
        }

        public void HideMovePanel()
        {
            _unitMovePopup.HidePanel();
        }

    }
}