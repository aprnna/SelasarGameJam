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

     
        public void Show(UnitPopUpController unitPopUpController)
        {
            _unitPopUpController = unitPopUpController;
            gameObject.SetActive(true);

            _confirmYes.onClick.RemoveAllListeners();
            _confirmNo.onClick.RemoveAllListeners();

            _confirmYes.onClick.AddListener(() => OnConfirmYes());
            _confirmNo.onClick.AddListener(() => OnConfirmNo());
        }
        public void HidePanel()
        {
            gameObject.SetActive(false);
        }
        private void OnConfirmYes()
        {
            Debug.Log("Yes Move");
        }

        private void OnConfirmNo()
        {
            Debug.Log("No Move");
            HidePanel();
            _unitPopUpController.ShowActionPanel();
        }
    }
}