using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChange : MonoBehaviour
{
    [SerializeField] private Image image1;
    [SerializeField] private Ease ease1, ease2;
    private Sequence sq;
    [SerializeField] private float x = 1;
    [SerializeField] private float y = 1;

    private void Awake()
    {
        sq.Kill(); // èâä˙âª
        sq = DOTween.Sequence();
        RectTransform rt = image1.transform as RectTransform;

        //sq.Join(rt.DOAnchorPos(new Vector2(x, y), 2.0f).SetRelative().SetEase(ease1).SetLoops(2, LoopType.Restart));

        sq.Join(rt.transform.DOMoveX(-5, 1).SetEase(ease1));
        sq.Join(rt.transform.DOMoveY(-5, 1).SetEase(ease2));
        sq.SetLoops(5, LoopType.Restart);
    }

    public void OnClickLeftButton()
    {
        //sq.SetLoops(5, LoopType.Restart);
    }

    public void OnClickRightButton()
    {

    }
    private void OnDestroy()
    {
        sq.Kill();
    }
}
