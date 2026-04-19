using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Credit : MonoBehaviour {

    public string twitterLink;

    private void OnMouseDown()
    {
        //open the URL supplied.
        Application.OpenURL(twitterLink);
    }
}
