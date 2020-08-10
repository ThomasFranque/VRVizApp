using UnityEngine;

namespace VRWheel
{
    public class Wheel : MonoBehaviour
    {
        [SerializeField] private Transform _holder = default;
        [SerializeField] private Grabber _grabber = default;
        private WheelButton[] _wheelButtons;

        public Transform Holder => _holder;

        private void Start()
        {
            _wheelButtons = GetComponentsInChildren<WheelButton>(false);
            for (int i = 0; i < _wheelButtons.Length; i++)
                _wheelButtons[i].INIT(this);
            _grabber.INIT(this);
        }

        // called by the buttons when opened
        public void ButtonOpened(WheelButton wheelButton)
        {
            for (int i = 0; i < _wheelButtons.Length; i++)
                if (_wheelButtons[i] != wheelButton)
                    _wheelButtons[i].OnOtherButtonOpen(wheelButton);
        }

        // called by the buttons when closed
        public void ButtonClosed(WheelButton wheelButton)
        {
            for (int i = 0; i < _wheelButtons.Length; i++)
                if (_wheelButtons[i] != wheelButton)
                    _wheelButtons[i].OnOtherButtonClose(wheelButton);
        }

        public void CloseAll()
        {
            for (int i = 0; i < _wheelButtons.Length; i++)
                _wheelButtons[i].Close();
        }
    }
}