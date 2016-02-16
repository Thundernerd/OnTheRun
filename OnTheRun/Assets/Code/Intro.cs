using UnityEngine;
using System.Collections;

public class Intro : MonoBehaviour {

    public GameObject Bloemen;
    public GameObject IN;
    public GameObject SHOTSFIRED;

    public GameObject FACE;
    public GameObject LOGO;

    // Use this for initialization
    IEnumerator Start() {
        yield return new WaitForSeconds( 1 );

        iTween.MoveTo( Bloemen, iTween.Hash( "y", 3, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack ) );

        yield return new WaitForSeconds( 1 );

        iTween.MoveTo( IN, iTween.Hash( "x", 0, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack ) );

        yield return new WaitForSeconds( 1 );

        iTween.MoveTo( SHOTSFIRED, iTween.Hash( "y", -1, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack ) );
        iTween.MoveTo( FACE, iTween.Hash( "x", 5.25f, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack ) );
        //iTween.MoveTo(LOGO, iTween.Hash("x", -4.29f, "time", 0.5f, "easetype", iTween.EaseType.easeOutBack));
    }

    // Update is called once per frame
    void Update() {
        if ( Input.GetKeyDown( KeyCode.Space ) ) {
            Application.LoadLevel( 1 );
        }
    }
}

