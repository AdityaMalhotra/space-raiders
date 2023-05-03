using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

	public GameObject enemyLaserBeam;
	public GameObject laser;
	public float health=200f;
	public float enemyLaserSpeed=1f;
	public float fireRate = 0.5f;
	public int scoreValue=50;
	public AudioClip explosion;
	public AudioClip fireSound;
	public Sprite easyEnemy;
	public Sprite mediumEnemy;
	public Sprite hardEnemy;
	
	private ScoreCalculator scoreCalc;
	
	void Start()
	{
		scoreCalc = GameObject.Find("ScoreCard").GetComponent<ScoreCalculator>();
	}
	void OnTriggerEnter2D(Collider2D projectile)
	{
		if(projectile.gameObject.tag == "Projectile")
		{
			float damageDealt = projectile.gameObject.GetComponent<Projectile>().GetDamage();
			health= health-damageDealt;
			Destroy (projectile.gameObject);
		}
		if(health<=0)
		{
			scoreCalc.Score (scoreValue);
			AudioSource.PlayClipAtPoint(explosion,transform.position);
			Destroy (gameObject);
		}
	}
	void Fire()
	{
		Vector3 offset = new Vector3(0, -1.0f, 0);
		Vector3 firePos = transform.position + offset;
		GameObject laser = Instantiate(enemyLaserBeam, firePos, Quaternion.identity) as GameObject;
		laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0,-enemyLaserSpeed);
	}
	void Update()
	{
		float probability = fireRate*Time.deltaTime;
		if(Random.value<probability)
		{
			AudioSource.PlayClipAtPoint(fireSound,transform.position);
			Fire ();
		}
	}
}
