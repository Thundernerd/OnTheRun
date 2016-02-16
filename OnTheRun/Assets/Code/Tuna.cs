using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[ExecuteInEditMode]
public class Tuna : MonoBehaviour {

    private int[,] grid = {
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
        {1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1},
        {1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1},
        {1,0,0,1,1,1,1,0,0,0,0,0,0,0,0,1},
        {1,0,0,1,1,0,0,0,0,0,0,0,0,0,0,1},
        {1,0,0,1,1,0,0,0,0,0,0,0,0,1,1,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
        {1,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1},
        {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
    };

    [HideInInspector]
    public GameObject Player;
    public float Speed = 2.5f;
    public bool DrawGrid = true;

    private Rect gInfo = new Rect();

    // Use this for initialization
    void Start() {
        if ( !Application.isPlaying ) return;
        Player = GameObject.Find( "Player" );

        var width = ( (float)Screen.width / (float)grid.GetLength( 1 ) );
        var height = ( (float)Screen.height / (float)grid.GetLength( 0 ) );
        var a = Camera.main.ScreenToWorldPoint( new Vector3( 0, 0 ) );
        var b = Camera.main.ScreenToWorldPoint( new Vector3( width, height ) );

        gInfo = new Rect( a.x, a.y, b.x - a.x, b.y - a.y );

        for ( int x = 0; x < 16; x++ ) {
            for ( int y = 0; y < 9; y++ ) {
                if ( grid[y, x] == 0 ) continue;
                var r = GetRect( x, 8 - y );
                var gobj = new GameObject();
                gobj.transform.position = r.center;
                var col = gobj.AddComponent<BoxCollider2D>();
                col.size = r.size;
            }
        }
    }

    void CalculateNew( float x, float y, ref float dist, ref Vector3 fpos ) {
        if ( grid[8 - (int)y, (int)x] == 1 ) return;

        var tpos = GetPosition( x, y );
        var tdist = ( tpos - Player.transform.position ).magnitude;
        if ( tdist > dist ) {
            dist = tdist;
            fpos = tpos;
        }
    }

    private Vector3 delta = new Vector3();
    private Vector3 newp = new Vector3();
    private bool found = false;

    // Update is called once per frame
    void Update() {
        if ( !Application.isPlaying ) return;

        if ( !found ) {
            found = true;
            var coords = GetCoordinates();

            var dist = 0f;
            var fpos = new Vector3();
            var tpos = new Vector3();

            CalculateNew( coords.x, coords.y - 1, ref dist, ref fpos );
            CalculateNew( coords.x, coords.y, ref dist, ref fpos );
            CalculateNew( coords.x - 1, coords.y, ref dist, ref fpos );
            CalculateNew( coords.x, coords.y, ref dist, ref fpos );
            CalculateNew( coords.x + 1, coords.y, ref dist, ref fpos );
            CalculateNew( coords.x, coords.y + 1, ref dist, ref fpos );

            var trpos = transform.position;
            trpos.x = (float)Math.Round( trpos.x, 1 );
            trpos.y = (float)Math.Round( trpos.y, 1 );
            fpos.x = (float)Math.Round( fpos.x, 1 );
            fpos.y = (float)Math.Round( fpos.y, 1 );

            delta = new Vector3();
            if ( trpos.x < fpos.x ) {
                delta.x = Speed;
            } else if ( trpos.x > fpos.x ) {
                delta.x = -Speed;
            }

            if ( trpos.y < fpos.y ) {
                delta.y = Speed;
            } else if ( trpos.y > fpos.y ) {
                delta.y = -Speed;
            }

            newp = fpos;
        }


        transform.position += delta * Time.deltaTime;

        var apos = transform.position;
        apos.x = (float)Math.Round( apos.x, 1 );
        apos.y = (float)Math.Round( apos.y, 1 );
        newp.x = (float)Math.Round( newp.x, 1 );
        newp.y = (float)Math.Round( newp.y, 1 );

        if ( apos == newp ) {
            found = false;
        }
    }

    private Vector3 GetCoordinates() {
        for ( int x = 0; x < 16; x++ ) {
            for ( int y = 0; y < 9; y++ ) {
                if ( grid[8 - y, x] == 1 ) continue;

                var rect = new Rect( gInfo.x + ( gInfo.width * x ), gInfo.y + ( gInfo.height * y ), gInfo.width, gInfo.height );
                if ( rect.Contains( transform.position ) ) {
                    return new Vector3( x, y );
                }
            }
        }

        return new Vector3( -1, -1 );
    }

    private Rect GetRect( int x, int y ) {
        return new Rect( gInfo.x + ( gInfo.width * x ), gInfo.y + ( gInfo.height * y ), gInfo.width, gInfo.height );
    }

    private Vector3 GetPosition( int x, int y ) {
        return new Vector3(
            gInfo.x + ( gInfo.width * x ) + ( gInfo.width / 2 ),
            gInfo.y + ( gInfo.height * y ) + ( gInfo.height / 2 ) );
    }

    private Vector3 GetPosition( float x, float y ) {
        return GetPosition( (int)x, (int)y );
    }

    public void OnGUI() {
        if ( !DrawGrid ) return;

        var width = ( (float)Screen.width / (float)grid.GetLength( 1 ) );
        var height = ( (float)Screen.height / (float)grid.GetLength( 0 ) );
        var xmax = Screen.width / width;
        var ymax = Screen.height / height;

        for ( int x = 0; x < xmax; x++ ) {
            for ( int y = 0; y < ymax; y++ ) {
                var val = grid[y, x];
                if ( val == 0 ) {
                    GUI.Box( new Rect( x * width, y * height, width, height ), "" );
                } else {
                    GUI.Box( new Rect( x * width, y * height, width, height ), "" );
                    GUI.Box( new Rect( x * width, y * height, width, height ), "" );
                    GUI.Box( new Rect( x * width, y * height, width, height ), "" );
                }
            }
        }
    }
}
