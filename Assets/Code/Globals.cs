using UnityEngine;

public static class Globals
{
    private static Vector3 _playerPosition;
    public static Vector3 PlayerPosition { get { return _playerPosition; } set { _playerPosition = value; } }
    private static Vector3 _playerNextPostition;
    public static Vector3 PlayerNextPosition { get { return _playerNextPostition; } set { _playerNextPostition = value; } }
    public static Vector3 RandomPointOnCircle(Vector3 center, float radius)
    {
        float angle = Random.value * 360;

        Vector3 pos = Vector3.zero;

        pos.z = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        pos.x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        pos.y = center.y;

        return pos;
    }
}