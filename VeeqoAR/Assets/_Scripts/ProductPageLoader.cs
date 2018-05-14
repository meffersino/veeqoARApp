using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProductPageLoader : MonoBehaviour {
    public string productURL = "";

    public void OnMouseDown()
    {
        string url = "https://app.veeqo.com/products/" + productURL;
        OpenProductPage(url);
    }

	public void OpenProductPage(string url)
    {
        Debug.Log("Opening URL: " + url);
        Application.OpenURL(url);
    }
}
