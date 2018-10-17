using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


	// 移动间隔时间,游戏人物需要休息1秒
	public float restTime = 1;

	// 平滑速度
	public float smoothing = 1;

	// 记录目标位置
	private Vector2 targetPos = new Vector2(1,1);

	// 刚体,给物体添加物理属性
	private Rigidbody2D rigidBD;

	// 计时器
	private float restTimer = 0;

	// 碰撞体
	private BoxCollider2D collider;

	// 获取动画状态机
	private Animator animator;

	// Use this for initialization
	void Start () {
		rigidBD = gameObject.GetComponent<Rigidbody2D> ();
		collider = gameObject.GetComponent<BoxCollider2D> ();
		animator = gameObject.GetComponent<Animator> ();
	}

	// Update is called once per frame
	void Update () {

		// 通过刚体来控制物体移动
		rigidBD.MovePosition (Vector2.Lerp(transform.position,targetPos,smoothing * Time.deltaTime * 3));

		// 计算到目前为止禁止的时间
		restTimer += Time.deltaTime;

		// 1秒移动一次
		if (restTimer < restTime) {
			return;
		}

		// 获取横轴
		float h = Input.GetAxisRaw("Horizontal");
		// 获取纵轴
		float v = Input.GetAxisRaw ("Vertical");
		// 
		if (h > 0) {
			v = 0;
		}

		// 有输入移动就改变坐标
		if (h != 0  || v != 0) {
			// 移动就会消耗食物
			GamesManager.instance.reduceFood(1);
			// 检测碰撞
			collider.enabled = false;
			// 获取到是否碰撞,是否有交集
			RaycastHit2D hit = Physics2D.Linecast (targetPos,targetPos + new Vector2(h,v));
			collider.enabled = true;

			// 没有交集就继续移动
			if (hit.transform == null) {
				// 位置改变
				targetPos += new Vector2 (h, v);


			} else {

				switch (hit.collider.tag) {
				case "Outwall":
					Debug.Log ("Outwall");
					break;
				case "InnerWall":
//					Debug.Log ("Innerwall");
					// 城墙损坏
					hit.collider.SendMessage("TakeDamager");
					// 播放攻击动画
					animator.SetTrigger("Attack");
					break;
				case "Food":
					// 角色食物增加
					GamesManager.instance.addFood(10);
					// 食物销毁
//					hit.collider.SendMessage("");
					Destroy(hit.transform.gameObject);

					// 角色继续移动
					targetPos += new Vector2 (h, v);

					break;
				case "Soda":
					GamesManager.instance.addFood(20);
					// 角色继续移动
					targetPos += new Vector2 (h, v);
					// 食物销毁
					Destroy(hit.transform.gameObject);
					break;
				}
			}

			// 只要移动了,就调用
			GamesManager.instance.playerMove ();

			// 重置
			restTimer = 0;
		}
	}


	public void takeDamage (int food) {
		GamesManager.instance.reduceFood (food);
	}
}
