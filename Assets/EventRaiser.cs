using UnityEngine;

public class EventRaiser : MonoBehaviour
{
    [SerializeField]
    private VoidEvent OnSpawnDetailed;

    public void Raise()
    {
        OnSpawnDetailed.Raise();
    }
}
