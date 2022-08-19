using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTail : MonoBehaviour
{
    public bool tail;
    public Transform bossTail;
    public Transform bossEnd;
    public float circleDiameter;

    private List<Transform> bossTails = new List<Transform>();
    private List<Vector2> positions = new List<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        positions.Add(bossTail.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            for(int i = 0; i < 7; i++)
            {
                AddTail(bossTail);
            }
            AddTail(bossEnd);
        }

        float distance = ((Vector2)bossTail.position - positions[0]).magnitude;

        if (distance > circleDiameter)
        {
            Vector2 direction = ((Vector2)bossTail.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * circleDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= circleDiameter;
        }

        for (int i = 0; i < bossTails.Count; i++)
        {
            bossTails[i].position = Vector2.Lerp(positions[i + 1], positions[i], distance / circleDiameter);
        }
    }

    

    public void AddTail(Transform tail)
    {
        Transform newTail = Instantiate(tail, positions[positions.Count - 1], bossTail.transform.rotation, transform);
        bossTails.Add(newTail);
        positions.Add(newTail.position);

    }
}
