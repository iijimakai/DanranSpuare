using UnityEngine;
using UnityEngine.UI;

namespace Shun_UI
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private Image _moveStick;

        private Canvas canvas;
        private Image moveStick;
        public void Init()
        {
            canvas = Instantiate(_canvas);
            moveStick = Instantiate(_moveStick);
            moveStick.transform.parent = canvas.transform;
            moveStick.gameObject.SetActive(false);
        }

        public void EnableStick(Vector2 pos)
        {
            moveStick.transform.localPosition = pos;
            moveStick.gameObject.SetActive(true);
        }

        public void DisEnableStick()
        {
            moveStick.gameObject.SetActive(false);
        }
    }
}
