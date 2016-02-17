using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    public static bool Invert = false;
    public static bool Slow = false;

    public GameObject CDown;
    public GameObject CUp;
    public GameObject CLeft;
    public GameObject CRight;
    public GameObject CIdle;

    public Text LaterText;

    public float Speed = 2f;

    enum EDirection {
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        None = 16
    }

    private EDirection currentDirection = EDirection.None;
    //private EDirection previousDirection = EDirection.None;

    private GameObject tuna;

    // Use this for initialization
    void Start() {
        Invert = false;
        Slow = false;
        tuna = GameObject.Find( "TUNA" );
    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.W ) || Input.GetKeyDown( KeyCode.UpArrow ) ) {
            ChangeDirection( EDirection.Up );
            SwitchCharacter();
        }

        if ( Input.GetKeyDown( KeyCode.S ) || Input.GetKeyDown( KeyCode.DownArrow ) ) {
            ChangeDirection( EDirection.Down );
            SwitchCharacter();
        }

        if ( Input.GetKeyDown( KeyCode.A ) || Input.GetKeyDown( KeyCode.LeftArrow ) ) {
            ChangeDirection( EDirection.Left );
            SwitchCharacter();
        }

        if ( Input.GetKeyDown( KeyCode.D ) || Input.GetKeyDown( KeyCode.RightArrow ) ) {
            ChangeDirection( EDirection.Right );
            SwitchCharacter();
        }

        if ( Input.GetKeyUp( KeyCode.W ) || Input.GetKeyUp( KeyCode.UpArrow ) ||
            Input.GetKeyUp( KeyCode.S ) || Input.GetKeyUp( KeyCode.DownArrow ) ||
            Input.GetKeyUp( KeyCode.A ) || Input.GetKeyUp( KeyCode.LeftArrow ) ||
            Input.GetKeyUp( KeyCode.D ) || Input.GetKeyUp( KeyCode.RightArrow ) ) {
            //currentDirection = previousDirection;
            //previousDirection = EDirection.None;

            if ( !Input.anyKey ) {
                currentDirection |= EDirection.None;

                //ChangeDirection( EDirection.None );
                SwitchCharacter();
            } else {
                if ( Input.GetKey( KeyCode.W ) || Input.GetKey( KeyCode.UpArrow ) ) {
                    ChangeDirection( EDirection.Up );
                    SwitchCharacter();
                }

                if ( Input.GetKey( KeyCode.S ) || Input.GetKey( KeyCode.DownArrow ) ) {
                    ChangeDirection( EDirection.Down );
                    SwitchCharacter();
                }

                if ( Input.GetKey( KeyCode.A ) || Input.GetKey( KeyCode.LeftArrow ) ) {
                    ChangeDirection( EDirection.Left );
                    SwitchCharacter();
                }

                if ( Input.GetKey( KeyCode.D ) || Input.GetKey( KeyCode.RightArrow ) ) {
                    ChangeDirection( EDirection.Right );
                    SwitchCharacter();
                }
            }
        }

        if ( Input.GetKeyUp( KeyCode.R ) ) {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene( scene.name );
        }

        var mov = new Vector3();

        switch ( currentDirection ) {
            case EDirection.Left:
                mov.x = -Speed;
                break;
            case EDirection.Right:
                mov.x = Speed;
                break;
            case EDirection.Up:
                mov.y = Speed;
                break;
            case EDirection.Down:
                mov.y = -Speed;
                break;
        }

        if ( ( currentDirection & EDirection.None ) == EDirection.None ) {
            mov.x = 0;
            mov.y = 0;
        }

        if ( Invert ) {
            var temp = mov.x;
            mov.x = -mov.y;
            mov.y = -temp;
        }

        if ( Slow ) {
            mov /= 5f;
        }

        transform.position += mov * Time.deltaTime;

        var tunadist = ( tuna.transform.position - transform.position ).magnitude;
        if ( tunadist < 0.35f ) {
            Tuna.Caught = true;
            StartCoroutine( ShowSomeTimeLater() );

        }
    }

    private IEnumerator ShowSomeTimeLater() {
        var fp = GameObject.Find( "_bpCameraFade" );
        var fr = fp.GetComponent<SpriteRenderer>();

        while ( fr.color.a < 1 ) {
            var c = fr.color;
            c.a += Time.deltaTime / 3f;
            fr.color = c;
            yield return new WaitForEndOfFrame();
        }

        while ( LaterText.color.a < 1 ) {
            var c = LaterText.color;
            c.a += Time.deltaTime /3;
            LaterText.color = c;

            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds( 3f );

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene( scene.name );

        yield break;
    }

    private void ChangeDirection( EDirection newDirection ) {
        //previousDirection = currentDirection;
        currentDirection = newDirection;
    }

    private void SwitchCharacter() {
        if ( ( currentDirection & EDirection.None ) == EDirection.None ) {
            CDown.SetActive( false );
            CUp.SetActive( false );
            CLeft.SetActive( false );
            CRight.SetActive( false );
            CIdle.SetActive( true );
        } else {
            CIdle.SetActive( false );
            CDown.SetActive( currentDirection == EDirection.Down );
            CUp.SetActive( currentDirection == EDirection.Up );
            CLeft.SetActive( currentDirection == EDirection.Left );
            CRight.SetActive( currentDirection == EDirection.Right );
        }
    }
}
