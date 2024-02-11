using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class BoidUnit : MonoBehaviour
{
    float speed;
    //Boids myBoids;
    Vector3 targetVec;

    List<BoidUnit> neighbours = new List<BoidUnit>();
    List<BoidUnit> desiredSeperation = new List<BoidUnit>();

    [SerializeField] LayerMask boidUnitLayer;

    public void InitializeUnit(float _speed)
    {
        //myBoids = _boids;
        speed = _speed;
    }

    private void Update()
    {
        FindNeighbour();
        Vector3 cohesionVec = CalculateCohesionVector();
        
        targetVec = cohesionVec;
        targetVec = Vector2.Lerp(this.transform.up, targetVec, Time.deltaTime);
        targetVec = targetVec.normalized;

        float angle = Mathf.Atan2(targetVec.y, targetVec.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        this.transform.position += targetVec * speed * Time.deltaTime;
    }

    private void FindNeighbour()
    {
        neighbours.Clear();

        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, 2f, boidUnitLayer);
        foreach(var coll in colls)
        {
            neighbours.Add(coll.GetComponent<BoidUnit>());
        }
        
        Debug.Log(neighbours.Count);
    }

    public Vector2 CalculateCohesionVector()
    {
        Vector2 cohesionVec = Vector2.zero;
        
        if(neighbours.Count > 0)
        {
            foreach (var boid in neighbours)
            {
                cohesionVec = cohesionVec + (Vector2)boid.transform.position;
            }
        }
        else
        {
            return Vector2.zero;
        }

        cohesionVec /= neighbours.Count;
        cohesionVec -= (Vector2)transform.position;
        cohesionVec.Normalize();
        return cohesionVec;
    }
}
