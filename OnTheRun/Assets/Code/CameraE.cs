using UnityEngine;

public static class _CameraE {

    public static Vector2 BoundsMin( this Camera camera ) {
        if ( camera != null ) {
            return camera.transform.position.ToVector2() - camera.Extents();
        }

        return Vector2.zero;
    }

    public static Vector2 BoundsMax( this Camera camera ) {
        if ( camera != null ) {
            return camera.transform.position.ToVector2() + camera.Extents();
        }

        return Vector2.zero;
    }

    public static Bounds Bounds( this Camera camera ) {
        return new Bounds( camera.transform.position, camera.Size() );
    }

    public static Vector2 Extents( this Camera camera ) {
        if ( camera != null ) {
            if ( camera.orthographic ) {
#if UNITY_EDITOR
                return new Vector2( camera.orthographicSize * camera.aspect, camera.orthographicSize );
#else
                return new Vector2(camera.orthographicSize * Screen.width / Screen.height, camera.orthographicSize);
#endif
            } else {
                Debug.LogError( "Camera is not ortographic.", camera );
                return new Vector2();
            }
        }

        return Vector2.zero;
    }

    public static Vector2 Size( this Camera camera ) {
        return Extents( camera ) * 2;
    }
}