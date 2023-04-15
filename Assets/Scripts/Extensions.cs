using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    private static LayerMask layerMask = LayerMask.GetMask("Default");
    //private static LayerMask layerMask = LayerMask.GetMask("Default","Enemy"); // Figure out how to get Goombas to turn when colliding with each other
    public static bool Raycast(this Rigidbody2D rigidbody, Vector2 direction) {
        if (rigidbody.isKinematic) { // isKinematic means that the physics engine isn't controlling the object at the moment
            return false;
        }
        // We are going to make sure the game understands when the bottom of Mario is touching something
        // Radius is 0.25 as Mario's hitbox is 1x1, so the radius is 0.5 then we take just half of that to do the check
        float radius = 0.25f;
        float distance = 0.375f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask); // layerMask will make Mario essentially ignore whatever is not on the specified layer.
                                                                                          // In this case it is "Default," which most of our stuff is default, so it will ignore other stuff
        return hit.collider != null && hit.rigidbody != rigidbody; // If it is null, then we didn't hit anything and make sure the rigidbody is not the same as the one being Raycasted from.
    }

    /* public static bool Raycast2(this Rigidbody2D rigidbody, Vector2 direction) {
        if (rigidbody.isKinematic) { // isKinematic means that the physics engine isn't controlling the object at the moment
            return false;
        }
        // We are going to make sure the game understands when the bottom of Mario is touching something
        // Radius is 0.25 as Mario's hitbox is 1x1, so the radius is 0.5 then we take just half of that to do the check
        float radius = 0.25f;
        float distance = 0.5f;

        RaycastHit2D hit = Physics2D.CircleCast(rigidbody.position, radius, direction.normalized, distance, layerMask2); // layerMask will make Mario essentially ignore whatever is not on the specified layer.
                                                                                          // In this case it is "Default," which most of our stuff is default, so it will ignore other stuff
        return hit.collider != null && hit.rigidbody != rigidbody; // If it is null, then we didn't hit anything and make sure the rigidbody is not the same as the one being Raycasted from.
    } */

    /* Other is the object we collide with. Transform will be Mario in most cases. */
    public static bool DotProductTest(this Transform transform, Transform other, Vector2 testDirection) {
        Vector2 direction = other.position - transform.position; // Get vector direction from mario to the object
        // Normalize the vector, this keeps the direction of the vector but makes the length 1.
        return Vector2.Dot(direction.normalized, testDirection) > 0.25f; // If the Dot Product is above 0.25 then the vectors are mostly in the same direction
    }
}
