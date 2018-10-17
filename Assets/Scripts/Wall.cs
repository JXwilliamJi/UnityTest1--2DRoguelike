using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour {

	// 墙的血量
	public int wallHP = 2;

	// 精灵
	public Sprite sprite;

	public void TakeDamager () {
		// 首先血量持续减少
		wallHP -= 1;

		// 然后更新收到攻击的动画
		gameObject.GetComponent<SpriteRenderer> ().sprite = sprite;

		// 血量没了就销毁
		if (wallHP <= 0) {
			Destroy (this.gameObject);
		}
	}
}
