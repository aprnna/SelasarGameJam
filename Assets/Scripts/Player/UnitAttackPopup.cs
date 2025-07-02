using TilemapLayer;
using UnityEngine;
using UnityEngine.UI;

namespace Player
{
    public class UnitAttackPopup:MonoBehaviour
    {
        [SerializeField] private Button _confirmYes;
        [SerializeField] private Button _confirmNo;
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
            Debug.Log("Yes Attack");
        }

        private void OnConfirmNo()
        {
            Debug.Log("No Attack");
            HidePanel();
            _unitPopUpController.ShowActionPanel();
        }
    }
}