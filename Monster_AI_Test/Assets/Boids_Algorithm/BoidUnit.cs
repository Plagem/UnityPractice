using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoidUnit : MonoBehaviour
{
    private float speed;
    private Vector3 targetVec;

    [SerializeField] private float NeighbourDistance = 2f;
    [SerializeField] private float SeparationDistance = 1f;

    private List<BoidUnit> neighbours;
    private List<BoidUnit> separations;

    [SerializeField] private LayerMask boidUnitLayerMask;

    private void Start()
    {
        neighbours = new List<BoidUnit>();
        separations = new List<BoidUnit>();
    }

    public void InitializeUnit(float _speed)
    {
        speed = _speed;
    }

    private void Update()
    {
        FindNeighbour();
        Vector3 cohesionVec = GetVectorCohesion();
        Vector3 separationVec = GetVectorSeparation();
        Vector3 alignmentVec = GetVectorAlignment();
        Vector3 mouseVec = GetMouseVector();

        targetVec = cohesionVec + separationVec * 3 + alignmentVec + mouseVec * 2;

        targetVec = Vector3.Lerp(this.transform.up, targetVec, Time.deltaTime);
        targetVec = targetVec.normalized;

        float angle = Mathf.Atan2(targetVec.y, targetVec.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        this.transform.position += targetVec * speed * Time.deltaTime;

    }

    public void FindNeighbour()
    {
        neighbours.Clear();

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, NeighbourDistance, boidUnitLayerMask);
        foreach(var coll in colls)
        {
            if(coll.transform.position != transform.position)
            {
                if(Vector3.Distance(transform.position, coll.transform.position) < SeparationDistance)
                {
                    separations.Add(coll.GetComponent<BoidUnit>());
                }
                neighbours.Add(coll.GetComponent<BoidUnit>());
            }
        }
    }

    // Neighbour의 중심 위치로 이동하는 Vector 생성
    private Vector3 GetVectorCohesion()
    {
        Vector3 cohesionVec = Vector3.zero;
        if(neighbours.Count <= 0)
        {
            return cohesionVec;
        }

        foreach(var neighbour in neighbours)
        {
            cohesionVec += neighbour.transform.position;
        }

        cohesionVec /= neighbours.Count;
        cohesionVec -= this.transform.position;
        cohesionVec.Normalize();

        return cohesionVec;
    }

    // Separation 안의 오브젝트에 대해 밀어내는 Vector 생성
    private Vector3 GetVectorSeparation()
    {
        Vector3 separationVec = Vector3.zero;
        if(separations.Count <= 0)
        {
            return separationVec;
        }

        foreach(var separation in separations)
        {
            separationVec += separation.transform.position;
        }

        separationVec /= separations.Count;
        separationVec = this.transform.position - separationVec;
        separationVec.Normalize();

        return separationVec;
    }

    // Neighbour의 평균 이동 방향으로 이동하는 Vector 생성
    private Vector3 GetVectorAlignment()
    {
        // 기본값 : transform.up
        Vector3 alignmentVec = transform.up;
        if(neighbours.Count <= 0)
        {
            return alignmentVec;
        }

        foreach(var neighbour in neighbours)
        {
            alignmentVec += neighbour.transform.up;
        }

        alignmentVec /= neighbours.Count;
        alignmentVec.Normalize();

        return alignmentVec;
    }

    private Vector3 GetMouseVector()
    {
        Vector3 mouseVector = Camera.main.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            -Camera.main.transform.position.z));

        mouseVector -= this.transform.position;
        mouseVector.Normalize();

        return mouseVector;
    }
}
