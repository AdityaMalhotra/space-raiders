using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FormationController : MonoBehaviour {

	private float xmin;
	private float xmax;
	private bool rightMovement=true;
	public int currentWave = 1;
	private Text waveNum;
	EnemyBehavior enhancedEnemy;
	PlayerController playerController;	
	private Sprite currentEnemySprite;
				
	public float speed=5f;
	public float height=10f;
	public float width=5f;
	public GameObject EnemyPrefab;
	public float spawnDelay = 0.5f;
	

	void Start () {
		SpawnUntilFull ();	
		Initialize ();
	}
	void SpawnUntilFull()
	{
		Transform freePosition = NextFreePosition();
		if(freePosition)
		{
			GameObject enemy = Instantiate (EnemyPrefab, freePosition.transform.position, Quaternion.identity) as GameObject;
			enemy.transform.parent= freePosition;
		}
		if(NextFreePosition())
		{
			Invoke ("SpawnUntilFull",spawnDelay);
		}
	}
	void Initialize()
	{
		enhancedEnemy = EnemyPrefab.GetComponent<EnemyBehavior>();
		currentEnemySprite = enhancedEnemy.easyEnemy;
		EnemyPrefab.GetComponent<SpriteRenderer>().sprite = currentEnemySprite;
		playerController = GameObject.Find ("Player").GetComponent<PlayerController>();
		playerController.health = 500f;
		waveNum = GameObject.Find ("WaveDisplay").GetComponent<Text>();
		enhancedEnemy.enemyLaserSpeed = 1f;
		enhancedEnemy.fireRate = 0.2f;
		enhancedEnemy.scoreValue = 50;
		enhancedEnemy.health=200;
		
		
		float distance= transform.position.z-Camera.main.transform.position.z;
		Vector3 leftMost=Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost=Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin=leftMost.x;
		xmax=rightMost.x;
	}
	
	public void OnDrawGizmos()
	{
		Gizmos.DrawWireCube (transform.position,new Vector3(width,height));
	}
	
	
	void Update () {
	if(!rightMovement)
		{
			gameObject.transform.position+=Vector3.left * speed * Time.deltaTime;
		}
	else
		{
			gameObject.transform.position+=Vector3.right *speed * Time.deltaTime;
			
		}
		GetrightMovement ();
		if(AllMembersDead())
		{
			currentWave ++;
			waveNum.text = "Wave: "+currentWave;
			EnhanceEnemy ();
			SpawnUntilFull ();
		}	
	}
	void EnhanceEnemy()
	{
		enhancedEnemy.fireRate += 0.015f;
		if((currentWave%2) == 0)
		{
			enhancedEnemy.enemyLaserSpeed += 0.25f;
		}
		
		if((currentWave%5)==0)
		{
			playerController.grantHealth();
		}
		if(currentWave >= 10 && currentWave<20)
		{
			enhancedEnemy.health =300f;
			currentEnemySprite = enhancedEnemy.mediumEnemy;
		}
		if(currentWave >= 20)
		{
			enhancedEnemy.health =400f;
			currentEnemySprite = enhancedEnemy.hardEnemy;
		}
		EnemyPrefab.GetComponent<SpriteRenderer>().sprite = currentEnemySprite;
		enhancedEnemy.scoreValue +=50;
	}
	
	
	bool AllMembersDead()
	{
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount>0)
			{
				return false;
			}
		}
		return true;
	}
	
	Transform NextFreePosition()
	{
		foreach(Transform childPositionGameObject in transform)
		{
			if(childPositionGameObject.childCount ==0)
			{
				return childPositionGameObject;
			}
		}
		return null;
	}
	
	void GetrightMovement()
	{
		float rightEdgeOfFormation = transform.position.x+(0.5f*width);
		float leftEdgeOfFormation = transform.position.x-(0.5f*width);
		
		if(leftEdgeOfFormation < xmin)
		{
			rightMovement=true;
		}
		else if(rightEdgeOfFormation>xmax)
		{
			rightMovement=false;
		}
	}
}
