using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductPageLoader : MonoBehaviour {
    
    public void OnMouseDown()
    {
        OpenProductPage("https://www.veeqo.com/");
    }

	public void OpenProductPage(string url)
    {
        Debug.Log("Opening URL: " + url);
        Application.OpenURL(url);
    }
}
