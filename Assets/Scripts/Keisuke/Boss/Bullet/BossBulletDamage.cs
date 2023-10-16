using UnityEngine;
using Cysharp.Threading.Tasks;
using Shun_Player;
public class BossBulletDamage : MonoBehaviour
{
    private BossDataLoader bossDataLoader;
    private int attackPoint;
    public async UniTask Start(){
        bossDataLoader = GetComponent<BossDataLoader>();
        await bossDataLoader.LoadBossData();
        attackPoint = bossDataLoader.bossData.initialDamage;
    }
    private void OnTriggerStay2D(Collider2D other){
        if (other.CompareTag(TagName.Player))
        {
            other.GetComponent<PlayerBase>().Damage(attackPoint);
        }
    }
}
