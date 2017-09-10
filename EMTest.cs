
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
using System.Linq;

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
        //string filepath2 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\testpicture3.jpg";
        string filepath3 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture637.jpg";
        string filepath4;
        string filepath5 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture97.jpg";
        string filepath6 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture2032.jpg";
        string filepath7 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture3431.jpg";
        string filepath_res = "";
        int size = 0;
        int times = 1;
        int i = 0;


        if (Application.isMobilePlatform)
        {
            filepath4 = Application.persistentDataPath + "/picture638.jpg";
            Application.CaptureScreenshot("/picture638.jpg");
        }
        else
        {
            filepath4 = Application.dataPath + "/picture638.jpg";
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
        string sqlQuery3 = "SELECT Count(*) FROM justimgTest";
        //dbcmd.Parameters.Add(new SqliteParameter("@img", imByteArr));
        //dbcmd.Parameters.Add(new SqliteParameter("@imglength", imByteArr.Length));

        dbcmd.CommandText = sqlQuery3;
        IDataReader reader3 = dbcmd.ExecuteReader();
        while (reader3.Read()) size = reader3.GetInt32(0);
        print("size is " + size);
        times = size / 350;
        reader3.Close();
        reader3 = null;


        dbcmd.CommandText = sqlQuery;
        print("here2");
        IDataReader reader = dbcmd.ExecuteReader();
        print("here3");
        
            //C: \Users\Sandeep\Documents\What_Are_Those\Assets
            // C:/Users/Sandeep/AppData/LocalLow/Soumya/What_Are_Those/picture36.jpg
        int score = 0;
        Image<Bgr, byte> picture = new Image<Bgr, byte>("C:\\Users\\Sandeep\\Pictures\\picture1.jpg");
        Image<Bgr, byte> picture2 = new Image<Bgr, Byte>(filepath2);
        Image<Gray, byte> picture3 = new Image<Gray, Byte>(filepath2);
        Image<Gray, byte> picture4 = new Image<Gray, byte>("C:\\Users\\Sandeep\\Pictures\\picture27.jpg");
        Image<Gray, byte> picture8 = new Image<Gray, byte>(filepath6);
        Image<Bgr, byte> picture85 = new Image<Bgr, byte>(filepath6);
        Image<Gray, byte> picture11 = new Image<Gray, byte>(filepath7);
        Image<Bgr, Byte> picture20 = null;
        int count = 0;
        string parta = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture";
        int num = 0;
        string partb = ".jpg";
        string imgurlprev = "";
        string imgurl = "";
        string imglinkprev = "";
        string imglink = "";
        string nameprev = "";
        string name = "1";
        string pgidprev = "";
        string pgid = "1";
        string pbidprev = "";
        string pbid = "1";
        int maxint = 0;

        /*
               // while (i < times) {
                    while (reader.Read() && count < 393)
                    {
                        //print("here4");
                        //int value = reader.GetInt32(0);
                        //string name = reader.GetString(1);
                        //int rand = reader.GetInt32(2);
                        imgurl = (string)reader.GetString(0);
                        if (imgurl.Contains("$NIKE_PWPx3$") && imgurl != imgurlprev)
                        {
                            imgurls.Add(imgurl); print(imgurl + " is added to the list "+(num+1));
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
                            if (nonZeroCount > max)
                            {
                                max = nonZeroCount;
                                //max = area;
                                maxint = imgurls.Count - 1;
                                print("max is " + max + " maxint is "+maxint + " picture name is "+ s );
                                //print("max is " + area);
                                picture5.Save(filepath3);
                                picture6.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture635.jpg");
                            }
                        }
                        count++;

                    }
               // } */
                reader.Close();
                reader = null;
                
        int counter = 0;
        byte[,,] data = picture85.Data;
        
        Image<Bgr, Byte> picture19 = new Image<Bgr, byte>(filepath2);
        byte[,,] data_two = picture19.Data;
        var red = data_two[0, 0, 2];
        var green = data_two[0, 0, 1];
        var blue = data_two[0, 0, 0];
        print("bgr is " + blue + " " + green + " " + red);
        // To resize the image 
        //Image<Bgr, byte> resizedImage = picture19.Resize(picture8.Width, picture8.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
        Boolean recfound = false;
        int mindiffarea = 36000;
        if (red < 246 && green < 246 && blue < 246) picture20 = ObjectDetector(picture19, filepath2); else picture20 = picture19;
        //Image<Gray, Byte> picture21 = new Image<Gray, byte>(picture20.Bitmap);
        for (int n=1; n < 120 && !recfound; n++)
        {
            for(int m=1; m<=3 && !recfound; m++)
            {
                for(int p=1; p<=m && !recfound; p++)
                {
                    int number = 100 * n + m * 10 + p;
                    int resnumber = number * 10;
                    //print("number is " + number);
                    if (number >= 131)
                    {
                        string s = "" + number;
                        string r = "" + resnumber;
                        filepath = parta + s + partb;
                        filepath_res = parta + r + partb;
                        try
                        {
                            Image<Gray, byte> picture6 = new Image<Gray, byte>(filepath);
                            Image<Bgr, byte> resizedImage = picture20.Resize(picture6.Width, picture6.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                            Image<Gray, Byte> picture21 = new Image<Gray, byte>(resizedImage.Bitmap);
                            //print("width is " + picture6.Width + " height is " + picture6.Height);
                            int picturearea = picture6.Width * picture6.Height * 2;
                            long matchTime;
                            //Image<Bgr, byte> picture5 = null;
                            /*Image<Bgr, Byte> picture5 = */ Draw(picture6, picture21, out matchTime);
                            //try { picture5 = Drawtwo(picture3, picture6); } catch(NullReferenceException e){ }
                            //print("nzc is now " + nonZeroCount + " picture name is " + s);

                            //picture5.Save(filepath_res);
                            //print("nzc is now " + r + " "+area);
                            int diffarea = Math.Abs(picturearea - area);
                            if(diffarea < mindiffarea)
                            {
                                //print("diff area is " + diffarea);
                                mindiffarea = diffarea;
                                maxint = n - 1;
                            }
/*
                            if (area > max)
                            {
                                max = area;
                                //max = area;

                                maxint = n - 1;

                                //print("max is " + max + " maxint is " + maxint + " picture name is " + s);
                                //print("max is " + area);
                                //picture5.Save(filepath3);
                                picture6.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture635.jpg");
                            }
                            if (Math.Abs(picturearea - area) <= 7000 && area != 0)
                            {
                                print("diff area is " + (picturearea - area));
                                max = area;
                                maxint = n - 1;
                                recfound = true;
                            } 
                            */
                        }
                        catch (FileNotFoundException e)
                        {

                        }
                    }

                }

            }
        }
        Image<Bgr, Byte> picture12 = new Image<Bgr, byte>(filepath2);
        //Image<Bgr, Byte> picture7 = ObjectDetector(picture12,filepath2);
        //Image<Bgr, Byte> picture7 = Whiteout(picture2);
        long matchTime2;
        long matchTime3;
        long matchTime4;
        //Image<Gray, Byte> picture14 = new Image<Gray, byte>(picture7.Bitmap);
        //Image<Gray, Byte> picture14 = new Image<Gray, byte>("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture636.jpg");
        //Image<Bgr, Byte> picture9 = Draw(picture8, picture14, out matchTime2);
        print("nzc2 of image1 is now " + nonZeroCount);
        print("area is " + area);
        //Image<Bgr, Byte> picture10 = Draw(picture11, picture14, out matchTime3);
        print("nzc2 of image2 is now " + nonZeroCount);
        print("area is " + area);
       // picture7.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture639.jpg");
        //picture9.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture641.jpg");
        //picture10.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture643.jpg");
     //   Image<Gray, Byte> gray7 = new Image<Gray, byte>(picture7.Bitmap); Image<Bgr, Byte> picture12 = Draw(gray7, picture8, out matchTime4); picture12.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture641.jpg");
        dbcmd.CommandText = sqlQuery2;
        IDataReader reader2 = dbcmd.ExecuteReader();
        count = 0;
        while (reader2.Read()  && count < 120)
        {
            //print("here5");
            imglink = (string)reader2.GetString(0);
            if (imglink.Contains("pgid") || imglink.Contains("pbid"))
            {
                if (imglink.Contains("pgid"))
                {
                    name = parsename(imglink);
                    pgid = parsepgid(imglink);

                    if (pgid != pgidprev)
                    {
                        imglinks.Add(name); //print(name + " is added to the list " + imglink + " " + count);
                        pgidprev = pgid;
                        //print(imglink);
                        count++;
                    }
                }

                else if(imglink.Contains("pbid"))
                {
                    name = parsepbidname(imglink);
                    pgid = parsepbid(imglink);

                    if (name != nameprev)
                    {
                        imglinks.Add(name); //print(name + " is added to the list " + imglink + " " + count);
                        nameprev = name;
                        //print(imglink);
                        count++;
                    }
                }
            }
        }

        buttonObject.SetActive(true);

        first_line_Object.SetActive(true);
        second_line_Object.SetActive(true);
        first_line_Object.transform.parent.gameObject.SetActive(true);
        second_line_Object.transform.parent.gameObject.SetActive(true);

        //print("num of imgurls "+imgurls.Count);
        print("num of imglinks "+imglinks.Count);

        print(maxint);
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
    public string parsepbidname(string link)
    {
        int prestart = link.IndexOf("product/");
        int start = prestart + 8;
        string acc = "";
        for (int i = start; link[i] != '/'; i++)
        {
            acc += link[i];
        }
        return acc;
    }
    public string parsepbid(string link)
    {
        int prestart = link.IndexOf("pbid=");
        int start = prestart + 5;
        string acc = "";
        for (int i = start; i < link.Length; i++)
        {
            acc += link[i];

        }
        return acc;
    }
    public string parsepgid(string link)
    {
        int prestart = link.IndexOf("pgid-");
        int start = prestart + 5;
        string acc = "";
        for(int i=start; i < link.Length; i++)
        {
            acc += link[i];
            
        }
        return acc;
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
    public Image<Bgr, Byte> ObjectDetector(Image<Bgr,Byte> modelImage, string filepath)
    {
        Stopwatch watch;
        HomographyMatrix homography = null;
        SURFDetector surfCPU = new SURFDetector(500, false);
        FastDetector fastCPU = new FastDetector(10, true);
        VectorOfKeyPoint modelKeyPoints;
        BriefDescriptorExtractor descriptor = new BriefDescriptorExtractor();
        Image<Gray, byte> grayImage = new Image<Gray, Byte>(filepath);
        modelKeyPoints = fastCPU.DetectKeyPointsRaw(grayImage, null);
        Matrix<byte> modelDescriptors = descriptor.ComputeDescriptorsRaw(grayImage, null, modelKeyPoints);

        Image<Bgr, Byte> result = Features2DToolbox.DrawKeypoints(grayImage, modelKeyPoints, new Bgr(0, 0, 255), Features2DToolbox.KeypointDrawType.DRAW_RICH_KEYPOINTS);
        result.Save("C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture645.jpg");
        //Image<Bgr, Byte> result = modelImage;

        MKeyPoint[] modelpoints = modelKeyPoints.ToArray();
        List<PointF> points = new List<PointF>();
        //List<PointF> boundarypointsList = new List<PointF>();
        Dictionary<float, float> boundaryPoints = new Dictionary<float, float>();
        Dictionary<float, float> boundaryPointshorizontal = new Dictionary<float, float>();
        Dictionary<float, float> boundaryPointsModified = new Dictionary<float, float>();
        Dictionary<float, float> boundaryPointsRed = new Dictionary<float, float>();
        for (int i=0; i < modelpoints.Length; i++)
        {
            points.Add(modelpoints[i].Point);
            //print("X is " + points.ToArray()[i].X + "Y is " + points.ToArray()[i].Y);
        }
        points.Sort((a, b) => a.X.CompareTo(b.X));
        float x = points.ToArray()[0].X;
        float y = points.ToArray()[0].Y;
        float nextx,nexty;
        float miny = grayImage.Height;
        float maxx = grayImage.Width;
        for (int i = 0; i < points.ToArray().Length-1; i++)
        {
            x = points.ToArray()[i].X;
            y = points.ToArray()[i].Y;
            nextx = points.ToArray()[i+1].X;
            nexty = points.ToArray()[i+1].Y;
            if (x == nextx)
            {
                miny = Mathf.Min(y, nexty);

            }
            else
            {
                boundaryPoints.Add(x, miny);

                //boundarypointsList.Add(new PointF(x, miny));
            }
            //print("X is " + points.ToArray()[i].X + " Y is " + points.ToArray()[i].Y);
            
        }
        int lastindex = points.ToArray().Length - 1;
        if(x != points.ToArray()[lastindex].X)
        {
            PointF lastpoint = points.ToArray()[lastindex];
            boundaryPoints.Add(lastpoint.X,lastpoint.Y);
        }
        points.Sort((a, b) => a.Y.CompareTo(b.Y));
        for (int i = 0; i < points.ToArray().Length - 1; i++)
        {
            x = points.ToArray()[i].X;
            y = points.ToArray()[i].Y;
            nextx = points.ToArray()[i + 1].X;
            nexty = points.ToArray()[i + 1].Y;
            if (y == nexty)
            {
                maxx = Mathf.Max(x, nextx);

            }
            else
            {
                boundaryPointshorizontal.Add(y, maxx);

                //boundarypointsList.Add(new PointF(x, miny));
            }
            //print("X is " + points.ToArray()[i].X + " Y is " + points.ToArray()[i].Y);

        }
        lastindex = points.ToArray().Length - 1;
        if (y != points.ToArray()[lastindex].Y)
        {
            PointF lastpoint = points.ToArray()[lastindex];
            boundaryPointshorizontal.Add(lastpoint.X, lastpoint.Y);
        }
        var min = boundaryPoints.ElementAt(0);
        var max = boundaryPoints.ElementAt(0);
        var hmax = boundaryPoints.ElementAt(0);
        for (int i=0; i < boundaryPoints.Count; i++)
        {
            var item = boundaryPoints.ElementAt(i);
            float itemKey = item.Key;
            float itemValue = item.Value;
            if (itemValue < min.Value) min = item;
            if (itemValue > max.Value || max.Value == result.Rows) max = item;
            //print("X is " + itemKey + " Y is " + itemValue);
        }
        for (int i = 0; i < boundaryPointshorizontal.Count; i++)
        {
            var item = boundaryPointshorizontal.ElementAt(i);
            float itemKey = item.Key;
            float itemValue = item.Value;
            if (itemValue < min.Value) min = item;
            if (itemValue > hmax.Value || hmax.Value == result.Cols) hmax = item;
            // print("horizontal Y is " + itemKey + " horizontal X is " + itemValue);
        }
        //print("MIN is " + min.Key + " " + min.Value);
        //print("MAX is " + max.Key + " " + max.Value);
        //print("HMAX is " + hmax.Key + " " + hmax.Value);

        float prev = boundaryPoints.ElementAt(0).Value;
        int mid = 0;
        for (int i = 0; i < boundaryPoints.ElementAt(0).Key; i++)
        {
            boundaryPointsModified[(float)i] = boundaryPoints.ElementAt(0).Value;
        }
        for (int i = 0; i < boundaryPoints.Count && boundaryPoints.ElementAt(i).Key != boundaryPointshorizontal.ElementAt(1).Value; i++)
        {
            var item = boundaryPoints.ElementAt(i);
            float itemKey = item.Key;
            float itemValue = item.Value;

            //print("itemKey "+itemKey+ " itemValue " + itemValue + " prev " + prev);

            if (itemValue > prev)
            {
                boundaryPointsModified[itemKey] = prev;
            }
            else if((prev - itemValue < 80 && prev != result.Rows)  || (prev == result.Rows && prev - itemValue > 0) )
            {
                boundaryPointsModified[itemKey] = itemValue;
                prev = itemValue;
            }
            else
            {
                boundaryPointsModified[itemKey] = prev;
            }
            mid = i;
        }
        for(int i = mid + 1; i < boundaryPoints.Count; i++)
        {
            var item = boundaryPoints.ElementAt(i);
            float itemKey = item.Key;
            float itemValue = item.Value;
            boundaryPointsModified[itemKey] = 0;
        }
        for (int i = 0; i < boundaryPointsModified.Count-1; i++)
        {
            var item = boundaryPointsModified.ElementAt(i);
            var itemKey = item.Key;
            var itemValue = item.Value;

            //print("X modified is " + itemKey + " Y modified is " + itemValue);
        }



        byte[,,] data = result.Data;
        byte[,,] data_model = modelImage.Data;

        int xstop = (int)boundaryPointsModified.ElementAt(0).Key;
        int ystop = (int)boundaryPointsModified.ElementAt(2).Value;



       


  /*     print("xstop is " + xstop + " ystop is "+ystop);
        for (int i = 0; i <= xstop; i++)
        {
            for (int j = 0; j <= ystop; j++)
            {
            data_model[j, i, 0] = 255;
            data_model[j, i, 1] = 255;
            data_model[j, i, 2] = 255;
            }
        }
        modelImage.Data = data_model; */



       for (int run = 19; run >= 0; run--)
        {
            for (int i = 0; i <= modelImage.Cols-1; i++)
            {
                for (int j = 0; j <= modelImage.Rows - 1; j++) {
                    
                    if(boundaryPoints.ContainsKey((float) i))
                    {
                        float stoppingPoint = boundaryPointsModified[(float)i];
                        //print("Stoppping Point is " + stoppingPoint);
                        if ((float) j <= stoppingPoint)
                        {
                            //print("j is "+j+" i is "+i+" red "+result[j, i].Red);
                            data_model[j, i, 0] = 246;
                            data_model[j, i, 1] = 246;
                            data_model[j, i, 2] = 246;
                        }
                        /*    else if (i == 600 || i == 612){
                                data[j, i, 0] = 255;
                                data[j, i, 1] = 0;
                                data[j, i, 2] = 0;
                            } */
                    }
                    else
                    {
                        float stoppingPoint = 0;
                        //print(" i is " + i);
                        if(i<boundaryPointsModified.Count) stoppingPoint = boundaryPointsModified.ElementAt(i).Value;
                        //print("Stoppping Point is " + stoppingPoint);

                        if ((float)j <= stoppingPoint)
                        {
                            //print("j is "+j+" i is "+i+" red "+result[j, i].Red);
                            data_model[j, i, 0] = 246;
                            data_model[j, i, 1] = 246;
                            data_model[j, i, 2] = 246;
                        }
                    }
                }
                
            }
            modelImage.Data = data_model;
        }

        //  for (int run = 19; run >= 0; run--)
        //  {

        if (min.Key < mid) mid = (int) min.Value;

        //print("mid is " + mid);
            for (int i = result.Cols - 1; i >= mid; i--)
            {
                for (int j = 0; j <= result.Rows - 1; j++)
                {

                    //      if (boundaryPointshorizontal.ContainsKey((float)i))
                    //      {
                    //float startingPoint = boundaryPointshorizontal[(float)i];
                    // print("Stoppping Point is " + stoppingPoint);
                    /*startingPoint <= j */

            /*            if (data[j, i, 2] < 180)
                        {
                            data[j, i, 0] = 255;
                            data[j, i, 1] = 255;
                            data[j, i, 2] = 255;

                        }
                        else
                        {
                        break;
                        } */

                        if (data[j, i, 2] >= 240)
                        {
                        boundaryPointsRed.Add(i, j);
                        //print("i is " + i + " j is " + j);
                        break;
                        }


       //             }
                }

            }
        //result.Data = data;
        //     }

        int maxredx = 0;
        int maxredy = 0;
        for (int run = 19; run >= 0; run--)
        {
            for (int i = result.Cols - 1; i >= mid; i--)
            {
                for (int j = 0; j <= result.Rows - 1; j++)
                {
                    if (boundaryPointsRed.ContainsKey(i))
                    {
                        if (i > maxredx) maxredx = i;
                        if (j > maxredy) maxredy = j;
                        float stoppingPoint = boundaryPointsRed[i];

                        if ((float)j <= stoppingPoint /* && i != 600 && i != 612 */  )
                        {
                            //print("j is "+j+" i is "+i+" red "+result[j, i].Red);
                            data_model[j, i, 0] = 246;
                            data_model[j, i, 1] = 246;
                            data_model[j, i, 2] = 246;
                        }
                    }
                }

            }
            modelImage.Data = data_model;
        }

        for (int run = 19; run >= 0; run--)
        {
            for (int i = maxredy; i >= 0; i--)
            {
                for (int j = result.Cols -1; j >= maxredx; j--)
                {
                    data_model[i, j, 0] = 246;
                    data_model[i, j, 1] = 246;
                    data_model[i, j, 2] = 246;
                }
            }
            modelImage.Data = data_model;
        }

  
        for (int run = 19; run >= 0; run--)
        {
            for (int i = result.Rows - 1; i >= max.Value; i--)
            {
                for (int j = 0; j <= result.Cols - 1; j++)
                {
                    data_model[i, j, 0] = 246;
                    data_model[i, j, 1] = 246;
                    data_model[i, j, 2] = 246;
                }
            }
            modelImage.Data = data_model;
        } 

       for (int run = 19; run >= 0; run--)
        {
            for (int i = result.Cols - 1; i >= hmax.Value; i--)
            {
                for (int j = 0; j <= result.Rows - 1; j++)
                {
                    data_model[j, i, 0] = 255;
                    data_model[j, i, 1] = 255;
                    data_model[j, i, 2] = 255;
                }
            }
            modelImage.Data = data_model;
        } 




        return modelImage;
    }
    public Image<Bgr, Byte> Whiteout(Image<Bgr, Byte> modelImage)
    {
        Image<Bgr, Byte> result = modelImage;

        byte[,,] data = result.Data;

        for (int run = 19; run >= 0; run--)
        {
            for (int i = result.Rows - 1; i >= 0; i--)
            {
                for (int j = result.Cols - 1; j >= 0; j--)
                {
                    if ((data[i, j, 0] >= 0 && data[i, j, 0] <= 20) && (data[i, j, 1] >= 0 && data[i, j, 1] <= 20) && (data[i, j, 2] >= 0 && data[i, j, 2] <= 20))
                    {
                        data[i, j, 0] = 255;
                        data[i, j, 1] = 255;
                        data[i, j, 2] = 255;

                    }
                    else
                    {
                        break;
                    }
                }
                for (int k = 0; k <= result.Cols - 1; k++)
                {
                    if ((data[i, k, 0] >= 0 && data[i, k, 0] <= 20) && (data[i, k, 1] >= 0 && data[i, k, 1] <= 20) && (data[i, k, 2] >= 0 && data[i, k, 2] <= 20))
                    {
                        data[i, k, 0] = 255;
                        data[i, k, 1] = 255;
                        data[i, k, 2] = 255;

                    }
                    else
                    {
                        break;
                    }
                }
            }

            result.Data = data;

        }
        return result;

    }
    public /*Image<Bgr, Byte>*/ void Draw(Image<Gray, Byte> modelImage, Image<Gray, byte> observedImage, out long matchTime)
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
        //Image<Bgr, Byte> result = Features2DToolbox.DrawMatches(modelImage, modelKeyPoints, observedImage, observedKeyPoints, indices, new Bgr(255, 255, 255), new Bgr(255, 255, 255), mask, Features2DToolbox.KeypointDrawType.DEFAULT);

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
             //for (int i = 0; i < pts.Length; i++) print("points are " + pts[i]);
            if (pts.Length > 0)
            {
                int a = (int)(pts[0].X * pts[1].Y + pts[1].X * pts[2].Y + pts[2].X * pts[3].Y + pts[3].X * pts[0].Y);
                int b = (int)(pts[1].X * pts[0].Y + pts[2].X * pts[1].Y + pts[3].X * pts[2].Y + pts[0].X * pts[3].Y);
                area = Math.Abs(a - b);
            }
            else
            {
                area = 0;
            }

        /**     int width = rect.Right - rect.Left;
             print("right is "+rect.Right +" left is "+rect.Left+" width is " + width);
             int height = rect.Top - rect.Bottom;
             print("top is " + rect.Top + " bottom is " + rect.Bottom + " height is " + height); **/


            try
            {
              //result.DrawPolyline(Array.ConvertAll<PointF, Point>(pts, Point.Round), true, new Bgr(System.Drawing.Color.Red), 5);
            }catch(OverflowException e)
            {

            }
        }
        else
        {
            area = 0;
        }
        #endregion
        
        matchTime = watch.ElapsedMilliseconds;

       // return result;
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
        //print("nonZeroCount is "+nonZeroCount);
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
            //area = Math.Abs((rect.Top - rect.Bottom) * (rect.Right - rect.Left)); 
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

  /*  void MatchingMethod()
    {
        //! [copy_source]
        /// Source image to display
        Mat img_display;
        img.copyTo(img_display);
        //! [copy_source]

        //! [create_result_matrix]
        /// Create the result matrix
        int result_cols = img.cols - templ.cols + 1;
        int result_rows = img.rows - templ.rows + 1;

        result.create(result_rows, result_cols, CV_32FC1);
        //! [create_result_matrix]

        //! [match_template]
        /// Do the Matching and Normalize
        bool method_accepts_mask = (CV_TM_SQDIFF == match_method || match_method == CV_TM_CCORR_NORMED);
        if (use_mask && method_accepts_mask)
        { matchTemplate(img, templ, result, match_method, mask); }
        else
        { matchTemplate(img, templ, result, match_method); }
        //! [match_template]

        //! [normalize]
        normalize(result, result, 0, 1, NORM_MINMAX, -1, Mat());
        //! [normalize]

        //! [best_match]
        /// Localizing the best match with minMaxLoc
        double minVal; double maxVal; Point minLoc; Point maxLoc;
        Point matchLoc;

        minMaxLoc(result, &minVal, &maxVal, &minLoc, &maxLoc, Mat());
        //! [best_match]

        //! [match_loc]
        /// For SQDIFF and SQDIFF_NORMED, the best matches are lower values. For all the other methods, the higher the better
        if (match_method == TM_SQDIFF || match_method == TM_SQDIFF_NORMED)
        { matchLoc = minLoc; }
        else
        { matchLoc = maxLoc; }
        //! [match_loc]

        //! [imshow]
        /// Show me what you got
        rectangle(img_display, matchLoc, Point(matchLoc.x + templ.cols, matchLoc.y + templ.rows), Scalar::all(0), 2, 8, 0);
        rectangle(result, matchLoc, Point(matchLoc.x + templ.cols, matchLoc.y + templ.rows), Scalar::all(0), 2, 8, 0);

        imshow(image_window, img_display);
        imshow(result_window, result);
        //! [imshow]

        return;
    } */

    // Update is called once per frame
    void Update () {
		
	}
}
