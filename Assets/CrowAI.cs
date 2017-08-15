using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAI : MonoBehaviour
{

    private Vector3 CurrentSpeed = Vector3.forward;

    private Vector3 sumForce = Vector3.zero;//合力
    public float m = 1;//质量

    private float severWeight = 1;
    private float severDistance = 2f;//分离监测半径
    private Vector3 severForce = Vector3.zero;//分离的力
    private List<GameObject> severGroup = new List<GameObject>();

    private float queueWeight = 1;
    private float queueDistance = 6;//队列监测半径
    private Vector3 queueForce = Vector3.zero;//队列的力
    public List<GameObject> queueGroup = new List<GameObject>();

    private float cohesionWeight = 1;
    private float cohesionDistance = 6;//内聚监测半径
    private Vector3 cohesionForce = Vector3.zero;//内聚的力
    private List<GameObject> cohesionGroup = new List<GameObject>();

    private float CheckTime = 0.2f;

    private Animation animation;

    // Use this for initialization
    void Start()
    {
        animation = transform.GetComponentInChildren<Animation>();
        InvokeRepeating("CaleForce", 0, CheckTime);
        StartCoroutine(StartAnimation());
    }

    IEnumerator StartAnimation()
    {
        yield return new WaitForSeconds(Random.Range(0, 1));
        animation.Play();
    }
    void CaleForce()
    {
        sumForce = Vector3.zero;
        severForce = Vector3.zero;
        queueForce = Vector3.zero;
        cohesionForce = Vector3.zero;
        //以当前位置为中心，半径内有那些物体
        CaleSeverForce();
        CaleQueueForce();
        CaleCohesionForce();
    }
    /// <summary>
    /// 计算分离的力
    /// </summary>
    void CaleSeverForce()
    {
        severGroup.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, severDistance);
        foreach (var item in colliders)
        {
            if (item != null && item.gameObject != this.gameObject)
            {
                severGroup.Add(item.gameObject);
            }
        }
        foreach (var item in severGroup)
        {
            Vector3 dir = transform.position - item.transform.position;
            severForce += dir.normalized / dir.magnitude;
        }
        if (severGroup.Count > 0)
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
        queueGroup.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, queueDistance);
        foreach (var item in colliders)
        {
            if (item != null && item.gameObject != this.gameObject)
            {
                queueGroup.Add(item.gameObject);
            }
        }
        Vector3 avgDir = Vector3.zero;
        foreach (var item in queueGroup)
        {
            avgDir += item.transform.forward;
        }
        if (queueGroup.Count > 0)
        {
            avgDir /= queueGroup.Count;
            queueForce = avgDir - transform.forward;
            queueForce = queueForce * queueWeight;
            sumForce += queueForce;
        }
    }
    /// <summary>
    /// 计算内聚的力
    /// </summary>
    void CaleCohesionForce()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, queueDistance);
        foreach (var item in colliders)
        {
            if (item != null && item.gameObject != this.gameObject)
            {
                cohesionGroup.Add(item.gameObject);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {

        Vector3 a = sumForce / m;
        print(a);
        CurrentSpeed += a * Time.deltaTime;
        transform.Translate(CurrentSpeed * Time.deltaTime, Space.World);
    }
}
