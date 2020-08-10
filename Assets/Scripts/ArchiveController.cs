using System.Collections;
using UnityEngine;

public class ArchiveController : MonoBehaviour
{
    [SerializeField]
    private VoidEvent OnMoveForward;

    [SerializeField]
    private VoidEvent OnMoveBack;

    [SerializeField]
    private VoidEvent OnConfirm;

    private bool ready = true;

    private void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow)) // forward
        {
            MoveForward();
        }
        else if (Input.GetKey(KeyCode.DownArrow)) // backwards
        {
            MoveBack();
        }
        else if (Input.GetKeyDown(KeyCode.Return))
        {
            Confirm();
        }
    }

    public void Confirm()
    {
        OnConfirm.Raise();
    }

    public void MoveForward()
    {
        if (ready)
        {
            OnMoveForward.Raise();
            StartCoroutine(Wait());
        }
    }

    public void MoveBack()
    {
        if (ready)
        {
            OnMoveBack.Raise();
            StartCoroutine(Wait());
        }
    }

    private IEnumerator Wait()
    {
        ready = false;

        yield return new WaitForSeconds(0.5f);

        ready = true;
    }
}
