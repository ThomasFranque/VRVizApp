using UnityEngine;
using DG.Tweening;

namespace VRWheel.Windows
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private WheelButton _attachedButton = default;
        public bool IsOpen{get; private set;}

        private void Awake()
        {
            _attachedButton.OnOpen += Open;
            _attachedButton.OnClose += Close;
            OnAwake();
        }

        private void Open()
        {
            IsOpen = true;
            OnOpen();
        }

        private void Close()
        {
            IsOpen = false;
            OnClose();
        }

        protected virtual void OnAwake()
        {

        }

        protected virtual void OnOpen()
        {

        }
        protected virtual void OnClose()
        {

        }
        
        protected void KillRunningAnimationsOn(Transform target)
        {
            transform.DOKill();
        }
    }
}