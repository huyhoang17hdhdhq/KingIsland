using UnityEngine;
using DG.Tweening;

public class SelectMarker : MonoBehaviour
{
    public Transform target;

    public Transform topLeft;
    public Transform topRight;
    public Transform bottomLeft;
    public Transform bottomRight;

    [Header("Khoảng cách lùi ra ngoài")]
    public float offset = 0.2f;

    [Header("Hiệu ứng DOTween")]
    public float pulseScale = 1.2f;
    public float pulseDuration = 0.5f;

    private Bounds bounds;

    private void Start()
    {
        PlayCornerAnimations();
    }

    private void Update()
    {
        if (target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        UpdateMarkerPosition();
    }

    private void UpdateMarkerPosition()
    {
        if (target.TryGetComponent(out SpriteRenderer sr))
        {
            bounds = sr.bounds;
        }
        else
        {
            Debug.LogWarning("Không tìm thấy SpriteRenderer!");
            return;
        }

        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        topLeft.position = new Vector3(min.x - offset, max.y + offset, 0);
        topRight.position = new Vector3(max.x + offset, max.y + offset, 0);
        bottomLeft.position = new Vector3(min.x - offset, min.y - offset, 0);
        bottomRight.position = new Vector3(max.x + offset, min.y - offset, 0);
    }

    private void PlayCornerAnimations()
    {
        // animation cho từng góc
        AnimateCorner(topLeft);
        AnimateCorner(topRight);
        AnimateCorner(bottomLeft);
        AnimateCorner(bottomRight);
    }

    private void AnimateCorner(Transform corner)
    {
        corner.localScale = Vector3.one;

        corner.DOScale(pulseScale, pulseDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }
}
