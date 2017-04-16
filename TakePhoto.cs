using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class TakePhoto : MonoBehaviour
{
    private const string BASE_URL = "www.<YOUR_SITE>.com/getData.php?url=";

    private const string GOOGLE_API_KEY = "***************************";

    private const string CLOUD_NAME = "*********";

    private const string UPLOAD_PRESET_NAME = "****";

    private const string IMAGE_SEARCH_URL = "https://www.google.com/searchbyimage?site=search&sa=X&image_url=";

    private const string GOOGLE_SEARCH_URL = "https://www.googleapis.com/customsearch/v1?key=" + "GOOGLE_API_KEY" + "&cref&q=";


    private string imageURl;

    private string wordsToSearch;





    byte[] imByteArr;
    private GameObject buttonObject;
    private GameObject scanningObject;
    private GameObject first_line_Object;
    private GameObject second_line_Object;
    // Use this for initialization
    void Start()
    {
        buttonObject = GameObject.Find("Button");
        scanningObject = GameObject.Find("Image");
        first_line_Object = GameObject.Find("line1");
        second_line_Object = GameObject.Find("line2");

        scanningObject.SetActive(false);
        first_line_Object.SetActive(false);
        first_line_Object.transform.parent.gameObject.SetActive(false);
        second_line_Object.SetActive(false);
        second_line_Object.transform.parent.gameObject.SetActive(false);

    }
    public void StartCamera()
    {
        first_line_Object.SetActive(false);
        second_line_Object.SetActive(false);
        first_line_Object.transform.parent.gameObject.SetActive(false);
        second_line_Object.transform.parent.gameObject.SetActive(false);
        StartCoroutine("Take_Photo");
    }
    public IEnumerator Take_Photo()
    {
        string filePath;

        if (Application.isMobilePlatform)
        {
            filePath = Application.persistentDataPath + "/image.png";
            Application.CaptureScreenshot("/image.png");
            yield return new WaitForSeconds(1.25f);
            imByteArr = File.ReadAllBytes(filePath);
        }
        else
        {
            filePath = Application.dataPath + "/StreamingAssets/" + "image.png";
            Application.CaptureScreenshot(filePath);
            yield return new WaitForSeconds(1.25f);
            imByteArr = File.ReadAllBytes(filePath);
        }

        buttonObject.SetActive(false);
        scanningObject.SetActive(true);
        StartCoroutine("Upload_Image");


    }
    public IEnumerator Upload_Image()
    {
        string url = "https://api.cloudinary.com/v1_1/" + CLOUD_NAME + "/auto/upload/";

        WWWForm myForm = new WWWForm();
        myForm.AddBinaryData("file", imByteArr);
        myForm.AddField("uploadPreset", UPLOAD_PRESET_NAME);

        WWW www = new WWW(url, myForm);
        yield return www;
        print(www.text);

        imageURl = www.text.Split('"', '"')[41];

        StartCoroutine("reverse_Image_Search");




    }

    public IEnumerator reverse_Image_Search()
    {
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
        string searchURL = GOOGLE_SEARCH_URL + WWW.EscapeURL(wordsToSearch);

        WWW www = new WWW(searchURL);
        yield return www;


        var parsedData = www.text.Split('\n');

        print(www.text);

        if (parsedData.Length > 42)
        {
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

            CreateVisibleText(wordsToSearch, line1, line2);
        }
        else
            /** {
                 string line1 = "ERROR";
                 string line2 = "ERROR";

                 CreateVisibleText(wordsToSearch, line1, line2);
             } **/
            scanningObject.SetActive(false);
            buttonObject.SetActive(true);
    }

    public void CreateVisibleText(string text1, string text2, string text3)
    {
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


    // Update is called once per frame
    void Update()
    {

    }
}


