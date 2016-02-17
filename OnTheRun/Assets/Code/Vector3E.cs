using UnityEngine;

public static class _Vector3E {

    public static Vector2 ToVector2( this Vector3 v ) {
        return new Vector2( v.x, v.y );
    }

    public static Vector4 ToVector4( this Vector3 v ) {
        return new Vector4( v.x, v.y, v.z );
    }
}