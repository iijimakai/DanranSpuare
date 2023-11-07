using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorMainController : MonoBehaviour
{

	public Image myself_image;
	public Image map_image;

	// Move �֐��������ɏ����Ă�����

	public virtual void MapCharactorMotion(Image image)
	{
		// Player, Enemy�̌����Ă�������ɍ��킹��image����]
		image.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.y * 180);
	}

	public virtual void MapMotion()
	{ // Map�̉摜���ړ�������(Player���ړ����Ă���悤�ɂ݂���)
		map_image.transform.localPosition = new Vector2(-transform.position.x * 10, -transform.position.z * 10);
	}
}
