using UnityEngine;

public static class _Vector2E {

    public static Vector3 ToVector3( this Vector2 v ) {
        return new Vector3( v.x, v.y );
    }

    public static Vector4 ToVector4( this Vector2 v ) {
        return new Vector4( v.x, v.y );
    }

    public static void Swap( this Vector2 v ) {
        float x = v.x;
        v.x = v.y;
        v.y = x;
    }
}