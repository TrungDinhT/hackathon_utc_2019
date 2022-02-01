using UnityEngine;
using System;
using System.Collections;

using Vuforia;

using System.Threading;

using ZXing;
using ZXing.QrCode;
using ZXing.Common;


[AddComponentMenu("System/VuforiaCamera")]
public class VuforiaCamera : MonoBehaviour
{

    //Movement arrow_mvt;

    private bool cameraInitialized;

    private BarcodeReader barCodeReader;
    private bool isDecoding = false;

    public bool test = false;
    public double alpha;

    void Start()
    {        
        //arrow_mvt = GetComponent<Movement>();
        barCodeReader = new BarcodeReader();
        StartCoroutine(InitializeCamera());
        //StartCoroutine(arrow_mvt.MoveArrow());
    }

    private IEnumerator InitializeCamera()
    {
        // Waiting a little seem to avoid the Vuforia's crashes.
        yield return new WaitForSeconds(1.25f);

        var isFrameFormatSet = CameraDevice.Instance.SetFrameFormat(Image.PIXEL_FORMAT.GRAYSCALE , true);
        Debug.Log(String.Format("FormatSet : {0}", isFrameFormatSet));

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
                var cameraFeed = CameraDevice.Instance.GetCameraImage(Image.PIXEL_FORMAT.GRAYSCALE);

                Debug.Log("AIEI");

                if (cameraFeed == null)
                {
                    Debug.Log("NANANAN");
                    return;
                }
                ThreadPool.QueueUserWorkItem(new WaitCallback(DecodeQr), cameraFeed);

            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    private void DecodeQr(object state){
        isDecoding = true;
        var cameraFeed = (Image)state;
        var data = barCodeReader.Decode(cameraFeed.Pixels, cameraFeed.BufferWidth, cameraFeed.BufferHeight, RGBLuminanceSource.BitmapFormat.RGB24);
        if (data != null)
            {
            // QRCode detected.
                isDecoding = false;
                //Debug.Log("QR Detected!!");
                Debug.Log(data.Text);

                double x_a = 3.0;
                double y_a = 0.0;
                string[] values = data.Text.Split(';');
                double x_b = Double.Parse(values[0].Split('=')[1]);
                double y_b = Double.Parse(values[1].Split('=')[1]);
                double beta = Math.Atan((y_a - 1.0 - y_b) / (x_a - x_b));
                alpha = Math.PI / 2.0 + beta;
                
                test = true;
                //arrow_mvt.transform.eulerAngles = new Vector3(90.0f, (float)alpha, 0.0f);
            }
        else
            {
                isDecoding = false;
                Debug.Log("No QR code detected !");
            }
    }

}