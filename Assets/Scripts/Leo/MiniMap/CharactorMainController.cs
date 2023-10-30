using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharactorMainController : MonoBehaviour
{

	public Image myself_image;
	public Image map_image;

	// Move ŠÖ”‚ğ‚±‚±‚É‘‚¢‚Ä‚à‚¢‚¢

	public virtual void MapCharactorMotion(Image image)
	{
		// Player, Enemy‚ÌŒü‚¢‚Ä‚¢‚é•ûŒü‚É‡‚í‚¹‚Äimage‚à‰ñ“]
		image.transform.rotation = Quaternion.Euler(0, 0, -transform.rotation.y * 180);
	}

	public virtual void MapMotion()
	{ // Map‚Ì‰æ‘œ‚ğˆÚ“®‚³‚¹‚é(Player‚ªˆÚ“®‚µ‚Ä‚¢‚é‚æ‚¤‚É‚İ‚¹‚é)
		map_image.transform.localPosition = new Vector2(-transform.position.x * 10, -transform.position.z * 10);
	}
}
