using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool _allowPan = true;
    [SerializeField] private bool _allowZoom = true;
    [Header("Panning Variables")]
    [SerializeField] private float _mouseSensitivity;
    [SerializeField] private bool _invertedMovement = true;
    [SerializeField] private float _cameraSpeed = 5f;
    [SerializeField] private Vector2 _cameraBounds = new Vector2(10, 10);
    [SerializeField] private Vector2 _cameraBoundsOffset;

    [Header("Zoom Variables")]
    [SerializeField] private float _minFOV = 10;
    [SerializeField] private float _maxFOV = 40;
    [SerializeField] private float _zoomSensitivity = 5;
    [SerializeField] private float _zoomSpeed = 5;
    [SerializeField] private bool _invertedZoom = true;
    [Header("Gizmos")]

    [SerializeField, Range(0, 100)] private int _gizmosDensity;
    [SerializeField, Range(20, 500)] private int _gizmosDistance = 200;
    [SerializeField] private Color _gizmosColor = Color.white;
    [SerializeField] private bool _drawGizmos = true;

    private Vector3 _lastMousePosition;
    private Transform _initialState;

    private Camera _cam;

    private Transform _panPivot;
    private float _targetFOV;
    private float _zoomInputTime;
    private float _fovT;
    private float _prevZoomIncrement;

    private bool TargetFOVMaxed => _targetFOV == _maxFOV && _targetFOV == _minFOV;
    private bool TargetFOVReached => Mathf.Abs(_targetFOV - _cam.fieldOfView) <= 0.0001f;
    private bool InputLastFrame => _prevZoomIncrement == 0;

    // Start is called before the first frame update
    void Awake()
    {
        _cam = GetComponent<Camera>();

        _panPivot = SpawnGameObject("Pan Pivot");
        _initialState = SpawnGameObject("Initial camera state");

        _targetFOV = _cam.fieldOfView;

        if (_allowPan)
            OnUpdate += DoPan;
        if (_allowZoom)
            OnUpdate += DoZoom;
    }

    private Transform SpawnGameObject(string name)
    {
        Transform t = new GameObject(name).transform;
        t.hideFlags = HideFlags.HideInHierarchy;

        t.transform.parent = transform;
        t.transform.localPosition = Vector3.zero;
        t.transform.localRotation = Quaternion.identity;
        t.parent = default;
        return t;
    }

    // Update is called once per frame
    void Update()
    {
        OnUpdate?.Invoke();
    }

    private void DoPan()
    {
        if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
        {
            _lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(1) || Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - _lastMousePosition;
            if (_invertedMovement)
                delta = -delta;
            _panPivot.Translate(delta.x * _mouseSensitivity, delta.y * _mouseSensitivity, 0);
            _lastMousePosition = Input.mousePosition;
        }
        else
        {
            RestrainPanPivot();
        }

        MoveTowardsPanPivot();
    }

    private void MoveTowardsPanPivot()
    {
        transform.position = Vector3.Lerp(transform.position, _panPivot.position, _cameraSpeed * Time.deltaTime);
    }

    private void RestrainPanPivot()
    {
        Vector3 initialPosition = _initialState.position;
        float rightOffset = (_cameraBoundsOffset.x / 2);
        float upOffset = (_cameraBoundsOffset.y / 2);
        float xHalfExtent = _cameraBounds.x / 2;
        float yHalfExtent = _cameraBounds.y / 2;

        Vector3 topRight = transform.up * (yHalfExtent + upOffset) + transform.right * (xHalfExtent + rightOffset) + initialPosition;
        Vector3 bottomLeft = -transform.up * (yHalfExtent - upOffset) - transform.right * (xHalfExtent - rightOffset) + initialPosition;

        Vector3 localPanPivotPos = transform.InverseTransformPoint(_panPivot.transform.position);
        Vector3 localTopRight = transform.InverseTransformPoint(topRight);
        Vector3 localBottomLeft = transform.InverseTransformPoint(bottomLeft);

        Debug.DrawLine(transform.position, transform.TransformPoint(localTopRight));
        Debug.DrawLine(transform.position, transform.TransformPoint(localBottomLeft));

        if (localPanPivotPos.x >= localTopRight.x)
            localPanPivotPos.x = localTopRight.x - 0.01f;
        if (localPanPivotPos.x <= localBottomLeft.x)
            localPanPivotPos.x = localBottomLeft.x + 0.01f;

        if (localPanPivotPos.y >= localTopRight.y)
            localPanPivotPos.y = localTopRight.y - 0.01f;
        if (localPanPivotPos.y <= localBottomLeft.y)
            localPanPivotPos.y = localBottomLeft.y + 0.01f;

        _panPivot.transform.localPosition = transform.TransformPoint(localPanPivotPos);
    }

    private void DoZoom()
    {
        float increment = Input.GetAxis("Mouse ScrollWheel") * _zoomSensitivity;
        bool inputThisFrame = increment != 0;
        if (inputThisFrame && !InputLastFrame && TargetFOVReached)
        {
            _zoomInputTime = Time.time;
        }
        if (inputThisFrame && _fovT != 0)
            _fovT = 0;
        if (_invertedZoom)
            increment = -increment;
        _targetFOV += increment;
        _targetFOV = Mathf.Clamp(_targetFOV, _minFOV, _maxFOV);

        MoveZoom();
        _prevZoomIncrement = increment;
    }

    private void MoveZoom()
    {
        _fovT += Time.deltaTime * _zoomSpeed;
        _fovT = Mathf.Clamp01(_fovT);
        _cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, _targetFOV, _fovT);
    }

    private Action OnUpdate;

    private void OnDrawGizmosSelected()
    {
        if (_panPivot != default)
            Gizmos.DrawLine(transform.position, _panPivot.position);
#if !UNITY_EDITOR
        return;
#endif

        if (!_drawGizmos) return;
        Vector3 initialPosition = Application.isPlaying ? _initialState.position : transform.position;

        // Draw camera bounds
        float rightOffset = (_cameraBoundsOffset.x / 2);
        float upOffset = (_cameraBoundsOffset.y / 2);
        float xHalfExtent = _cameraBounds.x / 2;
        float yHalfExtent = _cameraBounds.y / 2;

        Vector3 topLeft = transform.up * (yHalfExtent + upOffset) - transform.right * (xHalfExtent - rightOffset) + initialPosition;
        Vector3 topRight = transform.up * (yHalfExtent + upOffset) + transform.right * (xHalfExtent + rightOffset) + initialPosition;
        Vector3 bottomLeft = -transform.up * (yHalfExtent - upOffset) - transform.right * (xHalfExtent - rightOffset) + initialPosition;
        Vector3 bottomRight = -transform.up * (yHalfExtent - upOffset) + transform.right * (xHalfExtent + rightOffset) + initialPosition;

        Gizmos.color = _gizmosColor;

        for (int i = 0; i < 10 + _gizmosDensity; i++)
        {
            Vector3 offset = transform.forward * (i * (_gizmosDistance / (10 + _gizmosDensity)));
            Gizmos.DrawLine(topRight + offset, topLeft + offset);
            Gizmos.DrawLine(topLeft + offset, bottomLeft + offset);
            Gizmos.DrawLine(bottomLeft + offset, bottomRight + offset);
            Gizmos.DrawLine(bottomRight + offset, topRight + offset);
        }

        Gizmos.color = Color.gray;

        Gizmos.DrawRay(topLeft, transform.forward * _gizmosDistance);
        Gizmos.DrawRay(topRight, transform.forward * _gizmosDistance);
        Gizmos.DrawRay(bottomLeft, transform.forward * _gizmosDistance);
        Gizmos.DrawRay(bottomRight, transform.forward * _gizmosDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, transform.forward * _gizmosDistance);
    }
}