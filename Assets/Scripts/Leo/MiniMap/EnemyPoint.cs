//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class EnemyPoint : CharactorMainController
//{
//	// Inspectorでmyself_imageにEnemy_imageを入れておく

//	private void Update()
//	{
//		MapCharactorMotion(myself_image);
//	}

//	public override void MapCharactorMotion(Image image)
//	{
//		base.MapCharactorMotion(image);
//		// localpositionにするのはmapの上を動くから
//		image.transform.localPosition = new Vector2(this.transform.position.x * 10, this.transform.position.z * 10);
//	}

//	// enemyはmap上で動くのでMapMotion関数をオーバーライドする必要はない
//}
