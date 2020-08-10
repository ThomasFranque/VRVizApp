using UnityEngine;
using DG.Tweening;

namespace VRWheel.Windows
{
    public class Window : MonoBehaviour
    {
        [SerializeField] private WheelButton _attachedButton = default;

        private void Awake()
        {
            _attachedButton.OnOpen += Open;
            _attachedButton.OnClose += Close;
            OnAwake();
        }

        private void Open()
        {
            OnOpen();
        }

        private void Close()
        {
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