using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MovingPlate : RaycastController {
    public Vector3 move;

    public override void Start() {
        base.Start();
    }

    void Update() {
        UpdateRaycastOrigins();
        Vector3 velocity = move * Time.deltaTime;

        MovePassengers(velocity);
        this.transform.Translate(velocity);
    }

    void MovePassengers(Vector3 velocity) {
        HashSet<Transform> movedPassengers = new HashSet<Transform>();

        float directionX = Mathf.Sign(velocity.x);
        float directionY = Mathf.Sign(velocity.y);

        //Vertically Moving
        if(velocity.y != 0) {
            float rayLength = Mathf.Abs(velocity.y) + skinWidth;

            for(int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = (directionY == -1) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft; //움직이는 방향에 따라 Ray를 바꿈 
                rayOrigin += Vector2.right * (verticalRaySpacing * i) ;
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, Layer.passengerLayer);

                if(hit) {
                    if(!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = (directionY == 1) ? velocity.x : 0; //승객이 위에 있을때만 x값을 줌
                        float pushY = velocity.y - (hit.distance - skinWidth) * directionY;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        } 

        //Horizontally Moving
        if(velocity.x) {
            float rayLength = Mathf.Abs(velocity.x) + skinWidth;

            for(int i = 0; i < horizontalRayCount; i++) {
                Vector2 rayOrigin = (directionX == -1) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight; //움직이는 방향에 따라 Ray를 바꿈 
                rayOrigin += Vector2.up * (horizontalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, Layer.passengerLayer);

                if(hit) {
                    if(!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x - (hit.distance - skinWidth) * directionX;
                        float pushY = 0;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }

        //Passengers on top if a horizontally or downward moving platform
        if(directionY == -1 || velocity.y == 0 && velocity.x != 0) {
            float rayLength = skinWidth * 2;

            for(int i = 0; i < verticalRayCount; i++) {
                Vector2 rayOrigin = raycastOrigins.bottomLeft + Vector2.right * (verticalRaySpacing * i);
                RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up, rayLength, Layer.passengerLayer);

                if(hit) {
                    if(!movedPassengers.Contains(hit.transform)) {
                        movedPassengers.Add(hit.transform);
                        float pushX = velocity.x
                        float pushY = velocity.y;

                        hit.transform.Translate(new Vector3(pushX, pushY));
                    }
                }
            }
        }
    }
}