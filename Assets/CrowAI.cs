using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowAI : MonoBehaviour
{

    public float speed = 3;
    public Vector3 velocity = Vector3.forward;
    private Vector3 startVelocity;
    public Transform target;

    public Vector3 sumForce = Vector3.zero;
    public float m = 1;

    public float separationDistance = 3;
    public List<GameObject> seprationNeighbors = new List<GameObject>();
    public float separationWeight = 1;
    public Vector3 separationForce = Vector3.zero;//分离的力

    public float alignmentDistance = 6;
    public List<GameObject> alignmentNeighbors = new List<GameObject>();
    public float alignmentWeight = 1;
    public Vector3 alignmentForce = Vector3.zero;//队列的力
    public float cohesionWeight = 1;
    public Vector3 cohesionForce = Vector3.zero;

    public float checkInterval = 0.2f;

    public float animRandomTime = 2f;
    private Animation anim;
    private void Start()
    {
        target = GameObject.Find("Target").transform;
        startVelocity = velocity;
        InvokeRepeating("CalcForce", 0, checkInterval);
        anim = GetComponentInChildren<Animation>();
        Invoke("PlayAnim", Random.Range(0, animRandomTime));
    }
    void PlayAnim()
    {
        anim.Play();
    }

    void CalcForce()
    {
        //print("calcForce");
        sumForce = Vector3.zero;

        separationForce = Vector3.zero;//分离的力
        alignmentForce = Vector3.zero;//队列的力
        cohesionForce = Vector3.zero;

        seprationNeighbors.Clear();
        Collider[] colliders = Physics.OverlapSphere(transform.position, separationDistance);
        foreach (Collider c in colliders)
        {
            if (c != null && c.gameObject != this.gameObject)
            {
                seprationNeighbors.Add(c.gameObject);
            }
        }
        //计算分离的力
        foreach (GameObject neighbor in seprationNeighbors)
        {
            Vector3 dir = transform.position - neighbor.transform.position;
            separationForce += dir.normalized / dir.magnitude;
        }
        if (seprationNeighbors.Count > 0)
        {
            separationForce *= separationWeight;

            sumForce += separationForce;
        }


        //计算队列的力
        alignmentNeighbors.Clear();
        colliders = Physics.OverlapSphere(transform.position, alignmentDistance);
        foreach (Collider c in colliders)
        {
            if (c != null && c.gameObject != this.gameObject)
            {
                alignmentNeighbors.Add(c.gameObject);
            }
        }
        Vector3 avgDir = Vector3.zero;
        foreach (GameObject n in alignmentNeighbors)
        {
            avgDir += n.transform.forward;
        }
        if (alignmentNeighbors.Count > 0)
        {
            avgDir /= alignmentNeighbors.Count;
            alignmentForce = avgDir - transform.forward;
            alignmentForce *= alignmentWeight;
            sumForce += alignmentForce;
        }

        //聚集的力
        if (alignmentNeighbors.Count > 0)
        {
            Vector3 center = Vector3.zero;
            foreach (GameObject n in alignmentNeighbors)
            {
                center += n.transform.position;
            }
            center /= alignmentNeighbors.Count;
            Vector3 dirToCenter = center - transform.position;
            cohesionForce += dirToCenter.normalized * velocity.magnitude;
            cohesionForce *= cohesionWeight;
            sumForce += alignmentForce;
        }


        //保持恒定飞行速度的力
        Vector3 engineForce = velocity.normalized * startVelocity.magnitude;
        sumForce += engineForce * 0.1f;

        Vector3 targetDir = target.position - transform.position;
        sumForce += (targetDir.normalized - transform.forward) * speed;
    }


    // Update is called once per frame
    void Update()
    {

        Vector3 a = sumForce / m;

        velocity += a * Time.deltaTime;

        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(velocity), Time.deltaTime * 3);

        transform.Translate(transform.forward * Time.deltaTime * velocity.magnitude, Space.World);
    }
}
