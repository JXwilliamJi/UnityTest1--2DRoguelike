using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	// 敌人会自动追击
	private Transform player;

	// 目标位置
	private Vector2 targetPos;

	// 刚体
	private Rigidbody2D rigidBD;

	// 碰撞体
	private BoxCollider2D colloder;

	private Animator animator;

	// 移动平滑度
	public float smoothing = 3;

	// 攻击强度
	public int loseFood = 10;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		targetPos = transform.position;
		rigidBD = gameObject.GetComponent<Rigidbody2D> ();
		colloder = gameObject.GetComponent<BoxCollider2D> ();
		animator = gameObject.GetComponent<Animator> ();

		GamesManager.instance.enemyList.Add (this);
	}
	
	// Update is called once per frame
	void Update () {
		rigidBD.MovePosition (Vector2.Lerp(transform.position,targetPos,smoothing * Time.deltaTime));
	}

	// 通过调用这个方法追击或者攻击
	public void move () {
		// 获取到到玩家的偏移
		Vector2 offset = player.position - transform.position;

		// 如果玩家的偏移距离小于1,就攻击
		if (offset.magnitude < 1.1f) {
			// 攻击
			animator.SetTrigger("Attack");

			// 玩家收到伤害
			player.SendMessage ("takeDamage",loseFood);

		} else {
			// 追击
			float x = 0, y = 0;
			if (Mathf.Abs (offset.x) > Mathf.Abs (offset.y)) {

				if (offset.x > 0) {
					x = 1;
				} else {
					x = -1;
				}

			} else {
				if (offset.y > 0) {
					y = 1;
				}else {
					y = -1;
				}
			}

			colloder.enabled = false;
			// 先检测
			RaycastHit2D hit = Physics2D.Linecast (targetPos,targetPos + new Vector2(x,y));
			colloder.enabled = true;

			Debug.Log (hit.collider);

			if (hit.transform == null) {
				targetPos += new Vector2 (x, y);
			} else {
				//
				if (hit.collider.tag == "Food" || hit.collider.tag == "Soda") {
					targetPos += new Vector2 (x, y);
				}

			}


		}

	}
}
