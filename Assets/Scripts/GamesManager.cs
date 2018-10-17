using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamesManager : MonoBehaviour {

	// 为了全局能够访问,设置单例模式
	private static GamesManager _instance;
	// 
	public static GamesManager instance {
		get {
			return _instance;
		}
	}

	// 敌人列表
	public List<Enemy> enemyList = new List<Enemy>();

	// 当前关卡等级
	public int level = 1;

	// 角色食物,默认是100个
	public int food = 100;

	// 是否休息,默认休息一局
	private bool sleepStep = true;

	// 唤醒调用
	void Awake () {
		_instance = this;
	}

	// 增加食物
	public void addFood(int nums) {
		food += nums;
	}

	public void reduceFood(int nums) {
		food -= nums;
	}

	// 玩家移动两步,敌人移动一步
	public void playerMove() {
		if (sleepStep) {
			sleepStep = false;
		} else {
			foreach (var enemy in enemyList) {
				enemy.move ();
			} 

			sleepStep = true;
		}
	}
}
