using UnityEngine;
using System;

public static class Globals
{
    private static Vector3 _playerPosition;
    public static Vector3 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }
    private static Vector3 _playerNextPostition;
    public static Vector3 PlayerNextPosition { get { return _playerNextPostition; } set { _playerNextPostition = value; } }
    public static Vector3 RandomPointOnCircle(Vector3 center, float radius)
    {
        float angle = UnityEngine.Random.value * 360;

        Vector3 pos = Vector3.zero;

        pos.z = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        pos.y = center.y;

        return pos;
    }
    public const int EnvironmentLayer = 9;
    public const int EnemyLayer = 10;
    public const int PlayerLayer = 11;
    public static float FastInvSqrt(float x)
    {
        float xhalf = 0.5f * x;
        int i = BitConverter.ToInt32(BitConverter.GetBytes(x), 0);
        i = 0x5f3759df - (i >> 1);
        x = BitConverter.ToSingle(BitConverter.GetBytes(i), 0);
        x = x * (1.5f - xhalf * x * x);
        return Mathf.Pow(x, -1);
    }
    public static float FastSqrt(float x)
    {
        float newX = x - 1;
        return 1 + (newX / 2) - (Mathf.Pow(newX, 2) / 8) + (Mathf.Pow(newX, 3) / 16) - ((5 * Mathf.Pow(newX, 4)) / 128) + ((7 * Mathf.Pow(newX, 5)) / 256);
    }
}