using UnityEngine;
using UnityEngine.UI;

public class UIImagePlacement : MonoBehaviour
{
    public Image imageToPlace; // 表示するImage
    public RectTransform canvasRectTransform; // CanvasのRectTransform

    private bool isImageDisplayed = false; // 画像が表示されているかどうかのフラグ

    void Start()
    {
        //if (imageToPlace == null)
        //{
        //    imageToPlace = GetComponentInChildren<Image>();
        //}
    }
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && !isImageDisplayed) // ボタンが押された時かつ画像が表示されていない場合
        //{
        //    Vector3 mousePosition = Input.mousePosition;

        //    // ImageのRectTransformを生成して、マウス位置に設定
        //    RectTransform imageRectTransform = Instantiate(imageToPlace, canvasRectTransform).rectTransform;
        //    imageRectTransform.position = mousePosition;

        //    isImageDisplayed = true; // 画像を表示したのでフラグを更新
        //}
    }
}
