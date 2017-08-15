using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAI : MonoBehaviour {

    private Vector3 CurrentSpeed = Vector3.forward;

    private Vector3 sumForce = Vector3.zero;//合力
    public float m = 1;//质量

    private float severWeight = 1;
    private Vector3 severForce = Vector3.zero;//分离的力
    private Vector3 queueForce = Vector3.zero;//队列的力
    private Vector3 cohesionForce = Vector3.zero;//内聚的力

    private float CheckTime=0.2f;
    private float CheckRadius = 2f;

    private Animation animation;
    private List<GameObject> borderGroup = new List<GameObject>();
    // Use this for initialization
    void Start () {
        animation = transform.GetComponentInChildren<Animation>();
        InvokeRepeating("CaleForce",0, CheckTime); 
        StartCoroutine(StartAnimation());
    }
    
    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(Random. Range(0, 1));
        animation.Play();
    }
    void CaleForce()
    {
        sumForce = Vector3.zero;
        severForce = Vector3.zero;
        queueForce = Vector3.zero;
        cohesionForce = Vector3.zero;
        borderGroup.Clear();
        //以当前位置为中心，半径内有那些物体
        Collider[] colliders = Physics.OverlapSphere(transform.position, CheckRadius);
        foreach (var item in colliders)
        {
            if (item !=null &&item.gameObject!=this.gameObject)
            {
                borderGroup.Add(item.gameObject);
            }
        }
        CaleSeverForce();
    }
    /// <summary>
    /// 计算分离的力
    /// </summary>
    void CaleSeverForce()
    {
      
        foreach (var item in borderGroup)
        {
            Vector3 dir = transform.position - item.transform.position;
            severForce += dir.normalized / dir.magnitude;
        }
        if (borderGroup.Count>0)
        {
            severForce *= severWeight;
            sumForce += severForce;
        }
    }
    /// <summary>
    /// 计算队列的力
    /// </summary>
    void CaleQueueForce()
    {

    }
    /// <summary>
    /// 计算内聚的力
    /// </summary>
    void CaleCohesionForce()
    {

    }
    // Update is called once per frame
    void Update () {
        
        Vector3 a = sumForce / m;
        print(a);
        CurrentSpeed += a * Time.deltaTime;
        transform.Translate(CurrentSpeed* Time.deltaTime, Space.World);
	}
}
