using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class WalkingAdventurer : MonoBehaviour
{
    [SerializeField] float speed = 1f;  // Speed of movement
    [SerializeField] float bounceRate = 0.5f;
    [SerializeField] float bounceAmp = 0.1f;

    Adventurer adventurer;   //adventerer data 
    List<Vector3> points;  // Array of points to follow

    int currentPointIndex = 0;
    bool isMoving = false;

    Vector3 position = Vector3.zero;

    public void StartPath(Adventurer adventurer, List<Vector3> points)
    {
        this.adventurer = adventurer;
        this.points = points;

        SetSprite();

        if (points.Count > 0)
        {
            isMoving = true;
            position = points[0] - 0.3f * Vector3.up;
        }
        else
        {
            Debug.LogWarning("No points set for RouteFollower.");
        }
    }

    void Update()
    {
        if (isMoving && points.Count > 0)
        {
            MoveAlongRoute();
            SetPosition();
        }
    }



    //move adventerer character from point to point
    private void MoveAlongRoute()
    {
        Vector3 targetPoint = points[currentPointIndex] - 0.3f*Vector3.up;
        position = Vector3.MoveTowards(position, targetPoint, speed * Time.deltaTime);

        if (Vector3.Distance(position, targetPoint) < 0.01f)
        {
            currentPointIndex++;
            if (currentPointIndex >= points.Count)
            {
                isMoving = false;
                EndRoute();
            }
        }
    }

    private void EndRoute()
    {
        PartyManager.adventurerArrived.Invoke(adventurer);
        Destroy(gameObject);
    }


    public void SetSprite()
    {
        SpriteRenderer sr = gameObject.GetComponent<SpriteRenderer>();
        sr.sprite = adventurer.info.bodySprite;
    }

    void SetPosition()
    {
        transform.position = position + Vector3.up*Mathf.Abs(bounceAmp * Mathf.Sin(Time.time*bounceRate));
    }
}
