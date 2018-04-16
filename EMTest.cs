
using UnityEngine;
using System.Collections;
using Amazon.Lambda;
using Emgu.CV;
using Emgu.CV.Structure;
using System.Drawing;
using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using System.Text;
using Amazon.Lambda.Model;
using Amazon.S3.Model;
using Amazon.S3;

public class EMTest : MonoBehaviour {

    //public HOG hog;

    public int max = 0;
    public int area = 1;
    public int nonZeroCount = 0;
    public float[] vector;
    private const string CognitoIdentityPoolId = "";
    private string res = "";
    private byte[] imagebytes;
    private static LocationInfo currentGPSPosition;
    private static string gpsString = "";
    private static float radarRadius = 1000f;
    private static string radarSensor = "false";
    private static string radarType = "restaurant";
    private const string GOOGLE_PLACES_API_KEY = "AIzaSyDh1ZHNV5BysFPOx_1gqxzmhun5FgZrMYo";
    string GOOGLE_SEARCH_URL = "";
    private const string apilink = "https://a0x37qk1h6.execute-api.us-east-1.amazonaws.com/production/SearchThroughS3Bucket";
    public string[] jsonstring = {"https://store.nike.com/us/en_us/pd/air-zoom-mariah-flyknit-racer-mens-shoe/pid-11598426/pgid-11786883", "https://store.nike.com/us/en_us/pd/air-zoom-mariah-flyknit-racer-mens-shoe/pid-11598426/pgid-11786883", "https://store.nike.com/us/en_us/pd/air-zoom-mariah-flyknit-racer-mens-shoe/pid-11598422/pgid-11786883", "https://store.nike.com/us/en_us/pd/air-zoom-mariah-flyknit-racer-mens-shoe/pid-11598423/pgid-11786883", "https://store.nike.com/us/en_us/pd/air-zoom-mariah-flyknit-racer-mens-shoe/pid-11598424/pgid-11786883", "https://store.nike.com/us/en_us/pd/air-zoom-mariah-flyknit-racer-mens-shoe/pid-11598427/pgid-11786883", "https://store.nike.com/us/en_us/pd/duel-racer-mens-shoe/pid-11838187/pgid-11851253", "https://store.nike.com/us/en_us/pd/duel-racer-mens-shoe/pid-11838187/pgid-11851253", "https://store.nike.com/us/en_us/pd/duel-racer-mens-shoe/pid-11838185/pgid-11851253", "https://store.nike.com/us/en_us/pd/duel-racer-mens-shoe/pid-11838186/pgid-11851253", "https://store.nike.com/us/en_us/pd/air-jordan-5-retro-mens-shoe/pid-11594249/pgid-10264742", "https://store.nike.com/us/en_us/pd/air-jordan-5-retro-premium-mens-shoe/pid-11605077/pgid-11463731", "https://store.nike.com/us/en_us/pd/air-jordan-5-retro-premium-mens-shoe/pid-11605077/pgid-11463731", "https://store.nike.com/us/en_us/pd/air-jordan-5-retro-premium-mens-shoe/pid-11396746/pgid-11463731", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-10005903/pgid-11408180", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-10005903/pgid-11408180", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-11395673/pgid-11408180", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-11395669/pgid-11408180", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-11395670/pgid-11408180", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-11395671/pgid-11408180", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-10005902/pgid-11408180", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11591251/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11591251/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11792286/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11591247/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11591248/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11393831/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11393832/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11393836/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11792287/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11792285/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11393833/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11393834/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11380280/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11380281/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-11380282/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-10065820/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-huarache-mens-shoe/pid-10202679/pgid-10978234", "https://store.nike.com/us/en_us/pd/air-max-2017-mens-running-shoe/pid-11550056/pgid-11289589", "https://store.nike.com/us/en_us/pd/air-max-2017-mens-running-shoe/pid-11550056/pgid-11289589", "https://store.nike.com/us/en_us/pd/air-max-2017-mens-running-shoe/pid-11550054/pgid-11289589", "https://store.nike.com/us/en_us/pd/air-max-2017-mens-running-shoe/pid-11372555/pgid-11289589", "https://store.nike.com/us/en_us/pd/air-max-2017-mens-running-shoe/pid-11394139/pgid-11289589", "https://store.nike.com/us/en_us/pd/air-max-2017-mens-running-shoe/pid-11394140/pgid-11289589", "https://store.nike.com/us/en_us/pd/air-max-2017-mens-running-shoe/pid-11156954/pgid-11289589", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11385099/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11385099/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11591057/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11588892/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11588891/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11591055/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11591056/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11591058/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11591059/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11385100/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-rn-flyknit-2017-mens-running-shoe/pid-11385097/pgid-12011507", "https://store.nike.com/us/en_us/pd/free-train-virtue-hustle-hart-night-mens-training-shoe/pid-11863857/pgid-12192128", "https://store.nike.com/us/en_us/pd/free-train-virtue-hustle-hart-night-mens-training-shoe/pid-11863857/pgid-12192128", "https://store.nike.com/us/en_us/pd/free-train-virtue-hustle-hart-day-mens-training-shoe/pid-11863858/pgid-12192128", "https://store.nike.com/us/en_us/pd/jordan-trainer-2-flyknit-mens-training-shoe/pid-11644557/pgid-11832913", "https://store.nike.com/us/en_us/pd/jordan-trainer-2-flyknit-mens-training-shoe/pid-11644557/pgid-11832913", "https://store.nike.com/us/en_us/pd/jordan-trainer-2-flyknit-mens-training-shoe/pid-11674868/pgid-11832913", "https://store.nike.com/us/en_us/pd/jordan-trainer-2-flyknit-mens-training-shoe/pid-11673399/pgid-11832913", "https://store.nike.com/us/en_us/pd/metcon-3-mens-training-shoe/pid-11255885/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-mens-training-shoe/pid-11255885/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-mens-training-shoe/pid-11599437/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-mens-training-shoe/pid-11599439/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-mens-training-shoe/pid-11599436/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-rwb-mens-training-shoe/pid-11607176/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-freedom-mens-training-shoe/pid-11655116/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-mens-training-shoe/pid-11394260/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-3-mens-training-shoe/pid-11255882/pgid-12196588", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11599442/pgid-11994354", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11599442/pgid-11994354", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11599448/pgid-11994354", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11599443/pgid-11994354", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11599445/pgid-11994354", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11253562/pgid-11994354", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11255889/pgid-11994354", "https://store.nike.com/us/en_us/pd/metcon-dsx-flyknit-mens-training-shoe/pid-11394174/pgid-11994354", "https://store.nike.com/us/en_us/pd/zoom-fly-mens-running-shoe/pid-11792723/pgid-11588986", "https://store.nike.com/us/en_us/pd/zoom-fly-mens-running-shoe/pid-11792723/pgid-11588986", "https://store.nike.com/us/en_us/pd/zoom-fly-mens-running-shoe/pid-11792718/pgid-11588986", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11382129/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11382129/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11792690/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11591014/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-extra-wide-mens-running-shoe/pid-11394518/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11591012/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11591015/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11591016/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11600746/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11776442/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11395927/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11396008/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11396009/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11396010/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11596749/pgid-11619090", "https://store.nike.com/us/en_us/pd/air-zoom-pegasus-34-mens-running-shoe/pid-11382127/pgid-11619090", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11581604/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11581604/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11570587/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11570588/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11570590/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11570591/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11382233/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11385009/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11232562/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11232563/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11235196/pgid-12002801", "https://store.nike.com/us/en_us/pd/lunarepic-low-flyknit-2-mens-running-shoe/pid-11235198/pgid-12002801", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11398562/pgid-11619187", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11398562/pgid-11619187", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11398563/pgid-11619187", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11585149/pgid-11619187", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11591049/pgid-11619187", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11591051/pgid-11619187", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11398439/pgid-11619187", "https://store.nike.com/us/en_us/pd/free-rn-commuter-2017-mens-running-shoe/pid-11398440/pgid-11619187", "https://store.nike.com/us/en_us/pd/zoom-kdx-mens-basketball-shoe/pid-11597240/pgid-11817664", "https://store.nike.com/us/en_us/pd/zoom-kdx-mens-basketball-shoe/pid-11597240/pgid-11817664", "https://store.nike.com/us/en_us/pd/zoom-kdx-mens-basketball-shoe/pid-11597244/pgid-11817664", "https://store.nike.com/us/en_us/pd/lebron-soldier-xi-mens-basketball-shoe/pid-11599707/pgid-11817667", "https://store.nike.com/us/en_us/pd/lebron-soldier-xi-mens-basketball-shoe/pid-11599707/pgid-11817667", "https://store.nike.com/us/en_us/pd/lebron-soldier-xi-mens-basketball-shoe/pid-11859758/pgid-11817667", "https://store.nike.com/us/en_us/pd/lebron-soldier-xi-mens-basketball-shoe/pid-11809637/pgid-11817667", "https://store.nike.com/us/en_us/pd/kobe-ad-mens-basketball-shoe/pid-11812264/pgid-11455498", "https://store.nike.com/us/en_us/pd/kobe-ad-mens-basketball-shoe/pid-11812264/pgid-11455498", "https://store.nike.com/us/en_us/pd/kobe-ad-mens-basketball-shoe/pid-11812265/pgid-11455498", "https://store.nike.com/us/en_us/pd/kobe-ad-mens-basketball-shoe/pid-11391292/pgid-11455498", "https://store.nike.com/us/en_us/pd/kobe-ad-mens-basketball-shoe/pid-11391291/pgid-11455498", "https://store.nike.com/us/en_us/pd/kobe-ad-mens-basketball-shoe/pid-11807659/pgid-11455498", "https://store.nike.com/us/en_us/pd/kyrie-3-mens-basketball-shoe/pid-11891648/pgid-11459060", "https://store.nike.com/us/en_us/pd/kyrie-3-mens-basketball-shoe/pid-11891648/pgid-11459060", "https://store.nike.com/us/en_us/pd/kyrie-3-mens-basketball-shoe/pid-11809019/pgid-11459060", "https://store.nike.com/us/en_us/pd/kyrie-3-mens-basketball-shoe/pid-11297020/pgid-11459060", "https://store.nike.com/us/en_us/pd/jordan-trunner-lx-mens-shoe/pid-11400819/pgid-12125276", "https://store.nike.com/us/en_us/pd/jordan-trunner-lx-mens-shoe/pid-11400819/pgid-12125276", "https://store.nike.com/us/en_us/pd/jordan-trunner-lx-mens-shoe/pid-11601867/pgid-12125276", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11879684/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11879684/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11857089/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11973169/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11857088/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11776981/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11857090/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11857092/pgid-11985206", "https://store.nike.com/us/en_us/pd/jordan-fly-89-mens-shoe/pid-11857093/pgid-11985206", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-10298930/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-10298930/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-10000021/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-11393922/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-11046044/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-10471833/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-10348281/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-10192752/pgid-11406323", "https://store.nike.com/us/en_us/pd/roshe-one-mens-shoe/pid-10192757/pgid-11406323", "https://store.nike.com/us/en_us/pd/ck-racer-mens-shoe/pid-11597980/pgid-11657178", "https://store.nike.com/us/en_us/pd/ck-racer-mens-shoe/pid-11597980/pgid-11657178", "https://store.nike.com/us/en_us/pd/ck-racer-mens-shoe/pid-11597976/pgid-11657178", "https://store.nike.com/us/en_us/pd/ck-racer-mens-shoe/pid-11607816/pgid-11657178", "https://store.nike.com/us/en_us/pd/ck-racer-mens-shoe/pid-11607817/pgid-11657178", "https://store.nike.com/us/en_us/pd/ck-racer-mens-shoe/pid-11607818/pgid-11657178", "https://store.nike.com/us/en_us/pd/pg1-mens-basketball-shoe/pid-11971716/pgid-11555762", "https://store.nike.com/us/en_us/pd/pg1-mens-basketball-shoe/pid-11971716/pgid-11555762", "https://store.nike.com/us/en_us/pd/pg1-mens-basketball-shoe/pid-11391419/pgid-11555762", "https://store.nike.com/us/en_us/pd/kobe-ad-nxt-mens-basketball-shoe/pid-11482077/pgid-11562580", "https://store.nike.com/us/en_us/pd/kobe-ad-nxt-mens-basketball-shoe/pid-11482077/pgid-11562580", "https://store.nike.com/us/en_us/pd/kobe-ad-nxt-mens-basketball-shoe/pid-11391478/pgid-11562580", "https://store.nike.com/us/en_us/pd/lunarcharge-breathe-mens-shoe/pid-11782189/pgid-12002817", "https://store.nike.com/us/en_us/pd/lunarcharge-breathe-mens-shoe/pid-11782189/pgid-12002817", "https://store.nike.com/us/en_us/pd/lunarcharge-breathe-mens-shoe/pid-11782188/pgid-12002817", "https://store.nike.com/us/en_us/pd/lunarcharge-breathe-mens-shoe/pid-11782190/pgid-12002817", "https://store.nike.com/us/en_us/pd/cortez-basic-nylon-mens-shoe/pid-10872922/pgid-11821210", "https://store.nike.com/us/en_us/pd/cortez-basic-nylon-mens-shoe/pid-10872922/pgid-11821210", "https://store.nike.com/us/en_us/pd/cortez-basic-nylon-mens-shoe/pid-10872924/pgid-11821210", "https://store.nike.com/us/en_us/pd/converse-one-star-premium-suede-low-top-unisex-shoe/pid-11801819/pgid-12024235", "https://store.nike.com/us/en_us/pd/converse-one-star-premium-suede-low-top-unisex-shoe/pid-11801819/pgid-12024235", "https://store.nike.com/us/en_us/pd/converse-one-star-premium-suede-low-top-unisex-shoe/pid-11801817/pgid-12024235", "https://store.nike.com/us/en_us/pd/converse-one-star-premium-suede-low-top-unisex-shoe/pid-11907650/pgid-12024235", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11644892/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11644892/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11801776/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11907630/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11907631/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11644890/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11644891/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11653785/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-x-flyknit-high-top-unisex-shoe/pid-11801777/pgid-12043263", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214171/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214171/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214082/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214168/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214174/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214176/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214177/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214179/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214180/pgid-11593146", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214181/pgid-11593147", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214181/pgid-11593147", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214182/pgid-11593147", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11009298/pgid-11593147", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214083/pgid-11593147", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214170/pgid-11593147", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214178/pgid-11593147", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11467142/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11467142/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11631872/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11621858/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11599732/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11467139/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11467140/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-trainer-v7-mens-training-shoe/pid-11467143/pgid-11608832", "https://store.nike.com/us/en_us/pd/free-train-virtue-mens-training-shoe/pid-11394606/pgid-11608850", "https://store.nike.com/us/en_us/pd/free-train-virtue-mens-training-shoe/pid-11394606/pgid-11608850", "https://store.nike.com/us/en_us/pd/free-train-virtue-mens-training-shoe/pid-11601739/pgid-11608850", "https://store.nike.com/us/en_us/pd/free-train-virtue-mens-training-shoe/pid-11394607/pgid-11608850", "https://store.nike.com/us/en_us/pd/free-train-virtue-mens-training-shoe/pid-11394683/pgid-11608850", "https://store.nike.com/us/en_us/pd/free-train-virtue-mens-training-shoe/pid-11394684/pgid-11608850", "https://store.nike.com/us/en_us/pd/free-train-virtue-mens-training-shoe/pid-11597276/pgid-11608850", "https://store.nike.com/us/en_us/pd/jordan-trainer-essential-mens-training-shoe/pid-11786499/pgid-11634501", "https://store.nike.com/us/en_us/pd/jordan-trainer-essential-mens-training-shoe/pid-11786499/pgid-11634501", "https://store.nike.com/us/en_us/pd/jordan-trainer-essential-mens-training-shoe/pid-11601857/pgid-11634501", "https://store.nike.com/us/en_us/pd/jordan-trainer-essential-mens-training-shoe/pid-11601859/pgid-11634501", "https://store.nike.com/us/en_us/pd/jordan-trainer-essential-mens-training-shoe/pid-11601855/pgid-11634501", "https://store.nike.com/us/en_us/pd/metcon-repper-dsx-mens-training-shoe/pid-11591747/pgid-11496785", "https://store.nike.com/us/en_us/pd/metcon-repper-dsx-mens-training-shoe/pid-11591747/pgid-11496785", "https://store.nike.com/us/en_us/pd/metcon-repper-dsx-mens-training-shoe/pid-11591753/pgid-11496785", "https://store.nike.com/us/en_us/pd/metcon-repper-dsx-mens-training-shoe/pid-11591748/pgid-11496785", "https://store.nike.com/us/en_us/pd/metcon-repper-dsx-mens-training-shoe/pid-11591749/pgid-11496785", "https://store.nike.com/us/en_us/pd/air-max-zero-breathe-mens-shoe/pid-11394710/pgid-11619113", "https://store.nike.com/us/en_us/pd/air-max-zero-breathe-mens-shoe/pid-11394710/pgid-11619113", "https://store.nike.com/us/en_us/pd/air-max-zero-breathe-mens-shoe/pid-11394752/pgid-11619113", "https://store.nike.com/us/en_us/pd/air-max-zero-breathe-mens-shoe/pid-11490739/pgid-11619113", "https://store.nike.com/us/en_us/pd/sock-dart-unisex-shoe/pid-11591353/pgid-10973982", "https://store.nike.com/us/en_us/pd/sock-dart-unisex-shoe/pid-11591353/pgid-10973982", "https://store.nike.com/us/en_us/pd/sock-dart-unisex-shoe/pid-11394076/pgid-10973982", "https://store.nike.com/us/en_us/pd/sock-dart-unisex-shoe/pid-11394079/pgid-10973982", "https://store.nike.com/us/en_us/pd/sock-dart-unisex-shoe/pid-11394074/pgid-10973982", "https://store.nike.com/us/en_us/pd/sock-dart-unisex-shoe/pid-10872866/pgid-10973982", "https://store.nike.com/us/en_us/pd/sock-dart-unisex-shoe/pid-11052107/pgid-10973982", "https://store.nike.com/us/en_us/pd/sb-blazer-vapor-mens-skateboarding-shoe/pid-11395641/pgid-11608813", "https://store.nike.com/us/en_us/pd/sb-blazer-vapor-mens-skateboarding-shoe/pid-11395641/pgid-11608813", "https://store.nike.com/us/en_us/pd/sb-blazer-vapor-mens-skateboarding-shoe/pid-11395640/pgid-11608813", "https://store.nike.com/us/en_us/pd/sb-blazer-vapor-mens-skateboarding-shoe/pid-11395642/pgid-11608813", "https://store.nike.com/us/en_us/pd/zoom-kd-9-elite-mens-basketball-shoe/pid-11391433/pgid-12018101", "https://store.nike.com/us/en_us/pd/zoom-kd-9-elite-mens-basketball-shoe/pid-11391433/pgid-12018101", "https://store.nike.com/us/en_us/pd/zoom-kd-9-elite-mens-basketball-shoe/pid-11477650/pgid-12018101", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11591429/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11591429/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11242045/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11591426/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11591427/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11156924/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11394119/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11394124/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11394123/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11242040/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11242042/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11242043/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-presto-essential-mens-shoe/pid-11242046/pgid-11166280", "https://store.nike.com/us/en_us/pd/air-max-zero-essential-mens-shoe/pid-11591620/pgid-11296520", "https://store.nike.com/us/en_us/pd/air-max-zero-essential-mens-shoe/pid-11591620/pgid-11296520", "https://store.nike.com/us/en_us/pd/air-max-zero-essential-mens-shoe/pid-11591618/pgid-11296520", "https://store.nike.com/us/en_us/pd/air-max-zero-essential-mens-shoe/pid-11380374/pgid-11296520", "https://store.nike.com/us/en_us/pd/air-max-zero-essential-mens-shoe/pid-11380372/pgid-11296520", "https://store.nike.com/us/en_us/pd/air-max-ld-zero-unisex-shoe/pid-11394127/pgid-11619118", "https://store.nike.com/us/en_us/pd/air-max-ld-zero-unisex-shoe/pid-11394127/pgid-11619118", "https://store.nike.com/us/en_us/pd/air-max-ld-zero-unisex-shoe/pid-11394125/pgid-11619118", "https://store.nike.com/us/en_us/pd/air-max-ld-zero-unisex-shoe/pid-11394128/pgid-11619118", "https://store.nike.com/us/en_us/pd/air-max-ld-zero-unisex-shoe/pid-11394126/pgid-11619118", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-pride-core-high-top-unisex-shoe/pid-11801805/pgid-11910109", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-motion-mens-sandal/pid-11794701/pgid-11798305", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-motion-mens-sandal/pid-11794701/pgid-11798305", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-motion-mens-sandal/pid-11794703/pgid-11798305", "https://store.nike.com/us/en_us/pd/kawa-mens-slide/pid-10972402/pgid-11605275", "https://store.nike.com/us/en_us/pd/kawa-mens-slide/pid-10972402/pgid-11605275", "https://store.nike.com/us/en_us/pd/kawa-mens-slide/pid-11255658/pgid-11605275", "https://store.nike.com/us/en_us/pd/kawa-mens-slide/pid-11255657/pgid-11605275", "https://store.nike.com/us/en_us/pd/jordan-ultrafly-2-mens-basketball-shoe/pid-11396780/pgid-11605359", "https://store.nike.com/us/en_us/pd/jordan-ultrafly-2-mens-basketball-shoe/pid-11396780/pgid-11605359", "https://store.nike.com/us/en_us/pd/jordan-ultrafly-2-mens-basketball-shoe/pid-11601892/pgid-11605359", "https://store.nike.com/us/en_us/pd/jordan-ultrafly-2-mens-basketball-shoe/pid-11601893/pgid-11605359", "https://store.nike.com/us/en_us/pd/jordan-ultrafly-2-mens-basketball-shoe/pid-11396701/pgid-11605359", "https://store.nike.com/us/en_us/pd/jordan-ultrafly-2-mens-basketball-shoe/pid-11396702/pgid-11605359", "https://store.nike.com/us/en_us/pd/jordan-ultrafly-2-mens-basketball-shoe/pid-11396778/pgid-11605359", "https://store.nike.com/us/en_us/pd/jordan-cp3x-ae-mens-basketball-shoe/pid-11597145/pgid-11605369", "https://store.nike.com/us/en_us/pd/jordan-cp3x-ae-mens-basketball-shoe/pid-11597145/pgid-11605369", "https://store.nike.com/us/en_us/pd/jordan-cp3x-ae-mens-basketball-shoe/pid-11507070/pgid-11605369", "https://store.nike.com/us/en_us/pd/jordan-cp3x-ae-mens-basketball-shoe/pid-11597142/pgid-11605369", "https://store.nike.com/us/en_us/pd/kobe-mamba-instinct-mens-basketball-shoe/pid-11258426/pgid-11459052", "https://store.nike.com/us/en_us/pd/kobe-mamba-instinct-mens-basketball-shoe/pid-11258426/pgid-11459052", "https://store.nike.com/us/en_us/pd/kobe-mamba-instinct-mens-basketball-shoe/pid-11596064/pgid-11459052", "https://store.nike.com/us/en_us/pd/kobe-mamba-instinct-mens-basketball-shoe/pid-11385055/pgid-11459052", "https://store.nike.com/us/en_us/pd/kobe-mamba-instinct-mens-basketball-shoe/pid-11385056/pgid-11459052", "https://store.nike.com/us/en_us/pd/kobe-mamba-instinct-mens-basketball-shoe/pid-11385057/pgid-11459052", "https://store.nike.com/us/en_us/pd/kobe-mamba-instinct-mens-basketball-shoe/pid-11385008/pgid-11459052", "https://store.nike.com/us/en_us/pd/air-zoom-spiridon-mens-shoe/pid-11774737/pgid-11882992", "https://store.nike.com/us/en_us/pd/air-zoom-spiridon-mens-shoe/pid-11774737/pgid-11882992", "https://store.nike.com/us/en_us/pd/air-zoom-spiridon-mens-shoe/pid-11774736/pgid-11882992", "https://store.nike.com/us/en_us/pd/air-zoom-spiridon-mens-shoe/pid-11774738/pgid-11882992", "https://store.nike.com/us/en_us/pd/presto-fly-mens-shoe/pid-11809642/pgid-11491751", "https://store.nike.com/us/en_us/pd/presto-fly-mens-shoe/pid-11809642/pgid-11491751", "https://store.nike.com/us/en_us/pd/presto-fly-mens-shoe/pid-11809640/pgid-11491751", "https://store.nike.com/us/en_us/pd/presto-fly-mens-shoe/pid-11809641/pgid-11491751", "https://store.nike.com/us/en_us/pd/presto-fly-mens-shoe/pid-11521843/pgid-11491751", "https://store.nike.com/us/en_us/pd/presto-fly-mens-shoe/pid-11465127/pgid-11491751", "https://store.nike.com/us/en_us/pd/presto-fly-mens-shoe/pid-11465025/pgid-11491751", "https://store.nike.com/us/en_us/pd/lunarglide-8-mens-running-shoe/pid-11395887/pgid-11832062", "https://store.nike.com/us/en_us/pd/lunarglide-8-mens-running-shoe/pid-11395887/pgid-11832062", "https://store.nike.com/us/en_us/pd/lunarglide-8-mens-running-shoe/pid-11235134/pgid-11832062", "https://store.nike.com/us/en_us/pd/lunarglide-8-mens-running-shoe/pid-11235135/pgid-11832062", "https://store.nike.com/us/en_us/pd/lunarglide-8-mens-running-shoe/pid-11055854/pgid-11832062", "https://store.nike.com/us/en_us/pd/lunar-fingertrap-tr-mens-training-shoe/pid-11555934/pgid-11608822", "https://store.nike.com/us/en_us/pd/lunar-fingertrap-tr-mens-training-shoe/pid-11555934/pgid-11608822", "https://store.nike.com/us/en_us/pd/lunar-fingertrap-tr-mens-training-shoe/pid-11555936/pgid-11608822", "https://store.nike.com/us/en_us/pd/lunar-fingertrap-tr-mens-training-shoe/pid-11385128/pgid-11608822", "https://store.nike.com/us/en_us/pd/lunar-fingertrap-tr-mens-training-shoe/pid-11385161/pgid-11608822", "https://store.nike.com/us/en_us/pd/sfb-field-8-mens-boot/pid-10028427/pgid-10267225", "https://store.nike.com/us/en_us/pd/sfb-field-8-mens-boot/pid-10028427/pgid-10267225", "https://store.nike.com/us/en_us/pd/sfb-field-8-mens-boot/pid-10028430/pgid-10267225", "https://store.nike.com/us/en_us/pd/sfb-field-8-mens-boot/pid-10028429/pgid-10267225", "https://store.nike.com/us/en_us/pd/sfb-field-8-mens-boot/pid-10028428/pgid-10267225", "https://store.nike.com/us/en_us/pd/sfb-field-8-leather-mens-boot/pid-10213756/pgid-10247998", "https://store.nike.com/us/en_us/pd/sfb-field-8-leather-mens-boot/pid-10213756/pgid-10247998", "https://store.nike.com/us/en_us/pd/sfb-field-8-leather-mens-boot/pid-10346566/pgid-10247998", "https://store.nike.com/us/en_us/pd/sfb-field-6-mens-boot/pid-10028423/pgid-10349227", "https://store.nike.com/us/en_us/pd/sfb-field-6-mens-boot/pid-10028423/pgid-10349227", "https://store.nike.com/us/en_us/pd/sfb-field-6-mens-boot/pid-10028424/pgid-10349227", "https://store.nike.com/us/en_us/pd/sfb-field-6-mens-boot/pid-10028425/pgid-10349227", "https://store.nike.com/us/en_us/pd/jordan-trainer-prime-mens-training-shoe/pid-11605079/pgid-11634444", "https://store.nike.com/us/en_us/pd/jordan-trainer-prime-mens-training-shoe/pid-11605079/pgid-11634444", "https://store.nike.com/us/en_us/pd/jordan-trainer-prime-mens-training-shoe/pid-11400082/pgid-11634444", "https://store.nike.com/us/en_us/pd/jordan-trainer-prime-mens-training-shoe/pid-11400084/pgid-11634444", "https://store.nike.com/us/en_us/pd/jordan-trainer-prime-mens-training-shoe/pid-11292254/pgid-11634444", "https://store.nike.com/us/en_us/pd/air-force-1-ultraforce-mid-mens-shoe/pid-11394286/pgid-11464952", "https://store.nike.com/us/en_us/pd/air-force-1-ultraforce-mid-mens-shoe/pid-11394286/pgid-11464952", "https://store.nike.com/us/en_us/pd/air-force-1-ultraforce-mid-mens-shoe/pid-11394285/pgid-11464952", "https://store.nike.com/us/en_us/pd/air-force-1-ultraforce-mid-mens-shoe/pid-11242145/pgid-11464952", "https://store.nike.com/us/en_us/pd/air-force-1-ultraforce-mid-mens-shoe/pid-11242144/pgid-11464952", "https://store.nike.com/us/en_us/pd/air-force-1-ultraforce-mid-mens-shoe/pid-11253201/pgid-11464952", "https://store.nike.com/us/en_us/pd/air-force-1-ultraforce-mid-mens-shoe/pid-11254971/pgid-11464952", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11241043/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11241043/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11596028/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11596026/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11596029/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11391199/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11391335/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11241044/pgid-11302095", "https://store.nike.com/us/en_us/pd/air-max-sequent-2-mens-running-shoe/pid-11241045/pgid-11302095", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-11255668/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-11255668/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-11457929/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-11293981/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-11255669/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-11255670/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-11088032/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-10950845/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-10950852/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-10950846/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-10950847/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-10950849/pgid-11177870", "https://store.nike.com/us/en_us/pd/vapor-untouchable-pro-mens-football-cleat/pid-10950851/pgid-11177870", "https://store.nike.com/us/en_us/pd/force-zoom-trout-3-mens-baseball-cleat/pid-11395919/pgid-11456902", "https://store.nike.com/us/en_us/pd/force-zoom-trout-3-mens-baseball-cleat/pid-11395919/pgid-11456902", "https://store.nike.com/us/en_us/pd/force-zoom-trout-3-mens-baseball-cleat/pid-11395918/pgid-11456902", "https://store.nike.com/us/en_us/pd/force-zoom-trout-3-mens-baseball-cleat/pid-11157544/pgid-11456902", "https://store.nike.com/us/en_us/pd/force-zoom-trout-3-mens-baseball-cleat/pid-11157545/pgid-11456902", "https://store.nike.com/us/en_us/pd/force-zoom-trout-3-mens-baseball-cleat/pid-11157546/pgid-11456902", "https://store.nike.com/us/en_us/pd/force-zoom-trout-3-mens-baseball-cleat/pid-11157547/pgid-11456902", "https://store.nike.com/us/en_us/pd/air-force-1-high-07-mens-shoe/pid-10298923/pgid-11880185", "https://store.nike.com/us/en_us/pd/air-force-1-high-07-mens-shoe/pid-10298923/pgid-11880185", "https://store.nike.com/us/en_us/pd/air-force-1-high-07-mens-shoe/pid-11585044/pgid-11880185", "https://store.nike.com/us/en_us/pd/air-force-1-high-07-mens-shoe/pid-11585045/pgid-11880185", "https://store.nike.com/us/en_us/pd/air-force-1-high-07-mens-shoe/pid-10036069/pgid-11880185", "https://store.nike.com/us/en_us/pd/benassi-solarsoft-2-mens-slide/pid-10338145/pgid-11451878", "https://store.nike.com/us/en_us/pd/benassi-solarsoft-2-mens-slide/pid-10338145/pgid-11451878", "https://store.nike.com/us/en_us/pd/benassi-solarsoft-2-mens-slide/pid-10202435/pgid-11451878", "https://store.nike.com/us/en_us/pd/benassi-solarsoft-2-mens-slide/pid-10202437/pgid-11451878", "https://store.nike.com/us/en_us/pd/benassi-solarsoft-2-mens-slide/pid-10271642/pgid-11451878", "https://store.nike.com/us/en_us/pd/benassi-solarsoft-2-mens-slide/pid-10345008/pgid-11451878", "https://store.nike.com/us/en_us/pd/air-jordan-xiii-xiv-dmp-mens-basketball-shoe-pack/pid-11615711/pgid-12044453", "https://store.nike.com/us/en_us/product/roshe-one-ess-id/?piid=43563&pbid=154263469", "https://store.nike.com/us/en_us/product/roshe-one-ess-id/?piid=43563&pbid=154263469", "https://store.nike.com/us/en_us/product/roshe-one-ess-id/?piid=43563&pbid=356610350", "https://store.nike.com/us/en_us/product/roshe-one-ess-id/?piid=43563&pbid=710686039", "https://store.nike.com/us/en_us/product/roshe-one-ess-id/?piid=43563&pbid=313997640", "https://store.nike.com/us/en_us/product/roshe-one-ess-id/?piid=43563&pbid=471838918", "https://store.nike.com/us/en_us/product/roshe-one-ess-id/?piid=43563&pbid=667536051", "https://store.nike.com/us/en_us/pd/air-jordan-11-retro-low-mens-shoe/pid-11396567/pgid-11643343", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214172/pgid-11337711", "https://store.nike.com/us/en_us/pd/air-jordan-4-retro-mens-shoe/pid-11454915/pgid-10952699", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11215858/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11215858/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-womens-shoe/pid-12011820/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-womens-shoe/pid-12011823/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-unisex-shoe/pid-12011827/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-unisex-shoe/pid-12011831/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-unisex-shoe/pid-12011933/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-womens-shoe/pid-12011825/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-unisex-shoe/pid-12011829/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-unisex-shoe/pid-12011833/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-low-top-unisex-shoe/pid-12011931/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214002/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214012/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214014/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-shoe/pid-11214026/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214028/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214055/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214056/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214057/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214058/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11215857/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11215863/pgid-11593193", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011928/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011928/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011802/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011806/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-womens-shoe/pid-12011799/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011808/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011810/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011812/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-unisex-shoe/pid-12011926/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-seasonal-high-top-womens-shoe/pid-12011804/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214001/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214011/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214013/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11214025/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11215854/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-high-top-unisex-shoe/pid-11215855/pgid-11593190", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-monochrome-low-top-unisex-shoe/pid-11214092/pgid-11232805", "https://store.nike.com/us/en_us/pd/air-force-1-mid-07-mens-shoe/pid-10033322/pgid-11417771", "https://store.nike.com/us/en_us/pd/air-force-1-mid-07-mens-shoe/pid-10033322/pgid-11417771", "https://store.nike.com/us/en_us/pd/air-force-1-mid-07-mens-shoe/pid-11585048/pgid-11417771", "https://store.nike.com/us/en_us/pd/air-force-1-mid-07-mens-shoe/pid-11585049/pgid-11417771", "https://store.nike.com/us/en_us/pd/air-force-1-mid-07-mens-shoe/pid-10365586/pgid-11417771", "https://store.nike.com/us/en_us/pd/air-force-1-mid-07-mens-shoe/pid-10033323/pgid-11417771", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11255534/pgid-12185389", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11255534/pgid-12185389", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11605973/pgid-12185389", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11605974/pgid-12185389", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11399964/pgid-12185389", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11399895/pgid-12185389", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11255535/pgid-12185389", "https://store.nike.com/us/en_us/pd/jordan-eclipse-mens-shoe/pid-11155059/pgid-12185389", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-monochrome-high-top-unisex-shoe/pid-11214091/pgid-11232758", "https://store.nike.com/us/en_us/pd/air-jordan-13-retro-low-mens-shoe/pid-11396344/pgid-11643346", "https://store.nike.com/us/en_us/pd/air-jordan-13-retro-low-mens-shoe/pid-11396344/pgid-11643346", "https://store.nike.com/us/en_us/pd/air-jordan-13-retro-low-mens-shoe/pid-11396545/pgid-11643346", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-high-top-unisex-shoe/pid-11214023/pgid-11593176", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-high-top-unisex-shoe/pid-11214023/pgid-11593176", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-vintage-canvas-high-top-unisex-shoe/pid-11801737/pgid-11593176", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-high-top-unisex-shoe/pid-11214017/pgid-11593176", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-high-top-unisex-shoe/pid-11215852/pgid-11593176", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11242179/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11242179/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11591573/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11394225/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11394308/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11242180/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11242183/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-max-90-ultra-2-essential-mens-shoe/pid-11242186/pgid-11464954", "https://store.nike.com/us/en_us/pd/air-force-1-07-mens-shoe/pid-10459735/pgid-11406049", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-classic-low-top-unisex-shoe/pid-11214084/pgid-11593182", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-classic-low-top-unisex-shoe/pid-11214084/pgid-11593182", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-classic-low-top-unisex-shoe/pid-10099185/pgid-11593182", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-classic-low-top-unisex-shoe/pid-11214086/pgid-11593182", "https://store.nike.com/us/en_us/pd/air-huarache-ultra-mens-shoe/pid-11394073/pgid-10973990", "https://store.nike.com/us/en_us/pd/air-huarache-ultra-mens-shoe/pid-11394073/pgid-10973990", "https://store.nike.com/us/en_us/pd/air-huarache-ultra-mens-shoe/pid-11393974/pgid-10973990", "https://store.nike.com/us/en_us/pd/air-huarache-ultra-mens-shoe/pid-11394071/pgid-10973990", "https://store.nike.com/us/en_us/pd/air-huarache-ultra-mens-shoe/pid-11155596/pgid-10973990", "https://store.nike.com/us/en_us/pd/air-huarache-ultra-mens-shoe/pid-11052104/pgid-10973990", "https://store.nike.com/us/en_us/pd/air-huarache-ultra-mens-shoe/pid-10872864/pgid-10973990", "https://store.nike.com/us/en_us/pd/air-jordan-1-mid-mens-shoe/pid-11779724/pgid-12080468", "https://store.nike.com/us/en_us/pd/air-jordan-1-mid-mens-shoe/pid-11779724/pgid-12080468", "https://store.nike.com/us/en_us/pd/air-jordan-1-mid-mens-shoe/pid-11595154/pgid-12080468", "https://store.nike.com/us/en_us/pd/air-jordan-1-mid-mens-shoe/pid-11595155/pgid-12080468", "https://store.nike.com/us/en_us/pd/air-jordan-1-mid-mens-shoe/pid-11595157/pgid-12080468", "https://store.nike.com/us/en_us/pd/air-jordan-1-mid-mens-shoe/pid-11154786/pgid-12080468", "https://store.nike.com/us/en_us/pd/air-jordan-1-mid-mens-shoe/pid-11154787/pgid-12080468", "https://store.nike.com/us/en_us/pd/kawa-mens-slide/pid-10949276/pgid-11464876", "https://store.nike.com/us/en_us/pd/sb-benassi-solarsoft-mens-slide/pid-10997078/pgid-12029136", "https://store.nike.com/us/en_us/pd/classic-cortez-shark-low-mens-shoe/pid-10309128/pgid-11139164", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-tumbled-leather-low-top-unisex-shoe/pid-11215850/pgid-11593184", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-tumbled-leather-low-top-unisex-shoe/pid-11215850/pgid-11593184", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-tumbled-leather-low-top-unisex-shoe/pid-11215849/pgid-11593184", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-americana-high-top-unisex-shoe/pid-11214173/pgid-12035165", "https://store.nike.com/us/en_us/pd/converse-thunderbolt-unisex-shoe/pid-11804557/pgid-11867939", "https://store.nike.com/us/en_us/pd/converse-thunderbolt-unisex-shoe/pid-11804557/pgid-11867939", "https://store.nike.com/us/en_us/pd/converse-thunderbolt-unisex-shoe/pid-11837977/pgid-11867939", "https://store.nike.com/us/en_us/pd/converse-thunderbolt-unisex-shoe/pid-11801698/pgid-11867939", "https://store.nike.com/us/en_us/pd/cortez-basic-leather-mens-shoe/pid-10872916/pgid-11464929", "https://store.nike.com/us/en_us/pd/cortez-basic-leather-mens-shoe/pid-10872916/pgid-11464929", "https://store.nike.com/us/en_us/pd/cortez-basic-leather-mens-shoe/pid-10872917/pgid-11464929", "https://store.nike.com/us/en_us/pd/cortez-basic-leather-mens-shoe/pid-10872918/pgid-11464929", "https://store.nike.com/us/en_us/pd/cortez-basic-leather-mens-shoe/pid-10872920/pgid-11464929", "https://store.nike.com/us/en_us/pd/blazer-advanced-mens-shoe/pid-11293881/pgid-11621635", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-low-top-unisex-shoe/pid-11214018/pgid-11593180", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-low-top-unisex-shoe/pid-11214018/pgid-11593180", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-vintage-canvas-low-top-unisex-shoe/pid-11804648/pgid-11593180", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-low-top-unisex-shoe/pid-11214024/pgid-11593180", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-70-low-top-unisex-shoe/pid-11215853/pgid-11593180", "https://store.nike.com/us/en_us/product/air-zoom-mariah-flyknit-racer-id/?piid=43879&pbid=656598560", "https://store.nike.com/us/en_us/product/air-zoom-mariah-flyknit-racer-id/?piid=43879&pbid=656598560", "https://store.nike.com/us/en_us/product/air-zoom-mariah-flyknit-racer-id/?piid=43879&pbid=993181225", "https://store.nike.com/us/en_us/product/air-zoom-mariah-flyknit-racer-id/?piid=43879&pbid=968498763", "https://store.nike.com/us/en_us/pd/air-max-90-essential-mens-shoe/pid-11591297/pgid-11411134", "https://store.nike.com/us/en_us/pd/air-max-90-essential-mens-shoe/pid-11591297/pgid-11411134", "https://store.nike.com/us/en_us/pd/air-max-90-essential-mens-shoe/pid-11591296/pgid-11411134", "https://store.nike.com/us/en_us/pd/air-max-90-essential-mens-shoe/pid-11591298/pgid-11411134", "https://store.nike.com/us/en_us/pd/air-max-90-essential-mens-shoe/pid-10299047/pgid-11411134", "https://store.nike.com/us/en_us/pd/air-max-90-essential-mens-shoe/pid-11787805/pgid-11411134", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-10869060/pgid-11274308", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-10869060/pgid-11274308", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-10869057/pgid-11274308", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-10869058/pgid-11274308", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-11390482/pgid-11274308", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-10869056/pgid-11274308", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-10950616/pgid-11274308", "https://store.nike.com/us/en_us/pd/tanjun-mens-shoe/pid-11049621/pgid-11274308", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=189025247", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=189025247", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=247659331", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=552150670", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=1067652899", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=560861864", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=246239942", "https://store.nike.com/us/en_us/product/air-max-90-id/?piid=43521&pbid=142820861", "https://store.nike.com/us/en_us/pd/air-more-uptempo-96-mens-shoe/pid-11612550/pgid-11645641", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11242175/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11242175/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11373930/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11374008/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11373929/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11373928/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11241117/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11241118/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-ultra-2-essential-mens-shoe/pid-11241121/pgid-11464915", "https://store.nike.com/us/en_us/pd/air-max-1-premium-mens-shoe/pid-11394317/pgid-11464918", "https://store.nike.com/us/en_us/pd/air-max-1-premium-mens-shoe/pid-11394317/pgid-11464918", "https://store.nike.com/us/en_us/pd/air-max-1-premium-mens-shoe/pid-11394318/pgid-11464918", "https://store.nike.com/us/en_us/pd/converse-jack-purcell-low-profile-unisex-slip-on-shoe/pid-11214027/pgid-11593153", "https://store.nike.com/us/en_us/pd/manoadome-mens-boot/pid-11257517/pgid-12134271", "https://store.nike.com/us/en_us/pd/manoadome-mens-boot/pid-11257517/pgid-12134271", "https://store.nike.com/us/en_us/pd/manoadome-mens-boot/pid-11156251/pgid-12134271", "https://store.nike.com/us/en_us/pd/manoadome-mens-boot/pid-11239021/pgid-12134271", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-americana-low-top-unisex-shoe/pid-11214169/pgid-12035164", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-leather-unisex-high-top-shoe/pid-11214003/pgid-11593152", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-leather-unisex-high-top-shoe/pid-11214003/pgid-11593152", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-leather-high-top-unisex-shoe/pid-11214089/pgid-11593152", "https://store.nike.com/us/en_us/pd/lunarcharge-essential-bn-mens-shoe/pid-11673688/pgid-11870795", "https://store.nike.com/us/en_us/pd/lunarcharge-essential-bn-mens-shoe/pid-11673688/pgid-11870795", "https://store.nike.com/us/en_us/pd/lunarcharge-essential-bn-mens-shoe/pid-11652135/pgid-11870795", "https://store.nike.com/us/en_us/pd/lunarcharge-essential-bn-mens-shoe/pid-11774787/pgid-11870795", "https://store.nike.com/us/en_us/pd/lunarcharge-essential-bn-mens-shoe/pid-11652134/pgid-11870795", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-low-top-unisex-shoe/pid-11214175/pgid-11337715", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-high-top-shoe/?piid=43001&pbid=631645880", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-high-top-shoe/?piid=43001&pbid=631645880", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-high-top-shoe/?piid=43001&pbid=686918104", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-high-top-shoe/?piid=43001&pbid=615676125", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-high-top-shoe/?piid=43001&pbid=686877383", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-high-top-shoe/?piid=43001&pbid=497657527", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-high-top-shoe/?piid=43546&pbid=597989199", "https://store.nike.com/us/en_us/pd/flight-bonafide-mens-shoe/pid-11598239/pgid-11623930", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-leather-low-top-unisex-shoe/pid-11214006/pgid-11593155", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43005&pbid=1042211679", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43005&pbid=1042211679", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43005&pbid=655830723", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43005&pbid=695228566", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43005&pbid=470049317", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43005&pbid=966546184", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43005&pbid=329110302", "https://store.nike.com/us/en_us/product/custom-converse-chuck-taylor-leather-low-top-shoe/?piid=43547&pbid=243322723", "https://store.nike.com/us/en_us/pd/air-unlimited-mens-shoe/pid-11463449/pgid-11494587", "https://store.nike.com/us/en_us/pd/jordan-spizike-mens-shoe/pid-11601778/pgid-11418057", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-slide-mens-sandal/pid-11110762/pgid-11110764", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-slide-mens-sandal/pid-11110762/pgid-11110764", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-slide-mens-sandal/pid-11111681/pgid-11110764", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-looney-tunes-high-top-unisex-shoe/pid-11804762/pgid-12019786", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-looney-tunes-high-top-unisex-shoe/pid-11804762/pgid-12019786", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-bugs-bunny-high-top-unisex-shoe/pid-11866670/pgid-12019786", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-elite-mens-sandal/pid-11111855/pgid-11111857", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-elite-mens-sandal/pid-11111855/pgid-11111857", "https://store.nike.com/us/en_us/pd/hurley-phantom-free-elite-mens-sandal/pid-11112060/pgid-11111857", "https://store.nike.com/us/en_us/pd/converse-chuck-taylor-all-star-pride-geostar-high-top-unisex-shoe/pid-11801804/pgid-11910026", "https://store.nike.com/us/en_us/pd/air-max-uptempo-95-mens-shoe/pid-11608586/pgid-11660930", "https://store.nike.com/us/en_us/pd/kawa-printed-mens-slide/pid-11256213/pgid-11605297", "https://store.nike.com/us/en_us/pd/kawa-printed-mens-slide/pid-11256213/pgid-11605297", "https://store.nike.com/us/en_us/pd/kawa-printed-mens-slide/pid-11256215/pgid-11605297", "https://store.nike.com/us/en_us/pd/solarsoft-comfort-mens-slide/pid-10722123/pgid-10278224", "https://store.nike.com/us/en_us/pd/solarsoft-comfort-mens-slide/pid-10722123/pgid-10278224", "https://store.nike.com/us/en_us/pd/Nike-Air-VaporMax-College-Navy/pid-10722124/pgid-10278224", "https://store.nike.com/us/en_us/pd/air-max-95-essential-mens-shoe/pid-11591321/pgid-10338610", "https://store.nike.com/us/en_us/pd/Nike-Air-VaporMax-College-Navy/pid-11395686/pgid-11451380" };
    string googleRespStr;
    //public AmazonS3Client Client;
    public string S3Region = RegionEndpoint.USEast1.SystemName;
    private RegionEndpoint _S3Region
    {
        get { return RegionEndpoint.GetBySystemName(S3Region); }
    }

    /**   private static readonly RegionEndpoint _lambdaRegion = null;
       private RegionEndpoint LambdaRegion { get { return _lambdaRegion != null ? _lambdaRegion : AWSConfigs.RegionEndpoint; } }
       private CognitoAWSCredentials _credentials;
       private IAmazonLambda _lambdaClient;
       private static readonly RegionEndpoint _cognitoRegion = null;
       private RegionEndpoint CognitoRegion { get { return _cognitoRegion != null ? _cognitoRegion : AWSConfigs.RegionEndpoint; } }

       private CognitoAWSCredentials Credentials
       {
           get
           {
               if (_credentials == null)
                   _credentials = new CognitoAWSCredentials("us-east-1_uTBtftt9k", RegionEndpoint.USEast1);
               return _credentials;
           }
       }


       private IAmazonLambda LambdaClient
       {
           get
           {
               if (_lambdaClient == null)
               {
                   _lambdaClient = new AmazonLambdaClient(Credentials, RegionEndpoint.USEast1);
               }
               return _lambdaClient;
           }
       } **/

    public GameObject buttonObject;
    private GameObject first_line_Object;
    private GameObject second_line_Object;
    // Use this for initialization
#if UNITY_ANDROID
    [DllImport ("opencv_core249")]
    private static extern float FooPluginFunction();
    [DllImport ("opencv_highgui249")]
    private static extern float FooPluginFunction2();
#endif

    void Start()
    {
        UnityInitializer.AttachToGameObject(this.gameObject);
        //var credentials = new CognitoAWSCredentials("us-east-1_uTBtftt9k", RegionEndpoint.USEast1);
        //var Client = new AmazonLambdaClient(credentials, RegionEndpoint.USEast1);
        //Client = new AmazonS3Client(credentials);
        buttonObject = GameObject.Find("Button");
    }

    
    #region private members
    private IAmazonS3 _s3Client;
    private AWSCredentials _credentials;

    private AWSCredentials Credentials
    {
        get
        {
            if (_credentials == null)
                _credentials = new CognitoAWSCredentials("us-east-1:72fb36e6-66c8-4e33-8b5e-dd4054c26511", RegionEndpoint.USEast1);
            return _credentials;
        }
    }

    private IAmazonS3 Client
    {
        get
        {
            if (_s3Client == null)
            {
                _s3Client = new AmazonS3Client(Credentials, _S3Region);
            }
            //test comment
            return _s3Client;
        }
    }
    
#endregion
    public void TakePhoto() {
        buttonObject.SetActive(false);
        byte[] imByteArr;
        ArrayList imgurls = new ArrayList();
        ArrayList imglinks = new ArrayList();
        string filepath = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture635.jpg";
        //string filepath2 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture636.jpg";
        string filepath2 = "";
        if (!Application.isMobilePlatform)
        {
            filepath2 = Application.dataPath + "\\testpicture3.jpg";
        }
        else
        {
            filepath2 = "jar:file:\\" + Application.dataPath + "!\assets" + "\\testpicture3.jpg";
            WWW wwwfile = new WWW(filepath2);
            while (!wwwfile.isDone) { }
            filepath2 = wwwfile.text;
            string realPath = Application.persistentDataPath + "//testpicture3.jpg";
            System.IO.File.WriteAllBytes(realPath, wwwfile.bytes);
            filepath2 = realPath;
        }
        string filepath3 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture637.jpg";
        string filepath4 = "";
        string filepath5 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture97.jpg";
        string filepath6 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture2032.jpg";
        string filepath7 = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture3431.jpg";
        string filepath_res = "";
        float min = float.MaxValue;
        int size = 0;
        int times = 1;
        int i = 0;

        Image<Bgr, Byte> picture19 = new Image<Bgr, byte>(filepath2);
        print(Application.persistentDataPath);
        print(Application.dataPath);
        //filepath2 = Application.persistentDataPath + "/testpicture9.jpg";
        //File.WriteAllBytes(filepath2, picture19.Bytes);


   /**     if (Application.isMobilePlatform)
        {
            filepath4 = Application.persistentDataPath + "/picture638.jpg";
            print(filepath4);
            print(filepath2);
            Application.CaptureScreenshot(filepath4);
        }
        else
        {
            filepath4 = Application.dataPath + "/picture638.jpg";
            Application.CaptureScreenshot(filepath4);
        } **/

        first_line_Object = GameObject.Find("line1");
        second_line_Object = GameObject.Find("line2");
        first_line_Object.SetActive(false);
        first_line_Object.transform.parent.gameObject.SetActive(false);
        second_line_Object.SetActive(false);
        second_line_Object.transform.parent.gameObject.SetActive(false);


     /*   string conn = "URI=file:" + Application.dataPath + "/WhatAreThose (4).db";
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
        print("here3"); */

        

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
        //string parta = "C:\\Users\\Sandeep\\Documents\\What_Are_Those\\Assets\\picture";
        string parta = "";
        if (Application.isMobilePlatform)
        {
            parta = "jar:file:\\" + Application.dataPath + "!\assets" + "\\picture";
        }
        else
        {
            parta = Application.dataPath + "\\StreamingAssets" + "\\picture";
        }
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
          //      reader.Close();
           //     reader = null;
                
        int counter = 0;
        byte[,,] data = picture85.Data;
        
        byte[,,] data_two = picture19.Data;
        var red = data_two[0, 0, 2];
        var green = data_two[0, 0, 1];
        var blue = data_two[0, 0, 0];
        print("bgr is " + blue + " " + green + " " + red);
        // To resize the image 
        //Image<Bgr, byte> resizedImage = picture19.Resize(picture8.Width, picture8.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
        Boolean recfound = false;
        int mindiffarea = 36000;
        //if (red < 246 && green < 246 && blue < 246) picture20 = ObjectDetector(picture19, filepath2); else picture20 = picture19;

        //Image<Gray, Byte> picture21 = new Image<Gray, byte>(picture20.Bitmap);
        for (int n=1; n < 120 && !recfound; n++)
        {
            for(int m=1; m<=3 && !recfound; m++)
            {
                for(int p=1; p<=m && !recfound; p++)
                {
                    if (n == 45 || n == 46|| n == 63 || n== 82 || n == 83 || n == 112 || n == 114 || n  == 117 || n == 120) continue;
                    int number = 100 * n + m * 10 + p;
                    int resnumber = number * 10;
                    //print("number is " + number);
                    if (number >= 131)
                    {
                        string s = "" + number;
                        string r = "" + resnumber;
                        filepath = parta + s + partb;
                        //print("filepath is " + filepath);
                        if (Application.platform == RuntimePlatform.Android)
                        {

                            WWW wwwfile = new WWW(filepath);
                            while (!wwwfile.isDone) { }
                            string realPath = Application.persistentDataPath + "//picture"+s+partb;
                            System.IO.File.WriteAllBytes(realPath, wwwfile.bytes);
                            filepath = realPath;
                        }

                        //filepath_res = Application.persistentDataPath + "/picture"+ s + partb;

                        try
                        {
                            //print("filepath is " + filepath);
                            Image<Gray, byte> picture6 = new Image<Gray, byte>(filepath);
                            Image<Bgr, Byte> colorImage = new Image<Bgr, Byte>(filepath);
                            //File.WriteAllBytes(filepath_res,colorImage.Bytes);
                            //Image<Bgr, Byte> color2Image = new Image<Bgr, Byte>(Application.persistentDataPath + "\\picture" + s + partb);
                            Image<Bgr, byte> resizedImage = picture19.Resize(picture6.Width, picture6.Height, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
                            Image<Gray, Byte> picture21 = new Image<Gray, byte>(resizedImage.Bitmap);
                            //print("width is " + picture6.Width + " height is " + picture6.Height);
                            int picturearea = picture6.Width * picture6.Height * 2;
                            long matchTime;
                            //Image<Bgr, byte> picture5 = null;
                            //Image<Bgr, Byte> picture5 =  Draw(picture6, picture21, out matchTime);
                            float[] a;
                            imagebytes = colorImage.Bytes;
                            String res = "";
                            /**
                                                        var request = new InvokeRequest()
                                                        {
                                                            FunctionName = "SearchThroughS3Bucket",
                                                            Payload = "{\"key3\" : \"value3\"}",
                                                            InvocationType = InvocationType.Event
                                                        };
                                                        Client.InvokeAsync(request, (responseObject) => {
                                                            if (responseObject.Exception == null)
                                                            {
                                                                print("RES");
                                                                Debug.Log(Encoding.ASCII.GetString(responseObject.Response.Payload.ToArray()));
                                                                //print("RES");
                                                            }
                                                            else
                                                            {
                                                                Debug.LogError(responseObject.Exception);
                                                            }
                                                        });
                                                        **/


                            //PostImage(colorbytes);

                            //StartCoroutine("OnPostRender");

                            PostObject("testpicture12.jpg");
                            a = GetVector(colorImage);
              /**              if (!Application.isMobilePlatform)
                            {
                                a = GetVector(colorImage);
                                hog = new HOG(image, 8, 8, 2, 2);
                                a1 = hog.Describe();
                            }
                            else
                            {
                                //a = GetVector(color2Image);
                                a = GetVector(colorImage);
                                hog = new HOG(image, 8, 8, 2, 2);
                                a1 = hog.Describe();
                            } **/
                            //PostImage(resizedImage.Bytes);
                            float[] b = GetVector(resizedImage);
                            //b1 = hog2.Describe();
                            float dst = distance(a, b);
                            //float dst = doubledistance(a1, b1);
                            //print(filepath + " " +dst+" "+(n-1));
                           if (dst < min) {
                              min = dst;
                              maxint = n - 1;
                              print("min " + "is " + min + "max int is " + maxint);
                            } 
                            


                            //try { picture5 = Drawtwo(picture3, picture6); } catch(NullReferenceException e){ }
                            //print("nzc is now " + nonZeroCount + " picture name is " + s);

                            //picture5.Save(filepath_res);
                            //print("nzc is now " + r + " "+area);
                            /*
                            int diffarea = Math.Abs(picturearea - area);
                            if(diffarea < mindiffarea)
                            {
                                print(filepath_res+" diff area is " + diffarea);
                                mindiffarea = diffarea;
                                maxint = n - 1;
                            } */
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
        //Image<Bgr, Byte> picture12 = new Image<Bgr, byte>(filepath2);
        //Image<Bgr, Byte> picture7 = ObjectDetector(picture12,filepath2);
        //Image<Bgr, Byte> picture7 = Whiteout(picture2);
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
 /*       dbcmd.CommandText = sqlQuery2;
        IDataReader reader2 = dbcmd.ExecuteReader(); */
        count = 0;
        /*     while (reader2.Read()  && count < 120)
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
             } */
        int ind = 0;
        while (ind< jsonstring.Length && count < 120)
        {
            //print("here5");
            imglink = jsonstring[ind];
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

                else if (imglink.Contains("pbid"))
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
            ind++;
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
        second_line_Object.GetComponent<Text>().text = (string)imglinks[maxint];
        //second_line_Object.GetComponent<Text>().text = "kyrie-3-mens-basketball-shoe";
        //second_line_Object.GetComponent<Text>().text = res;


        /*     dbcmd.Dispose();
             dbcmd = null;
             dbconn.Close();
             dbconn = null; */



        /**
        Image<Gray, byte> picture6 = new Image<Gray, byte>(filepath);
        long matchTime;
        Image<Bgr, byte> picture5 = Drawtwo(picture3, picture6);
        picture5.Save(filepath3);
        **/
    }
    void RetrieveGPSData()
    {
        currentGPSPosition = Input.location.lastData;
        gpsString = "Lat: " + currentGPSPosition.latitude + "  Lon: " + currentGPSPosition.longitude + "  Alt: " + currentGPSPosition.altitude +
          "  HorAcc: " + Input.location.lastData.horizontalAccuracy + "  VerAcc: " + Input.location.lastData.verticalAccuracy + "  TS: " + Input.location.lastData.timestamp;
    }
    IEnumerator Radar()
    {
        GOOGLE_SEARCH_URL = "https://maps.googleapis.com/maps/api/place/radarsearch/json?location=" + currentGPSPosition.latitude + "," + currentGPSPosition.longitude + "&radius=" + radarRadius + "&types=" + radarType + "&sensor=false" + radarSensor + "&key=" + GOOGLE_PLACES_API_KEY;
        WWW googleResp = new WWW(GOOGLE_SEARCH_URL);
        yield return googleResp;
        googleRespStr = googleResp.text;
        print(googleRespStr);
    }

    // filename is testpicture12.jpg
    public void PostObject(string fileName)
    {

        AWSConfigs.HttpClient = AWSConfigs.HttpClientOption.UnityWebRequest;
        var stream = new FileStream(Application.persistentDataPath + Path.DirectorySeparatorChar + fileName,
        FileMode.Open, FileAccess.Read, FileShare.Read);

        var request = new PostObjectRequest()
        {
            Bucket = "visionprocessing",
            Key = fileName,
            InputStream = stream,
            CannedACL = S3CannedACL.Private,
            Region = _S3Region,
            ContentType = "image/jpg"
        };

        //print(request);
        //print(Client);
        Client.PostObjectAsync(request, (responseObj) =>
        {
            if (responseObj.Exception == null)
            {
                //ResultText.text += string.Format("\nobject {0} posted to bucket {1}",
                //responseObj.Request.Key, responseObj.Request.Bucket);
                print("no error");
            }
            else
            {
                //ResultText.text += "\nException while posting the result object";
                //ResultText.text += string.Format("\n receieved error {0}",
                //responseObj.Response.HttpStatusCode.ToString());
                print(responseObj.Exception.ToString());
            }
        });

    }
    public IEnumerator PostImage(byte[] image)
    {
        WWWForm myForm = new WWWForm();
        myForm.AddBinaryData("file", image);
        WWW getvector = new WWW(apilink, myForm);
        yield return getvector;
        //print(getvector.text);
        parsepost(getvector.text);
    }
    public IEnumerator OnPostRender()
    {
        Hashtable postHeader = new Hashtable();
        postHeader.Add("Content-Type", "image/png");

        var formData = System.Text.Encoding.UTF8.GetBytes(imagebytes.ToString());
        WWWForm form = new WWWForm();
        form.AddBinaryData("byte array",imagebytes);
        WWW www = new WWW(apilink,imagebytes,postHeader);
        //print("HERE");
        yield return www;
        res = www.text;
        //print("HERE");
        print("HERE HERE HERE"+www.text + "RES IS "+res);
        HOGImage hogimage = new HOGImage();
        hogimage = JsonUtility.FromJson<HOGImage>(res);
        vector = hogimage.hogoutput;
        //res = www.text;
    }
    public IEnumerator OnPlacesRender()
    {
        WWW www = new WWW(GOOGLE_SEARCH_URL);
        yield return www;
    }
    public float[] parsepost(String text)
    {
        float[] a = new float[2];
        return a;
    }
    public float[] parsestring (String res, float[] a)
    {
        char[] chars = res.ToCharArray();
        int c = 0;
        int spaces = 0;
        for (int q = 1; q < res.Length; q++) if (chars[q] == ' ') spaces++;
        a = new float[spaces + 1];
        for (int q = 1; q < res.Length; q++)
        {
            String fnum = "";
            if (chars[q] != ' ') fnum += chars[q]; else a[c] = (float)Convert.ToDouble(fnum);

        }
        print("float values:");
        for (int q = 0; q < a.Length; q++)
        {
            print(a[q] + " ");
        }
        return a;
    }
    public float distance(float[] a, float[] b)
    {
        double sum = 0;
        for(int i =0; i < a.Length; i++)
        {
            double diff = (a[i] - b[i]);
            sum += Math.Pow(diff, 2.0);
        }
        float distance = (float) Math.Sqrt(sum);
        return distance;
    }
    public float doubledistance(double[] a, double[] b)
    {
        double sum = 0;
        for (int i = 0; i < a.Length; i++)
        {
            double diff = (a[i] - b[i]);
            sum += Math.Pow(diff, 2.0);
        }
        float distance = (float)Math.Sqrt(sum);
        return distance;
    }
#if UNITY_EDITOR
    public float[] GetVector(Image<Bgr, Byte> im)
    {
        HOGDescriptor hog = new HOGDescriptor(); 
        Image<Bgr, Byte> imageOfInterest = im.Resize(64, 128, Emgu.CV.CvEnum.INTER.CV_INTER_LINEAR);
        Point[] p = new Point[imageOfInterest.Width * imageOfInterest.Height];
        int k = 0;
        for (int i = 0; i < imageOfInterest.Width; i++)
        {
            for (int j = 0; j < imageOfInterest.Height; j++)
            {
                Point p1 = new Point(i, j);
                p[k++] = p1;
            }
        }

        return hog.Compute(imageOfInterest, new Size(8, 8), new Size(0, 0), p);
    }
#endif
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
    [Serializable]
    public class HOGImage
    {
        public float[] hogoutput;
    }
}
