using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorMainController : MonoBehaviour
{

	public Image myself_image;
	public Image map_image;

	// Move 関数をここに書いてもいい

	public virtual void MapCharactorMotion(Image image)
	{
		// Player, Enemyの向いている方向に合わせてimageも回転
		image.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.y * 180);
	}

	public virtual void MapMotion()
	{ // Mapの画像を移動させる(Playerが移動しているようにみせる)
		map_image.transform.localPosition = new Vector2(-transform.position.x * 10, -transform.position.z * 10);
	}
}
