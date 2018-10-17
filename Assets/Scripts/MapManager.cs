using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour {

	public GameObject[] floors;
	public GameObject[] outWalls;

	// 障碍物列表
	public GameObject[] walls;

	// 食物列表
	public GameObject[] foods;

	// 敌人列表
	public GameObject[] enemies;

	// 离开的游戏实体
	public GameObject exitObj;
 	
	// 障碍物地址列表
	private List<Vector2> positionList = new List<Vector2> ();

	// 获取游戏控制器
	private GamesManager gameManager; 

	public int rows = 10; // 行
	public int cols = 10; // 列

	// 障碍物的最小个数和最大个数
	public int minObjCount = 2;
	public int maxObjCount = 8;

	// 背景
	private Transform MapHolder;

	// 障碍物
	private Transform SenseHolder;

	// 物资
	private Transform foodHolder;

	// 敌人
	private Transform enemyHolder;

	// Use this for initialization
	void Start () {

		// 获取游戏控制器
		gameManager = this.GetComponent<GamesManager>();

		// 创建地图
		createMap ();

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	// 创建地图的方法
	private void createMap () {

		// 创建一个名称为Map的游戏物体收纳地图
		MapHolder = new GameObject ("Map").transform;

		// 创建一个关卡收纳
		SenseHolder = new GameObject ("Sense").transform;

		// 创建一个食物收纳
		foodHolder = new GameObject ("Food").transform;

		// 创建一个怪物收纳
		enemyHolder = new GameObject ("Enemy").transform;

		// 开始创建地板和围墙
		for (int i = 0; i < cols; i++) {
			for (int j = 0; j < rows; j++) {
				// 新建围墙
				if (i == 0 || j == 0 || i == cols - 1 || j == rows - 1) {

					// 随机生成,先生成一个随机数

					int index = Random.Range (0,outWalls.Length);

					GameObject outWall = GameObject.Instantiate (outWalls [index], new Vector3 (i, j, 0), Quaternion.identity);

					outWall.transform.SetParent (MapHolder);
				} else { // 创建地板

					int index = Random.Range (0,floors.Length);
					GameObject floor = GameObject.Instantiate (floors[index], new Vector3 (i, j, 0), Quaternion.identity);
					floor.transform.SetParent (MapHolder);
				}

			}
		}

		// 图片地址位置数组
		positionList.Clear ();
		for (int i = 2; i < cols - 2; i++) {
			for (int j = 2; j < rows - 2; j++) {
				positionList.Add (new Vector2 (i,j));
			}
		}

		// 创建障碍物
		// 障碍物个数
		int wallCount = Random.Range(minObjCount,maxObjCount + 1);

		// 添加障碍物
		for (int count = 0; count < wallCount; count++) {
			// 随机获取到位置,然后删除位置
			Vector2 positon = randomPos();

			// 创建障碍物
			GameObject sense = GameObject.Instantiate(randomProfab(walls),positon,Quaternion.identity);
			sense.transform.SetParent (SenseHolder);
		}

		// 创建食物

		// 食物的个数随机,范围是2个到2*level 之间的随机数
		int foodCount = Random.Range(2,gameManager.level * 2);

		for (int index = 0; index < foodCount; index++){

			// 获取随机位置
			Vector2 position = randomPos();

			// 创建食物
			GameObject food = GameObject.Instantiate(randomProfab(foods),position,Quaternion.identity);
			food.transform.SetParent (foodHolder);
		}

		// 创建敌人
		int enemyCount = gameManager.level / 2 + 1;

		for (int index = 0; index < enemyCount; index++) {
			// 获取随机位置
			Vector2 position = randomPos();
			// 创建敌人

			GameObject enemy = GameObject.Instantiate (randomProfab(enemies),position,Quaternion.identity);
			enemy.transform.SetParent (enemyHolder);
		}

		// 添加离开的图标

		Vector2 exitPosition = new Vector2 (cols - 2, rows - 2);
		GameObject.Instantiate (exitObj,exitPosition,Quaternion.identity);

	}

	// 获取随机位置
	private Vector2 randomPos () {
		// 随机获取地址数组的下标
		int positionIndex = Random.Range(0,positionList.Count);
		// 随机获取到位置,然后删除位置
		Vector2 positon = positionList [positionIndex];
		positionList.Remove (positon);

		return positon;
	}

	// 后去随机的profabs 预制体
	private GameObject randomProfab (GameObject[] profabs) {
		int index = Random.Range (0, profabs.Length);
		return profabs[index];
	}

}
