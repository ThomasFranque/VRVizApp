using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform), typeof(GridLayoutGroup))]
public class DotPopulator : MonoBehaviour
{
    private const string DOT_PATH = "Prefabs/RawFadedDot";
    [SerializeField, Min(0)] private int _dotAmount = 1;

    private GameObject[, ] _dotGrid;
    int xSize;
    int ySize;

    private void Start()
    {
        GameObject d = Resources.Load<GameObject>(DOT_PATH);
        GridLayoutGroup group = GetComponent<GridLayoutGroup>();
        if (group.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
        {
            Debug.Log("To have dots, the gridlayout must have a fixed column count.");
            return;
        }
        xSize = group.constraintCount;
        ySize = _dotAmount / group.constraintCount;
        _dotGrid = new GameObject[xSize, ySize];

        //group.constraintCount;
        int lines = -1;
        for (int i = 0; i < _dotAmount; i++)
        {
            if (i % group.constraintCount == 0)
            {
                // new line
                lines++;
            }
            GameObject dot = Instantiate(d, transform);
            dot.name = $"Dot {i}";
            Debug.Log($"({i % group.constraintCount}, {lines})");
            _dotGrid[i % group.constraintCount, lines] = dot;
        }
        Close();
    }

    public void Close()
    {
        KillAllAnimations();
        AnimateNeighbors(0.01f, 0.15f);
        //AnimateNeighbors(0, 0, 0.01f, 0.1f);
    }
    public void Open()
    {
        KillAllAnimations();
        AnimateNeighbors(1, 0.4f);
        //AnimateNeighbors(0, 0, 1, 0.08f);
    }

    private void AnimateNeighbors(int x, int y, float scaleAmount, float speed, int sign = 1)
    {
        int modX = x + sign;
        int modY = y + sign;
        // Side
        if (modX < xSize)
        {
            _dotGrid[modX, y].transform.DOScale(Vector3.one * scaleAmount, speed)
                .OnComplete(() => AnimateNeighbors(modX, y, scaleAmount, speed, sign));
        }
        // Diagonal
        if (modX < xSize && modY < ySize)
        {
            _dotGrid[modX, modY].transform.DOScale(Vector3.one * scaleAmount, speed)
                .OnComplete(() => AnimateNeighbors(modX, modY, scaleAmount, speed, sign));
        }
        // Vertical
        if (modY < ySize)
        {
            _dotGrid[x, modY].transform.DOScale(Vector3.one * scaleAmount, speed)
                .OnComplete(() => AnimateNeighbors(x, modY, scaleAmount, speed, sign));
        }
    }

    private void AnimateNeighbors(float scaleAmount, float speed)
    {
        foreach (GameObject g in _dotGrid) g.transform.DOScale(Vector3.one * scaleAmount, speed);
    }

    private void KillAllAnimations()
    {
        foreach (GameObject g in _dotGrid) g.transform.DOKill();
    }
}