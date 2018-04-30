using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using API;
using UnityEngine.UI;
using Newtonsoft.Json.Linq;

public class QRDetector : MonoBehaviour {
    private ARCamera aRcam;
    private ARTrackedObject arTrackedObj;
    private Camera c = null;
    private Texture2D tex;
    private string CurrentQR;
    private int width, height;
    private BarcodeReader barcodeReader;
    private RenderTexture rt;

    public TextMesh arText;

    public float GPUTransferRate = 1.0f;
    private float timeSinceLastGPUTransfer = 0.0f;

    private int W;
    private int H;

    //Logging
    private const string LogTag = "QRDetector: ";

    // Use this for initialization
    IEnumerator Start () {
        yield return new WaitForEndOfFrame();
        
        //Find ARCamera and Camera objects
        aRcam = FindObjectOfType<ARCamera>(); // (or FindObjectsOfType(typeof(ARCamera)) as ARCamera[])
        c = aRcam.gameObject.GetComponent<Camera>();
        
        arTrackedObj = FindObjectOfType<ARTrackedObject>();
        
        W = c.pixelWidth;
        H = c.pixelHeight;

        tex = new Texture2D(W, H, TextureFormat.RGB24, false);

        barcodeReader = new BarcodeReader { AutoRotate = false };

        //Debug.Log(LogTag + "QR Detection Initialised");
    }

    // Update is called once per frame
    void Update () {
        StartCoroutine(DecodeQR());
	}

    IEnumerator DecodeQR()
    {
        //Debug.Log(LogTag + "Start QR Detection");
        yield return new WaitForEndOfFrame();

        if (timeSinceLastGPUTransfer > 1 / GPUTransferRate)
        {
            timeSinceLastGPUTransfer = 0;

            tex.ReadPixels(new Rect(0, 0, W, H), 0, 0);
            tex.Apply();

            var result = barcodeReader.Decode(tex.GetPixels32(), W, H);
            if (result != null)
            {
                //Fetch API Key
                string API_Key = System.IO.File.ReadAllText(@"C:\Users\Thomas Fisher\Documents\GitHub\API Keys\API-KEY.txt");
                Debug.Log("API_KEY = " + API_Key);

                CurrentQR = result.Text;
                Debug.Log(LogTag + CurrentQR);

                //Product p = API_Calls.getProductByURL(CurrentQR);

                Debug.Log("Making API Request.");
                Dictionary<string, string> headers = new Dictionary<string, string>();
                headers.Add("Content-Type", "application/json");
                headers.Add("x-api-key", API_Key);
                ///GET by IIS hosting...
                WWW api = new WWW(CurrentQR, null, headers);
                StartCoroutine(WaitForWWW(api));

                //arText.text = .Title;
            }

            //Debug.Log(LogTag + "Attempted QR Detection");
        }
        else
        {
            timeSinceLastGPUTransfer += Time.deltaTime;
        }

        //Debug.Log(LogTag + "End QR Detection");
    }

    IEnumerator WaitForWWW(WWW www)
    {
        yield return www;

        string txt = "";
        if (string.IsNullOrEmpty(www.error))
            txt = www.text;  //text of success
        else
            txt = www.error;  //error

        JObject json = JObject.Parse(txt);
        Product p = API_Calls.parseToProduct(json);

        arText.text = p.Title + '\n';
        arText.text += "Qty: " + p.TotalStockLevel + '\n';
        arText.text += "Price: " + p.Price + '\n';
        arText.text += "Stock Check?: " + p.StockEntries + '\n';
        Debug.Log("API Response Received");
    }
}
