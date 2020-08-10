using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class LoadingCanvas : MonoBehaviour
{
    private const float _BAR_FILL_SPEED = 1.5f;

    [SerializeField]
    private Image _loadingBarImg;

    public UnityEvent OnLoadingFinished;

    private float _barFilledAmmount;

    private int _filesToLoad;

    private int _filesLoaded = 0;

    private void Awake()
    {
        _loadingBarImg.fillAmount = 0.0f;
    }

    private float GetPercentage(int numToCheck)
    {
        return numToCheck == 
            0 ? 0.0001f : (float)numToCheck / (float)_filesToLoad;
    }

    private void Update()
    {
        if (_loadingBarImg.fillAmount < _barFilledAmmount)
        {
            float newFill = _loadingBarImg.fillAmount;

            newFill += Time.deltaTime * _BAR_FILL_SPEED;

            if (newFill > _barFilledAmmount) newFill = _barFilledAmmount;

            _loadingBarImg.fillAmount = newFill;
        }
    }

    private void UpdateBar()
    {
        _barFilledAmmount =
            GetPercentage(_filesLoaded);
    }

    public void SetFilesToLoad(int amount)
    {
        _filesToLoad = amount;
        UpdateBar();
    }

    public void NewFilesLoaded()
    {
        _filesLoaded++;
        UpdateBar();
    }
}
