using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(RectTransform))]
public class WorldViewController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler 
{
    [Header("Zoom")]
    [SerializeField] float zoomSpeed = 0.1f;
    [SerializeField] float zoomLerpSpeed = 10f;
    [SerializeField] float maxZoom = 2.5f;

    [Header("Pan")]
    [SerializeField] float panSpeed = 1f;

    [Header("Input")]
    [SerializeField] InputActionAsset inputActions;

    RectTransform world;
    Canvas canvas;
    RectTransform canvasRect;

    InputAction pointAction;
    InputAction scrollAction;
    InputAction panAction;
    InputAction panAltAction;

    bool isPanning = false;
    Vector3 panStartPointerWorldPos;
    Vector3 panStartWorldPos;
    
    float targetScale;

    private bool canPan;

    void Awake()
    {
        world = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();

        var map = inputActions.FindActionMap("World", true);
        pointAction = map.FindAction("Point", true);
        scrollAction = map.FindAction("ScrollWheel", true);
        panAction = map.FindAction("Click", true);

        targetScale = world.localScale.x;
    }

    void OnEnable()
    {
        pointAction.Enable();
        scrollAction.Enable();
        panAction.Enable();
    }

    void OnDisable()
    {
        pointAction.Disable();
        scrollAction.Disable();
        panAction.Disable();
    }

    void Update()
    {
        HandleZoom();
        HandlePanStart();
        HandlePanEnd();
    }

    void LateUpdate()
    {
        ApplyPan();
        ApplyZoom();
        ClampWorldPosition();
    }

    void HandleZoom()
    {
        float scrollValue = scrollAction.ReadValue<Vector2>().y;
        if (Mathf.Abs(scrollValue) < 0.01f) return;

        float dynamicMinZoom = GetMinZoomToFit();
        targetScale += scrollValue * zoomSpeed * 10f; // pas Time.deltaTime pour zoom instantané
        targetScale = Mathf.Clamp(targetScale, dynamicMinZoom, maxZoom);
    }

    void ApplyZoom()
    {
        float currentScale = world.localScale.x;
        if (Mathf.Approximately(currentScale, targetScale)) return;

        Vector2 pointerScreenPos = pointAction.ReadValue<Vector2>();
        RectTransformUtility.ScreenPointToWorldPointInRectangle(world, pointerScreenPos, canvas.worldCamera, out Vector3 pointerWorldPosBefore);

        world.localScale = Vector3.one * Mathf.Lerp(currentScale, targetScale, Time.deltaTime * zoomLerpSpeed);

        RectTransformUtility.ScreenPointToWorldPointInRectangle(world, pointerScreenPos, canvas.worldCamera, out Vector3 pointerWorldPosAfter);
        Vector3 delta = pointerWorldPosAfter - pointerWorldPosBefore;
        world.position -= delta; // ajustement pour garder le point sous la souris
    }

    void HandlePanStart()
    {
        if (!isPanning && panAction.WasPressedThisFrame())
        {
            isPanning = true;

            // point sous la souris en world
            RectTransformUtility.ScreenPointToWorldPointInRectangle(world, pointAction.ReadValue<Vector2>(), canvas.worldCamera, out panStartPointerWorldPos);
            panStartWorldPos = world.position;
        }
    }

    void HandlePanEnd()
    {
        if (isPanning && panAction.WasReleasedThisFrame())
        {
            isPanning = false;
        }
    }

    void ApplyPan()
    {
        if (!isPanning || !canPan) return;

        RectTransformUtility.ScreenPointToWorldPointInRectangle(world, pointAction.ReadValue<Vector2>(), canvas.worldCamera, out Vector3 currentPointerWorldPos);
        Vector3 delta = currentPointerWorldPos - panStartPointerWorldPos;
        world.position = panStartWorldPos + delta * panSpeed;
    }

    void ClampWorldPosition()
    {
        Vector2 canvasSize = canvasRect.rect.size;
        Vector2 worldSize = world.rect.size * world.localScale.x;

        if (worldSize.x <= canvasSize.x && worldSize.y <= canvasSize.y)
        {
            world.anchoredPosition = Vector2.zero;
            return;
        }

        Vector2 min = (canvasSize - worldSize) * 0.5f;
        Vector2 max = -min;

        Vector2 pos = world.anchoredPosition;
        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        world.anchoredPosition = pos;
    }

    float GetMinZoomToFit()
    {
        Vector2 canvasSize = canvasRect.rect.size;
        Vector2 worldSize = world.rect.size;
        float minScaleX = canvasSize.x / worldSize.x;
        float minScaleY = canvasSize.y / worldSize.y;
        return Mathf.Max(minScaleX, minScaleY);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canPan = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canPan = false;
    }
}
