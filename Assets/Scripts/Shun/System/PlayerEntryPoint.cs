using Shun_Player;
using Shun_Constants;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace Shun_System
{
    public class PlayerEntryPoint : MonoBehaviour
    {
        [field: SerializeField, Header("キャラクターの種類")] public CharacterType characterType { get; private set; }

        private PlayerBase _playerBase;
        private PlayerInput _playerInput;
        void Awake()
        {
            _ = Init(characterType);
        }

        private async UniTask Init(CharacterType type)
        {
            var playerData = await Addressables.LoadAssetAsync<PlayerData>(type.ToString());
            var playerBase = await Addressables.LoadAssetAsync<GameObject>("PBase");
            var playerInput = await Addressables.LoadAssetAsync<GameObject>("PInput");

            _playerBase = Instantiate(playerBase).GetComponent<PlayerBase>();
            _playerInput = Instantiate(playerInput).GetComponent<PlayerInput>();

            _playerInput.Init(_playerBase);
            _playerBase.Init(playerData);
        }
    }
}
