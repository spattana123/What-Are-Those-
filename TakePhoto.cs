using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Text;

public class TakePhoto : MonoBehaviour
{
    private const string BASE_URL = "www.google.com/getData.php?url=";

    private const string GOOGLE_API_KEY = "AIzaSyBSC2CjRcfgvo0dStXeOqsYAa1lsuPkjY8";

    private const string CLOUD_NAME = "dtludbb6q";

    private const string UPLOAD_PRESET_NAME = "t9fw3fqu";

    private const string IMAGE_SEARCH_URL = "https://www.google.com/searchbyimage?site=search&sa=X&image_url=";

    private const string GOOGLE_SEARCH_URL = "https://www.googleapis.com/customsearch/v1?key=" + GOOGLE_API_KEY + "&cref&q=";


    private string imageURl;

    private string wordsToSearch;





    byte[] imByteArr;
    byte[] imByteArr_resize;
    public GameObject buttonObject;
    //private GameObject scanningObject;
    private GameObject first_line_Object;
    private GameObject second_line_Object;
    // Use this for initialization
    void Start()
    {
        //first_line_Object = GameObject.Find("line1");
        //second_line_Object = GameObject.Find("line2");
        //this is the scan button
        buttonObject = GameObject.Find("Button");
        //scanningObject = GameObject.Find("Image");
        //first_line_Object= GameObject.Find("line1");
        //second_line_Object = GameObject.Find("line2");


        //scanningObject.SetActive(false);
       /** first_line_Object.SetActive(false);
        first_line_Object.transform.parent.gameObject.SetActive(false);
        second_line_Object.SetActive(false);
        second_line_Object.transform.parent.gameObject.SetActive(false); **/

       

    }
    public IEnumerator Take_Photo()
    {
        print("Here Take_Photo");
        string filePath;
        print("Here Take_Photo1");
        if (Application.isMobilePlatform)
        {
            print("Here pre-Application.isMobilePlatform");
            filePath = Application.persistentDataPath + "/image.png";
            Application.CaptureScreenshot("/image.png");
            yield return new WaitForSeconds(1.25f);
            imByteArr_resize = new byte[16384];
            imByteArr = File.ReadAllBytes(filePath);
            for (int i = 161519; i < 177903; i++)
            {
                imByteArr_resize[i - 161519] = imByteArr[i];
            }
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imByteArr_resize);
            print(imByteArr.Length);
            print("Here Application.isMobilePlatform");
        }
        else
        {
            //print("Here pre-Application.isMobilePlatform2");
            filePath = Application.dataPath + "/image.png";
            print(filePath);
            //print("stop?");
            Application.CaptureScreenshot(filePath);
            //print("stop??");
            yield return new WaitForSeconds(1.25f);
            print("stop??");
            imByteArr_resize = new byte[16384];
            imByteArr = File.ReadAllBytes(filePath);
            for(int i=161519; i <177903; i++)
            {
                imByteArr_resize[i - 161519] = imByteArr[i];
            }
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imByteArr_resize);
            print(imByteArr.Length);
            print("stop????");
            print("Here Application.isMobilePlatform2");
        }

        buttonObject.SetActive(false);
        //scanningObject.SetActive(true);
        StartCoroutine("Upload_To_Server");


    }
    public IEnumerator Upload_To_Server()
    {
        Dictionary<string, string> headers = new Dictionary<string, string>();
        //string json = JsonUtility.ToJson(imByteArr);
       // byte[] postThisz = Encoding.UTF8.GetBytes(json);
        /*for(int i=0; i < postThisz.Length; i++)
        {
            print(postThisz[i]);
        } */
        //byte[] response = wc.UploadData(siteUrl, postThisz);
        headers.Add("Content-Type","application/json; charset=utf8");
        foreach (KeyValuePair<string, string> entry in headers)
        {
            print(entry);
            // do something with entry.Value or entry.Key
        } 
       // string json_string = JsonUtility.ToJson(headers);
      /*  for (int i=0; i < imByteArr_resize.Length; i++){
            print(imByteArr_resize[i]);
        } */
        WWW request = new WWW("https://what-are-those-seanbrhn3.c9users.io/", imByteArr_resize);
        yield return request;
        string txt = "";
        if (string.IsNullOrEmpty(request.error)) { txt = request.text; }  //text of success
        else { txt = request.error; }


        StartCoroutine("Upload_Image");
    }
    public IEnumerator Upload_Image()
    {
        print("Here Upload_Image");
        string url = "https://api.cloudinary.com/v1_1/" + CLOUD_NAME + "/auto/upload/";

        WWWForm myForm = new WWWForm();
        myForm.AddBinaryData("file", imByteArr_resize);
        myForm.AddField("uploadPreset", UPLOAD_PRESET_NAME);

        WWW www = new WWW(url, myForm);
        yield return www;
        print(www.text);

        imageURl = www.text.Split('"', '"')[0];
        /*
        string[] urlarr = www.text.Split('"', '"');
        for(int i=0; i < urlarr.Length; i++)
        {
            print(urlarr[i]);
        }

        imageURl = urlarr[0];
        */

        print(imageURl);

        StartCoroutine("reverse_Image_Search");




    }

    public IEnumerator reverse_Image_Search()
    {
        print("Here Image_Search");
        print("imageURL is" + imageURl);
        string fullSearchURL = BASE_URL + WWW.EscapeURL(IMAGE_SEARCH_URL + imageURl);
        print(fullSearchURL);

        WWW www = new WWW(fullSearchURL);
        yield return www;

        wordsToSearch = www.text.Substring(www.text.IndexOf(">") + 1);

        print(wordsToSearch);

        StartCoroutine("Google_Search_API");
    }

    public IEnumerator Google_Search_API()
    {
        print("Here Google_Search_API");
        string searchURL = GOOGLE_SEARCH_URL + WWW.EscapeURL(wordsToSearch);

        WWW www = new WWW(searchURL);
        yield return www;


        var parsedData = www.text.Split('\n');

        print(www.text);

        if (parsedData.Length > 42)
        {
            print("Here in if");
            string line1 = parsedData[43];
            string line2 = parsedData[42];

            for (int i = 0; i < parsedData.Length; i++)
            {
                if (parsedData[i].Contains("Wikipedia"))
                {
                    line1 = parsedData[i];
                    line2 = parsedData[i + 4];
                    break;
                }
            }

            line1 = line1.Remove(0, 13);
            line2 = line2.Remove(0, 15);

            line1 = line1.Remove(line1.Length - 2);
            line2 = line2.Remove(line2.Length - 2);

            if (line2.Contains("\n"))
            {
                line2.Replace("\n", " ");
            }
            print(wordsToSearch);
            CreateVisibleText(wordsToSearch, line1, line2);
        }
        else {
            print("Here in else");
            /** {
                 string line1 = "ERROR";
                 string line2 = "ERROR";

                 CreateVisibleText(wordsToSearch, line1, line2);
             } **/
            //scanningObject.SetActive(false);
            buttonObject.SetActive(true);
    }
    }

    public void CreateVisibleText(string text1, string text2, string text3)
    {
        //print(text1);
        print("Here CreateVisibleText");
        first_line_Object.SetActive(true);
        second_line_Object.SetActive(true);
        first_line_Object.transform.parent.gameObject.SetActive(true);
        second_line_Object.transform.parent.gameObject.SetActive(true);

        if (text1.Contains(" "))
        {
            text1 = text1.Replace(" ", "\n");
        }
        first_line_Object.GetComponent<Text>().text = text1;

        if (text3.Contains("\\n"))
        {
            text3 = text3.Replace(@"\n", " ");
        }
        int spaceCounter = 0;
        for (int i = 0; i < text2.Length; i++)
        {
            if (text2[i] == ' ')
            {
                spaceCounter++;
                if (spaceCounter % 3 == 0)
                {
                    text2 = text2.Insert(i, "\n");
                }
            }
        }

        spaceCounter = 0;

        for (int i = 0; i < text3.Length; i++)
        {
            if (text3[i] == ' ')
            {
                spaceCounter++;
                if (spaceCounter % 4 == 0)
                {
                    text3 = text3.Insert(i, "/n");
                }
            }
        }

        second_line_Object.GetComponent<Text>().text = text3;
    }

    public void StartCamera()
    {

        first_line_Object = GameObject.Find("line1");
        second_line_Object = GameObject.Find("line2");
        first_line_Object.SetActive(false);
        first_line_Object.transform.parent.gameObject.SetActive(false);
        second_line_Object.SetActive(false);
        second_line_Object.transform.parent.gameObject.SetActive(false);

       /** first_line_Object.SetActive(false);
        second_line_Object.SetActive(false);
        first_line_Object.transform.parent.gameObject.SetActive(false);
        second_line_Object.transform.parent.gameObject.SetActive(false); **/
        StartCoroutine("Take_Photo");
    }

    // Update is called once per frame
    void Update()
    {

    }
}


