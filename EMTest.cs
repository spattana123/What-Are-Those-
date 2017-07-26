using UnityEngine;
using System.Collections;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.UI;
using Emgu.CV.CvEnum;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Features2D;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using Emgu.CV.GPU;
using Emgu.CV.Structure;
using Mono.Data.Sqlite;
using System.Runtime.InteropServices;
using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;

using UnityEngine;
using UnityEngine.UI;

public class EMTest : MonoBehaviour {

    public int max = 0;
    public int area = 1;
    public int nonZeroCount = 0;

    public GameObject buttonObject;
    private GameObject first_line_Object;
    private GameObject second_line_Object;
    // Use this for initialization
    void Start()
    {
        buttonObject = GameObject.Find("Button");
    }
    public void TakePhoto() {
        buttonObject.SetActive(false);
        byte[] imByteArr;
        ArrayList imgurls = new ArrayList();
        ArrayList imglinks = new ArrayList();
        string filepath = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture635.jpg";
        string filepath2 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture636.jpg";
        string filepath3 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture637.jpg";
        string filepath4;


        if (Application.isMobilePlatform)
        {
            filepath4 = Application.persistentDataPath + "/picture38.jpg";
            Application.CaptureScreenshot("/picture38.jpg");
        }
        else
        {
            filepath4 = Application.dataPath + "/picture38.jpg";
            Application.CaptureScreenshot(filepath4);
        }

        first_line_Object = GameObject.Find("line1");
        second_line_Object = GameObject.Find("line2");
        first_line_Object.SetActive(false);
        first_line_Object.transform.parent.gameObject.SetActive(false);
        second_line_Object.SetActive(false);
        second_line_Object.transform.parent.gameObject.SetActive(false);

        string conn = "URI=file:" + Application.dataPath + "/WhatAreThose.db";
        print("here1");
        IDbConnection dbconn;
        dbconn = (IDbConnection)new SqliteConnection(conn);
        dbconn.Open(); //Open connection to the database.
        IDbCommand dbcmd = dbconn.CreateCommand();
        string sqlQuery = "SELECT images " + "FROM justimgTest";
        string sqlQuery2 = "SELECT link " + "FROM justlnk";
        //dbcmd.Parameters.Add(new SqliteParameter("@img", imByteArr));
        //dbcmd.Parameters.Add(new SqliteParameter("@imglength", imByteArr.Length));
        dbcmd.CommandText = sqlQuery;
        print("here2");
        IDataReader reader = dbcmd.ExecuteReader();
        print("here3");
        
            //C: \Users\Sandeep\Documents\What_Are_Those\Assets
            // C:/Users/Sandeep/AppData/LocalLow/Soumya/What_Are_Those/picture36.jpg
        int score = 0;
        Image<Bgr, byte> picture = new Image<Bgr, byte>("C:\\Users\\Sandeep\\Pictures\\picture1.jpg");
        Image<Gray, byte> picture3 = new Image<Gray, Byte>(filepath2);
        Image<Gray, byte> picture4 = new Image<Gray, byte>("C:\\Users\\Sandeep\\Pictures\\picture27.jpg");
        int count = 0;
        string parta = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture";
        int num = 0;
        string partb = ".jpg";
        string imgurlprev = "";
        string imgurl = "";
        string imglinkprev = "";
        string imglink = "";
        int maxint = 0;
        while (reader.Read()  && count < 350)
        {
            print("here4");
            //int value = reader.GetInt32(0);
            //string name = reader.GetString(1);
            //int rand = reader.GetInt32(2);
            imgurl = (string)reader.GetString(0);
            if (imgurl.Contains("$NIKE_PWPx3$") && imgurl != imgurlprev)
            {
                imgurls.Add(imgurl); print(imgurl + " is added to the list");
                num++;
                string s = "" + num;
                filepath = parta + s + partb;
                ServicePointManager.ServerCertificateValidationCallback = MyRemoteCertificateValidationCallback;
                using (System.Net.WebClient client = new System.Net.WebClient())
                {
                    //client.DownloadFile(new Uri(imgurl), @filepath);
                    client.DownloadFile(new Uri(imgurl), @filepath);
                }
                imgurlprev = imgurl;
                print(imgurl);
                Image<Gray, byte> picture6 = new Image<Gray, byte>(filepath);
                long matchTime;
                Image<Bgr, Byte> picture5 = Draw(picture3, picture6, out matchTime);
                print("nzc is now " + nonZeroCount);
                //print("nzc is now " + area);
                if(nonZeroCount > max)
                {
                    max = nonZeroCount;
                    //max = area;
                    maxint = imgurls.Count-1;
                    print("max is " + max);
                    //print("max is " + area);
                    picture5.Save(filepath3);
                    picture6.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture635.jpg");
                }
            }
            count++;
          
        }
        reader.Close();
        reader = null;

        dbcmd.CommandText = sqlQuery2;
        IDataReader reader2 = dbcmd.ExecuteReader();
        count = 0;
        while (reader2.Read()  && count < 350)
        {
            print("here5");
            imglink = (string)reader2.GetString(0);
            if (imglink.Contains("pgid") && imglink != imglinkprev)
            {
                imglink = parsename(imglink);
                if (imglink != imglinkprev)
                {
                    imglinks.Add(imglink); print(imglink + " is added to the list");
                    imglinkprev = imglink;
                    print(imglink);
                    count++;
                }
            }
        }

        buttonObject.SetActive(true);

        first_line_Object.SetActive(true);
        second_line_Object.SetActive(true);
        first_line_Object.transform.parent.gameObject.SetActive(true);
        second_line_Object.transform.parent.gameObject.SetActive(true);

        print("num of imgurls "+imgurls.Count);
        print("num of imglinks "+imglinks.Count);

        print("shoe is" + imglinks[maxint]);
        second_line_Object.GetComponent<Text>().text = (string) imglinks[maxint];



        dbcmd.Dispose();
        dbcmd = null;
        dbconn.Close();
        dbconn = null;



        /**
        Image<Gray, byte> picture6 = new Image<Gray, byte>(filepath);
        long matchTime;
        Image<Bgr, byte> picture5 = Drawtwo(picture3, picture6);
        picture5.Save(filepath3);
        **/
    }
    public string parsename(string link)
    {
        int prestart = link.IndexOf("pd/");
        int start = prestart + 3;
        string acc = "";
        for (int i=start; link[i] != '/'; i++)
        {
            acc += link[i];
        }
        return acc;
    }
    public Image<Bgr, Byte> Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime)
    {
        Stopwatch watch;
        HomographyMatrix homography = null;

        SURFDetector surfCPU = new SURFDetector(500, false);
        VectorOfKeyPoint modelKeyPoints;
        VectorOfKeyPoint observedKeyPoints;
        Matrix<int> indices;

        Matrix<byte> mask;
        int k = 2;
        double uniquenessThreshold = 0.8;
        if (GpuInvoke.HasCuda)
        {
            GpuSURFDetector surfGPU = new GpuSURFDetector(surfCPU.SURFParams, 0.01f);
            using (GpuImage<Gray, Byte> gpuModelImage = new GpuImage<Gray, byte>(modelImage))
            //extract features from the object image
            using (GpuMat<float> gpuModelKeyPoints = surfGPU.DetectKeyPointsRaw(gpuModelImage, null))
            using (GpuMat<float> gpuModelDescriptors = surfGPU.ComputeDescriptorsRaw(gpuModelImage, null, gpuModelKeyPoints))
            using (GpuBruteForceMatcher<float> matcher = new GpuBruteForceMatcher<float>(DistanceType.L2))
            {
                modelKeyPoints = new VectorOfKeyPoint();
                surfGPU.DownloadKeypoints(gpuModelKeyPoints, modelKeyPoints);
                watch = Stopwatch.StartNew();

                // extract features from the observed image
                using (GpuImage<Gray, Byte> gpuObservedImage = new GpuImage<Gray, byte>(observedImage))
                using (GpuMat<float> gpuObservedKeyPoints = surfGPU.DetectKeyPointsRaw(gpuObservedImage, null))
                using (GpuMat<float> gpuObservedDescriptors = surfGPU.ComputeDescriptorsRaw(gpuObservedImage, null, gpuObservedKeyPoints))
                using (GpuMat<int> gpuMatchIndices = new GpuMat<int>(gpuObservedDescriptors.Size.Height, k, 1, true))
                using (GpuMat<float> gpuMatchDist = new GpuMat<float>(gpuObservedDescriptors.Size.Height, k, 1, true))
                using (GpuMat<Byte> gpuMask = new GpuMat<byte>(gpuMatchIndices.Size.Height, 1, 1))
                using (Emgu.CV.GPU.Stream stream = new Emgu.CV.GPU.Stream())
                {
                    matcher.KnnMatchSingle(gpuObservedDescriptors, gpuModelDescriptors, gpuMatchIndices, gpuMatchDist, k, null, stream);
                    indices = new Matrix<int>(gpuMatchIndices.Size);
                    mask = new Matrix<byte>(gpuMask.Size);

                    //gpu implementation of voteForUniquess
                    using (GpuMat<float> col0 = gpuMatchDist.Col(0))
                    using (GpuMat<float> col1 = gpuMatchDist.Col(1))
                    {
                        GpuInvoke.Multiply(col1, new MCvScalar(uniquenessThreshold), col1, stream);
                        GpuInvoke.Compare(col0, col1, gpuMask, CMP_TYPE.CV_CMP_LE, stream);
                    }

                    observedKeyPoints = new VectorOfKeyPoint();
                    surfGPU.DownloadKeypoints(gpuObservedKeyPoints, observedKeyPoints);

                    //wait for the stream to complete its tasks
                    //We can perform some other CPU intesive stuffs here while we are waiting for the stream to complete.
                    stream.WaitForCompletion();

                    gpuMask.Download(mask);
                    gpuMatchIndices.Download(indices);

                    if (GpuInvoke.CountNonZero(gpuMask) >= 4)
                    {
                        int nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                        if (nonZeroCount >= 4)
                            homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
                    }

                    watch.Stop();
                }
            }
        }
        else
        {
            //extract features from the object image
            modelKeyPoints = surfCPU.DetectKeyPointsRaw(modelImage, null);
            Matrix<float> modelDescriptors = surfCPU.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);

            watch = Stopwatch.StartNew();

            // extract features from the observed image
            observedKeyPoints = surfCPU.DetectKeyPointsRaw(observedImage, null);
            Matrix<float> observedDescriptors = surfCPU.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);
            BruteForceMatcher<float> matcher = new BruteForceMatcher<float>(DistanceType.L2);
            matcher.Add(modelDescriptors);

            indices = new Matrix<int>(observedDescriptors.Rows, k);
            using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
            {
                matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
                mask = new Matrix<byte>(dist.Rows, 1);
                mask.SetValue(255);
                Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
            }

            nonZeroCount = CvInvoke.cvCountNonZero(mask);
            if (nonZeroCount >= 4)
            {
                nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
                if (nonZeroCount >= 4)
                    homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(modelKeyPoints, observedKeyPoints, indices, mask, 2);
            }

            watch.Stop();
        }

        //Draw the matched keypoints
        Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
           indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);

        #region draw the projected region on the image
        if (homography != null)
        {  //draw a rectangle along the projected model
            Rectangle rect = modelImage.ROI;
            PointF[] pts = new PointF[] {
               new PointF(rect.Left, rect.Bottom),
               new PointF(rect.Right, rect.Bottom),
               new PointF(rect.Right, rect.Top),
               new PointF(rect.Left, rect.Top)};
            homography.ProjectPoints(pts);

           /** int width = rect.Right - rect.Left;
            int height = rect.Top - rect.Bottom;
            int score = 0;
            int area = width * height;
            score = Math.Abs(area);
            print("score is " + score); **/
            result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(System.Drawing.Color.Red), 5);
        }
        #endregion
        
        matchTime = watch.ElapsedMilliseconds;

        return result;
    }

    public Image<Bgr, Byte> Drawtwo(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage)
    {
        HomographyMatrix homography = null;

        FastDetector fastCPU = new FastDetector(10, true);
        VectorOfKeyPoint modelKeyPoints;
        VectorOfKeyPoint observedKeyPoints;
        Matrix<int> indices;

        BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();

        Matrix<byte> mask;
        int k = 2;
        double uniquenessThreshold = 0.8;

        //extract features from the object image
        modelKeyPoints = fastCPU.DetectKeyPointsRaw(modelImage, null);
        Matrix<Byte> modelDescriptors = descriptor.ComputeDescriptorsRaw(modelImage, null, modelKeyPoints);

        // extract features from the observed image
        observedKeyPoints = fastCPU.DetectKeyPointsRaw(observedImage, null);
        Matrix<Byte> observedDescriptors = descriptor.ComputeDescriptorsRaw(observedImage, null, observedKeyPoints);
        BruteForceMatcher<Byte> matcher = new BruteForceMatcher<Byte>(DistanceType.L2);
        matcher.Add(modelDescriptors);

        indices = new Matrix<int>(observedDescriptors.Rows, k);
        using (Matrix<float> dist = new Matrix<float>(observedDescriptors.Rows, k))
        {
            matcher.KnnMatch(observedDescriptors, indices, dist, k, null);
            mask = new Matrix<byte>(dist.Rows, 1);
            mask.SetValue(255);
            Features2DToolbox.VoteForUniqueness(dist, uniquenessThreshold, mask);
        }

        nonZeroCount = CvInvoke.cvCountNonZero(mask);
        print("nonZeroCount is "+nonZeroCount);
        if (nonZeroCount >= 4)
        {
            nonZeroCount = Features2DToolbox.VoteForSizeAndOrientation(modelKeyPoints, observedKeyPoints, indices, mask, 1.5, 20);
            if (nonZeroCount >= 4)
                homography = Features2DToolbox.GetHomographyMatrixFromMatchedFeatures(
                modelKeyPoints, observedKeyPoints, indices, mask, 2);
        }

        //Draw the matched keypoints
        Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints,
           indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);

        #region draw the projected region on the image
        if (homography != null)
        {  //draw a rectangle along the projected model
            Rectangle rect = modelImage.ROI;
            PointF[] pts = new PointF[] {
         new PointF(rect.Left, rect.Bottom),
         new PointF(rect.Right, rect.Bottom),
         new PointF(rect.Right, rect.Top),
         new PointF(rect.Left, rect.Top)};
            homography.ProjectPoints(pts);
            area = Math.Abs((rect.Top - rect.Bottom) * (rect.Right - rect.Left)); 
            result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(System.Drawing.Color.Red), 5);
        }
        #endregion

        return result;
    }
    public bool MyRemoteCertificateValidationCallback(System.Object sender,
    X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        bool isOk = true;
        // If there are errors in the certificate chain,
        // look at each error to determine the cause.
        if (sslPolicyErrors != SslPolicyErrors.None)
        {
            for (int i = 0; i < chain.ChainStatus.Length; i++)
            {
                if (chain.ChainStatus[i].Status == X509ChainStatusFlags.RevocationStatusUnknown)
                {
                    continue;
                }
                chain.ChainPolicy.RevocationFlag = X509RevocationFlag.EntireChain;
                chain.ChainPolicy.RevocationMode = X509RevocationMode.Online;
                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
                chain.ChainPolicy.VerificationFlags = X509VerificationFlags.AllFlags;
                bool chainIsValid = chain.Build((X509Certificate2)certificate);
                if (!chainIsValid)
                {
                    isOk = false;
                    break;
                }
            }
        }
        return isOk;
    }

    // Update is called once per frame
    void Update () {
		
	}
}
