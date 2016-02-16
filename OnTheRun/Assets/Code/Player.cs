using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {

    public GameObject CDown;
    public GameObject CUp;
    public GameObject CLeft;
    public GameObject CRight;

    public float Speed = 5f;

    enum EDirection {
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
        None = 16
    }

    private EDirection currentDirection = EDirection.None;
    //private EDirection previousDirection = EDirection.None;

    // Use this for initialization
    void Start() {

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
                //SwitchCharacter();
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

        transform.position += mov * Time.deltaTime;
    }

    private void ChangeDirection( EDirection newDirection ) {
        //previousDirection = currentDirection;
        currentDirection = newDirection;
    }

    private void SwitchCharacter() {
        CDown.SetActive( currentDirection == EDirection.Down );
        CUp.SetActive( currentDirection == EDirection.Up );
        CLeft.SetActive( currentDirection == EDirection.Left );
        CRight.SetActive( currentDirection == EDirection.Right );
    }
}
