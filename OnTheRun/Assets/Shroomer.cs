using UnityEngine;
using System.Collections;

public class Shroomer : MonoBehaviour {

    private static GameObject inst;

    public static void Activate() {
        inst.GetComponent<Shroomer>().STartIt();
    }

    private IEnumerator ShroomItUp() {
        iTween.MoveTo( inst, new Vector3( 3.12f, -0.95f ), 1.5f );
        yield return new WaitForSeconds( 3.5f );
        iTween.MoveTo( inst, new Vector3( 3.12f, -11.95f ), 0.5f );
    }

    void Start() {
        inst = gameObject;
    }

    private void STartIt() {
        StartCoroutine( ShroomItUp() );
    }
}
