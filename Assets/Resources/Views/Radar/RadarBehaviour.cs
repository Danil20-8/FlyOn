using UnityEngine;
using System.Collections.Generic;
using Assets.Other;
using Assets.Models.Pool;

public class RadarBehaviour : MonoBehaviour {

    [SerializeField]
    Renderer shipView;
    PlayerDriver driver;
    SpecialObjectPool<GameObject<Renderer>> beacons;

    Material friendMaterial;
    Material enemyMaterial;

    void Awake()
    {
        enabled = false;

        friendMaterial = Instantiate(shipView.sharedMaterial);
        friendMaterial.color = Color.green;

        enemyMaterial = Instantiate(shipView.sharedMaterial);
        enemyMaterial.color = Color.red;

        beacons = new SpecialObjectPool<GameObject<Renderer>>(new GameObject<Renderer>(shipView, transform));
    }
	public void SetDriver(PlayerDriver driver)
    {
        this.driver = driver;
    }
	
	// Update is called once per frame
	public void Update () {
        beacons.ReleaseAll();
        foreach(var s in driver.enemiesAround)
        {
            var b = beacons.Get().obj;
            b.material = enemyMaterial;
            b.transform.localPosition = (s.transform.position - driver.shipPosition) / 100;
        }
        foreach(var s in driver.friendsAround)
        {
            var b = beacons.Get().obj;
            b.material = friendMaterial;
            b.transform.localPosition = (s.transform.position - driver.shipPosition) / 100;
        }
        transform.localRotation = Quaternion.Inverse(driver.moveDirection);
	}
}
