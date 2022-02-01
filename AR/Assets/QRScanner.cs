using UnityEngine;
using System;
using System.Collections;

using Vuforia;

using System.Threading;

using ZXing;
using ZXing.QrCode;
using ZXing.Common;


[AddComponentMenu("System/QRScanner")]
public class QRScanner : MonoBehaviour
{    
private bool cameraInitialized;

private BarcodeReader barCodeReader;
private bool isDecoding = false;

void Start()
{        
    barCodeReader = new BarcodeReader();
    StartCoroutine(InitializeCamera());
}

private IEnumerator InitializeCamera()
{
    // Waiting a little seem to avoid the Vuforia's crashes.
    yield return new WaitForSeconds(1.25f);

    // var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.RGB888, true);
    // Debug.Log(String.Format("FormatSet : {0}", isFrameFormatSet));

    // Force autofocus.
    var isAutoFocus = CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    if (!isAutoFocus)
    {
        CameraDevice.Instance.SetFocusMode(CameraDevice.FocusMode.FOCUS_MODE_NORMAL);
    }
    Debug.Log(String.Format("AutoFocus : {0}", isAutoFocus));
    cameraInitialized = true;
}

private void Update()
{
    if (cameraInitialized && !isDecoding)
    {
        try
        {
            var cameraFeed = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.RGB888);

            if (cameraFeed == null)
            {
                return;
            }
            ThreadPool.QueueUserWorkItem(new WaitCallback(DecodeQr), cameraFeed);

        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }

    Debug.Log("TESTSETESTESTESTESTS");
}

private void DecodeQr(object state){
    isDecoding = true;
    var cameraFeed = (Image)state;
    var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.RGB24);
    if (data != null)
        {
        // QRCode detected.
            isDecoding = false;
        }
    else
        {
            isDecoding = false;
            Debug.Log("No QR code detected !");
        }
}    
}