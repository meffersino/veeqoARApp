using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;

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
                CurrentQR = result.Text;
                Debug.Log(LogTag + CurrentQR);
                arText.text = CurrentQR;
            }

            //Debug.Log(LogTag + "Attempted QR Detection");
        }
        else
        {
            timeSinceLastGPUTransfer += Time.deltaTime;
        }

        //Debug.Log(LogTag + "End QR Detection");
    }
}
