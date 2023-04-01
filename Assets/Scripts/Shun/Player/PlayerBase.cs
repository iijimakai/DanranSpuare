using UnityEngine;
using Shun_Constants;

namespace Shun_Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerBase : MonoBehaviour
    {
        private PlayerData data;

        public void Init(PlayerData _data)
        {
            data = _data;
        }

        /// <summary>
        /// ÀÛ‚É“®‚©‚·
        /// </summary>
        /// <param name="moveVec">ˆÚ“®ƒxƒNƒgƒ‹</param>
        public void Move(Vector2 moveVec)
        {
            transform.Translate(moveVec * data.speed * Time.deltaTime);
        }
    }
}
