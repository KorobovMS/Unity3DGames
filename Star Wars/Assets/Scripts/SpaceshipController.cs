using UnityEngine;
using System.Collections;

public class SpaceshipController : MonoBehaviour {
	// Характеристики основного корабля
	public Transform spaceshipTransform;
	private float spaceshipVelocity = 20.0f;
	
	// Параметры для снаряда корабля
	public GameObject empireMissile;
	private float missileVelocity = 200.0f;
	private float missileLifeTime = 3.0f;
	
	// Параметры стрельбы для корабля
	private float rateOfFire = 0.25f;
	private float lastTimeOfShot = 0.0f;	
	
	// Позиция мыши
	Vector3 mousePosition;
	
	// Use this for initialization
	void Start() {
		mousePosition = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update() {		
		// Крен, тангаж и рысканье
		//float dx = Input.mousePosition.y - mousePosition.y;
		//float dz = Input.mousePosition.x - mousePosition.x;		
		float dx = ((Input.GetKey(KeyCode.W))?1:0) - ((Input.GetKey(KeyCode.S))?1:0);
		float dy = ((Input.GetKey(KeyCode.D))?1:0) - ((Input.GetKey(KeyCode.A))?1:0);
		float dz = ((Input.GetKey(KeyCode.Q))?1:0) - ((Input.GetKey(KeyCode.E))?1:0);
		
		spaceshipTransform.RotateAround(spaceshipTransform.position, spaceshipTransform.right, dx/2);
		spaceshipTransform.RotateAround(spaceshipTransform.position, spaceshipTransform.up, dy/2);
		spaceshipTransform.RotateAround(spaceshipTransform.position, spaceshipTransform.forward, dz/2);
		
		// Перемещение
		spaceshipTransform.position += spaceshipTransform.forward * spaceshipVelocity * Time.deltaTime; 
		
		// Стрельба
		if (Input.GetKey(KeyCode.Mouse0)) {
			if (Time.time > lastTimeOfShot + rateOfFire) { 
				// Два снаряда
				GameObject leftMissile;
				GameObject rightMissile;
				
				// Их положение относительно центра
				Vector3 relativeLeftMissilePosition = new Vector3(-0.1102656f, -0.08644582f, 0.6194916f);
				Vector3 relativeRightMissilePosition = new Vector3(0.09834232f, -0.08318619f, 0.6194916f);
				
				// Вращаем векторы, чтобы они соответствовали повороту корабля
				Vector3 relativeRotatedLeftMissilePosition = spaceshipTransform.rotation * relativeLeftMissilePosition;
				Vector3 relativeRotatedRightMissilePosition = spaceshipTransform.rotation * relativeRightMissilePosition;
				
				// Создаем копию префаба
				leftMissile = Instantiate(empireMissile, 
					spaceshipTransform.position + relativeRotatedLeftMissilePosition, 
					spaceshipTransform.rotation) as GameObject;
				rightMissile = Instantiate (empireMissile, 
					spaceshipTransform.position + relativeRotatedRightMissilePosition, 
					spaceshipTransform.rotation) as GameObject;
				
				// Подгоняем поворот снаряда
				leftMissile.transform.RotateAround(leftMissile.transform.position, spaceshipTransform.right, 90.0f);
				rightMissile.transform.RotateAround(rightMissile.transform.position, spaceshipTransform.right, 90.0f);
				
				// Устанавливаем скорость снарядов
				leftMissile.rigidbody.velocity = spaceshipTransform.forward * missileVelocity;
				rightMissile.rigidbody.velocity = spaceshipTransform.forward * missileVelocity;
				
				// Удаляем снаряды через несколько секунд
				Destroy(leftMissile, missileLifeTime);
				Destroy(rightMissile, missileLifeTime);
				
				// Сохраняем время последнего выстрела
				lastTimeOfShot = Time.time;
			}
		}
		
		// Для управления мышью
		//mousePosition = Input.mousePosition;
		
		// Выход
		if (Input.GetKey(KeyCode.Escape)) {
			Application.Quit();	
		}
	}
}
