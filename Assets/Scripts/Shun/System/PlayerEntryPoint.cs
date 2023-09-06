using Shun_Player;
using Shun_Constants;
using Shun_UI;
using UnityEngine.AddressableAssets;
using UnityEngine;
using Cysharp.Threading.Tasks;
using wave;

namespace Shun_System
{
    public class PlayerEntryPoint : MonoBehaviour
    {
        [field: SerializeField, Header("キャラクターの種類")] public CharacterType characterType { get; private set; }
        [field: SerializeField, Header("Wave Controller")] public  WaveController waveController { get; private set; }
        [field: SerializeField, Header("Main Camera")] public Camera mainCamera { get; private set; }

        private PlayerBase _playerBase;
        private PlayerInput _playerInput;
        void Awake()
        {
            Init(characterType);
        }

        private async void Init(CharacterType type)
        {
            string playerType = "";
            string rodType = "";
            switch (type)
            {
                case CharacterType.P1:
                    playerType = AddressableAssetAddress.P1;
                    rodType = AddressableAssetAddress.ROD1;
                    break;
                case CharacterType.P2:
                    playerType = AddressableAssetAddress.P2;
                    rodType = AddressableAssetAddress.ROD2;
                    break;
                case CharacterType.P3:
                    playerType = AddressableAssetAddress.P3;
                    rodType = AddressableAssetAddress.ROD3;
                    break;
                default:
                    break;
            }

            var playerData = await Addressables.LoadAssetAsync<PlayerData>(playerType);
            var playerBase = await Addressables.LoadAssetAsync<GameObject>(playerType + "Object");
            var playerInput = await Addressables.LoadAssetAsync<GameObject>(AddressableAssetAddress.PINPUT);

            _playerBase = Instantiate(playerBase).GetComponent<PlayerBase>();
            _playerInput = Instantiate(playerInput).GetComponent<PlayerInput>();

            _playerInput.Init(_playerBase);
            _playerBase.Init(playerData, rodType, mainCamera);

            waveController.Init(_playerBase);

            Destroy(gameObject);
        }
    }
}
