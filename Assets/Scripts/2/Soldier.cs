using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : MonoBehaviour {
    private float viewDistance = 5;
    private float viewAngle = 120;

    private GameObject Player;
	// Use this for initialization
	void Start () {
        Player = GameObject.Find("Player");
	}
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(Vector3.Distance(Player.transform.position,this.transform.position))<5)
        {
            Vector3 playerDic = Player.transform.position - transform.position;
            float angle = Vector3.Angle(playerDic,transform.forward);
            if (angle<=viewAngle)
            {
                print("I see you");
            }
        }
	}
}
