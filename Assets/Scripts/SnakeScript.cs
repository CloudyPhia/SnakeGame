using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeScript : MonoBehaviour
{
    // Credit to Zigurous on Youtube [https://www.youtube.com/watch?v=U8gUnpeaMbQ&ab_channel=Zigurous] 
    // for the base tutorial 

    private Vector2 direction = Vector2.right; // snake can move in both x and y axis
    private List<Transform> segments; // List of Snake's Segments
    public Transform segmentPrefab; 
    public int initialSize = 4;

    private void Start(){
        segments = new List<Transform>();
        segments.Add(this.transform);

        ResetState();
    }

    // Update: called every frame your game is running (variable)
    private void Update() {
        // Input Directions
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
            direction = Vector2.up;
        } else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
            direction = Vector2.left;
        } else if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
            direction = Vector2.down;
        } else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
            direction = Vector2.right;
        }
    }

    // FixedUpdate: ran at a fixed time interval ~ important for physics related code
    private void FixedUpdate() {

        // Add segments ~> iterating in reverse order such that each segment follows the head
        for (int i = segments.Count - 1; i > 0; i--) {
            segments[i].position = segments[i - 1].position;
        }

        // Movement of Snake
        this.transform.position = new Vector3(
            Mathf.Round(this.transform.position.x) + direction.x,
            Mathf.Round(this.transform.position.y) + direction.y,
            0.0f
        );
    }

    private void ResetState() {
        for (int i = 1; i < segments.Count; i++) { // index 0 is snake head, thus start at index 1
            
                Destroy(segments[i].gameObject);
        }
        segments.Clear(); // ensure references are destroyed
        segments.Add(this.transform);

        for (int i = 1; i < this.initialSize; i++) {
            segments.Add(Instantiate(this.segmentPrefab));
        }

        this.transform.position = Vector3.zero;
    }

    private void Grow() {
        Transform segment = Instantiate(this.segmentPrefab);
        segment.position = segments[segments.Count - 1].position;

        segments.Add(segment); 
    }

    private void OnTriggerEnter2D(Collider2D otherCollider) {
        
        if (otherCollider.tag == "Food") {
            Grow();
        } else if (otherCollider.tag == "Obstacle") {
            ResetState();
        }
    } 
}
