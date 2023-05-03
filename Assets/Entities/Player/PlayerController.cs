using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	float xmin;
	float xmax;
	
	public float speed=5f;
	public float padding= 1f;
	public GameObject laserBeam;
	public float laserSpeed;
	public float firingRate = 1f;
	public float health = 500f;
	public AudioClip explosion;
	public AudioClip shoot;
	private Text healthBar;
	
	private Animator hudAnim;
			
	void Start () 
	{
		healthBar= GameObject.Find ("HealthDisplay").GetComponent<Text>();
		healthBar.text = health.ToString ();
		hudAnim = GameObject.Find ("HUD").GetComponent<Animator>();
		float distance= transform.position.z-Camera.main.transform.position.z;
		Vector3 leftMost=Camera.main.ViewportToWorldPoint(new Vector3(0,0,distance));
		Vector3 rightMost=Camera.main.ViewportToWorldPoint(new Vector3(1,0,distance));
		xmin=leftMost.x + padding;
		xmax=rightMost.x - padding;
	}
	public void grantHealth()
	{
		health=health+300f;
		healthBar.text = health.ToString ();
	}
	void Fire()
	{
		GameObject newBeam = Object.Instantiate(laserBeam,this.transform.position,Quaternion.identity) as GameObject;
		newBeam.GetComponent<Rigidbody2D>().velocity = new Vector3(0,laserSpeed,0);
			
	}
	void Update () 
	{
		if(Input.GetKey(KeyCode.A))
		{
			gameObject.transform.position+=Vector3.left * speed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.D))
		{
			gameObject.transform.position+=Vector3.right * speed * Time.deltaTime;
		}
		//restricts the player to playSpace
		float newX = Mathf.Clamp (transform.position.x,xmin,xmax);
		transform.position=new Vector3(newX,transform.position.y,transform.position.z);
		
		if(Input.GetKeyDown (KeyCode.Space))
		{
			AudioSource.PlayClipAtPoint(shoot,transform.position);
			InvokeRepeating("Fire",0.0000001f,firingRate);
		}
		if(Input.GetKeyUp (KeyCode.Space))
		{
			CancelInvoke ();
		}
	}
	void OnTriggerEnter2D(Collider2D beam)
	{
		if(beam.gameObject.tag=="EnemyBeam")
		{
			print ("Hit by laser!");
			health=health-100;
			healthBar.text = health.ToString ();
			Destroy (beam.gameObject);
		}
		if(health<=0)
		{
			AudioSource.PlayClipAtPoint(explosion,transform.position);
			Destroy (gameObject);
			hudAnim.SetBool ("PlayerDead",true);		
		}
	}
	
}
