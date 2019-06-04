// Dependencies
using UnityEngine;
using System;

// Static Globals Information Datastore class. Variables are updated i.e. read from and written to
public static class Globals
{
    // References to player's position THIS frame
    private static Vector3 _playerPosition;
    public static Vector3 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }

    // References to what the player's position will be in the next frame
    private static Vector3 _playerNextPostition;
    public static Vector3 PlayerNextPosition { get { return _playerNextPostition; } set { _playerNextPostition = value; } }

    // Helper function to find a random point on a circle
    public static Vector3 RandomPointOnCircle(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.value * 360;

        Vector3 pos = Vector3.zero;

        pos.z = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        pos.y = center.y;

        return pos;
    }
    
    // Important constants
    public const int EnvironmentLayer = 9; // The layermask value for Environment objects (see Unity Editor Inspector Tab)
    public const int EnemyLayer = 10; // The layermask value for Enenmy objects (see Unity Editor Inspector Tab)
    public const int PlayerLayer = 11; // The layermask value for the Player (see Unity Editor Inspector Tab)
    public const int BulletLayer = 13; // The layermask value for Bullet instances (see Unity Editor Inspector Tab)

}