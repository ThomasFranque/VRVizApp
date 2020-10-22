using ArchiveLoad;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VRWheel.Fullscreen;
using VRWheel.Layer;

public class FloatingImageInfo : MonoBehaviour
{
    // All the fields we need to display info
    [SerializeField] private Image _photographImage = default;
    [SerializeField] private TMP_Text _ownerPro = default;
    [SerializeField] private TMP_Text _typePro = default;
    [SerializeField] private TMP_Text _topicPro = default;
    [SerializeField] private TMP_Text _physicalDescrPro = default;
    [SerializeField] private TMP_Text _yearPro = default;
    [SerializeField] private TMP_Text _sizePro = default;
    [SerializeField] private TMP_Text _idPro = default;
    [Header("Other")]
    [SerializeField] private Image _highlighter = default;

    // Stores the fed info
    public ArchiveInfo _targetinfo;

    Vector3 _imageInitialLocalPos;
    LayerImage _linkedImg;
    public bool Pinned { get; private set; }

    // The good old reliable Awake 
    private void Awake()
    {
        // Prevent users from seeing the defaults
        ToggleFields(false);
        _highlighter.enabled = false;
    }

    // Initialize the object with provided info, assigning all the necessary
    // info to their respective fields.
    public void Init(ArchiveInfo info, LayerImage img = null)
    {
        _imageInitialLocalPos = _photographImage.transform.parent.localPosition;
        _linkedImg = img;
        // Store the given info
        _targetinfo = info;

        // Update the fields using given info
        UpdateFields(_targetinfo);

        // Enable the fields again already with the feeded info (disabled on Awake)
        ToggleFields(true);
    }

    // Update all the fields with the given info
    private void UpdateFields(ArchiveInfo info)
    {
        // Bla bla bla assign stuff
        if (_photographImage != null)
            _photographImage.sprite = info.Image;

        if (_ownerPro != null)
            _ownerPro.text = info.Owner;
        if (_typePro != null)
            _typePro.text = "Archive info still has no field for the type"; //! Change later
        if (_topicPro != null)
            _topicPro.text = info.Topic;
        if (_physicalDescrPro != null)
            _physicalDescrPro.text = info.PhysicalDescription;
        if (_yearPro != null)
            _yearPro.text = ParseYear();
        if (_sizePro != null)
            _sizePro.text = string.Format("{0} x {1} cm", info.ImageWidth, info.ImageHeight);
        if (_idPro != null)
            _idPro.text = string.IsNullOrWhiteSpace(info.NumberOriginal) ? info.NumberRelvas : info.NumberOriginal;

        // Get the pretty year string from info
        string ParseYear() => $"{info.StartYear}\n-\n{info.EndYear}";
    }

    // Used to toggle every field to the desired state.
    private void ToggleFields(bool active)
    {
        // This script is used both for basic info and detailed info.
        // Basic info doesn't have all the fields, so to prevent errors,
        // check if they are not null before messing with them.

        if (_photographImage != null)
            _photographImage.enabled = active;

        if (_ownerPro != null)
            _ownerPro.enabled = active;

        if (_typePro != null)
            _typePro.enabled = active;

        if (_topicPro != null)
            _topicPro.enabled = active;

        if (_physicalDescrPro != null)
            _physicalDescrPro.enabled = active;

        if (_sizePro != null)
            _sizePro.enabled = active;

        if (_yearPro != null)
            _yearPro.enabled = active;
    }

    public void Pin()
    {
        FloatingInfoManager.Instance.Pin(this);
        Pinned = true;
        Debug.Log("Pinned!");
    }

    public void Close()
    {
        transform.DOScale(Vector3.zero, 0.5f).SetEase(Ease.OutCirc).OnComplete(() => Destroy(gameObject));
        _linkedImg?.CloseOptions();
    }

    public void ShowZoomed()
    {
        FullscreenImageDisplay.DisplayZoomed(_targetinfo);
    }
    public void Show3D()
    {
        FullscreenImageDisplay.DisplayDual(_targetinfo);
    }

    //! Spaghetti :)
    private Tween _imageTween;
    private bool _hovering;
    private bool _imageOpened;
    public void OnHoverEnter()
    {
        if (_hovering || _imageOpened) return;
        _hovering = true;
        if (_imageTween != null)
        {
            DOTween.Kill(_imageTween.target);
        }
        _imageTween = _photographImage.transform.parent.DOLocalMoveX(_imageInitialLocalPos.x + 30, .7f).SetEase(Ease.OutCirc);
        _highlighter.enabled = true;
    }
    public void OnHoverExit()
    {
        if (!_hovering || _imageOpened) return;
        _hovering = false;
        if (_imageTween != null)
        {
            DOTween.Kill(_imageTween.target);
        }
        _imageTween = _photographImage.transform.parent.DOLocalMoveX(_imageInitialLocalPos.x, .3f).SetEase(Ease.OutCirc);
        _highlighter.enabled = false;
    }
    public void OnFullClick()
    {
        if (!_imageOpened)
        {
            _imageOpened = true;
            if (_imageTween != null)
                DOTween.Kill(_imageTween.target);
            _imageTween = _photographImage.transform.parent.DOLocalMoveX(_imageInitialLocalPos.x + 80, .5f).SetEase(Ease.OutCirc);

        }
        else
        {
            _imageOpened = false;
            if (_imageTween != null)
                DOTween.Kill(_imageTween.target);
            _imageTween = _photographImage.transform.parent.DOLocalMoveX(_imageInitialLocalPos.x + 1, .5f).SetEase(Ease.OutCirc);
        }
    }
}