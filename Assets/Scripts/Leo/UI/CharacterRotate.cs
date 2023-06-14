using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CharacterRotate : MonoBehaviour
{
    [SerializeField] private Image image1, image2, image3; // ïœâªÇ≥ÇπÇΩÇ¢ëŒè€ï®
    [SerializeField] private AnimationCurve ease;
    private Sequence sq1, sq2, sq3;
    [SerializeField] private float x1, x2, x3;
    [SerializeField] private float y1, y2, y3;
    [SerializeField] private float xScale1, xScale2, xScale3;
    [SerializeField] private float yScale1, yScale2, yScale3;
    [SerializeField, Header("??????")] private float scaleSize = 1.0f;
    [SerializeField, Header("????")] private float timeSize = 1.0f;

    RectTransform rt1;

    void Start()
    {
        sq1.Kill(); // ???
        sq2.Kill(); // ???
        sq3.Kill(); // ???
        sq1 = DOTween.Sequence();
        sq2 = DOTween.Sequence();
        sq3 = DOTween.Sequence();
        rt1 = image1.transform as RectTransform;
        sq1.Join(image1.transform.DOScale(xScale1 * scaleSize, yScale1 * scaleSize).SetEase(ease).SetLoops(1, LoopType.Incremental));

        sq1.Append(rt1.DOAnchorPos(new Vector2(x1, y1), timeSize).SetRelative().SetEase(ease).SetLoops(1, LoopType.Incremental));
        //sq1.Join(image1.transform.DOScale(xScale1 * scaleSize, yScale1 * scaleSize).SetEase(ease).SetLoops(1, LoopType.Incremental));
    }

    void Update()
    {
        
    }

    // CharacterScene???
    public void OnClickLeftButton() // ?????
    {
        //sq1 = transform.DORotate(vec1, 1.0f).SetRelative().SetEase(ease).SetLoops(1, LoopType.Incremental);
    }

    // CharacterScene???
    public void OnClickRightButton() // ?????
    {
        //sq1.Append(rt1.DOAnchorPos(new Vector2(x1, y1), 2.0f).SetRelative().SetEase(ease).SetLoops(2, LoopType.Incremental));
        //sq1.Join(image1.transform.DOScale(xScale1 * 0.8f, yScale1 * 0.8f).SetEase(ease).SetLoops(2, LoopType.Incremental));

    }

    private void OnDestroy()
    {
        sq1.Kill();
    }
}
