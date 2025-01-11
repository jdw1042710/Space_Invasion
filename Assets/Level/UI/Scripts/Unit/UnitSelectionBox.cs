using UnityEngine;
using UnityEngine.InputSystem.Android;

public class UnitSelectionBox : MonoBehaviour
{
    private RectTransform boxVisual;

    private Rect box;

    private Vector2 startPos = Vector2.zero;
    private Vector2 endPos = Vector2.zero;

    private void Awake()
    {
        boxVisual = transform.GetChild(0).GetComponent<RectTransform>();
        box = Rect.zero;
        Clear();
    }

    private void DrawVisual()
    {
        // Draw Box Visual
        Vector2 boxStart = startPos;
        Vector2 boxEnd = endPos;

        Vector2 boxCenter = (startPos + endPos) / 2;
        boxVisual.position = boxCenter;

        Vector2 boxSize = new Vector2(
            Mathf.Abs(boxEnd.x - boxStart.x), 
            Mathf.Abs(boxEnd.y - boxStart.y));
        boxVisual.sizeDelta = boxSize;

        // Draw Selection Visual
        box.xMin = Mathf.Min(startPos.x, endPos.x);
        box.xMax = Mathf.Max(startPos.x, endPos.x);
        box.yMin = Mathf.Min(startPos.y, endPos.y);
        box.yMax = Mathf.Max(startPos.y, endPos.y);
    }

    public Rect GetBox()
    {
        return box;
    }

    public void EnterDrawing()
    {
        startPos = Input.mousePosition;
    }

    public void Draw()
    {
        endPos = Input.mousePosition;
        DrawVisual();
    }

    public void Clear()
    {
        startPos = Vector2.zero;
        endPos = Vector2.zero;
        DrawVisual();
    }

    public bool IsBoxVisualized()
    {
        return boxVisual.rect.width > 0 || boxVisual.rect.height > 0;
    }
}
