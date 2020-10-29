using UnityEngine;
namespace VRWheel.Individuals
{
    public class DetailsSlice : WheelButton
    {
        protected override void OnInit()
        {
            _type = WheelButtonType.Details;
        }
        
        [SerializeField] private GameObject _window = default;

        public void ToggleWindow()
        {
            _window.SetActive(!_window.activeSelf);
        }
    }
}