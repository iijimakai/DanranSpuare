using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using wave;
public class EnemyCounterDisplay : MonoBehaviour
{
    [SerializeField]
    private WaveController waveController;

    [SerializeField]
    private TextMeshProUGUI enemiesInfoText;

    private void Start()
    {
        waveController.OnEnemyDestroyed.Subscribe(UpdateDisplay).AddTo(this);
        UpdateDisplay(0);
    }

    private void UpdateDisplay(int destroyedEnemyCount)
    {
        int totalEnemies = waveController.waves[waveController.currentWaveIndex].totalEnemies;
        enemiesInfoText.text = $"Destroyed: {destroyedEnemyCount} / Total: {totalEnemies}";
    }
}
