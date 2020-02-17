using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DayNightCycle : MonoBehaviour {
    
    public static DayNightCycle Instance;
    public float TimeScale = 0.2f;            // Game Day ratio to Real Day
    public TimeSpan GameTime = new TimeSpan(); // Current adjusted game time
    private float gameSeconds = 0;             // Accumulated 'Game Time' in seconds
    public Text TimeText;
    public GameObject SunMoon;
    public GameObject SunMoonPrefab;
    public Light SunLight;
    public Light MoonLight;
    public GameObject Map;
    
    public Gradient gradient;
    public GradientColorKey[] colorKey;
    public GradientAlphaKey[] alphaKey;
    public Color DayColour;
    public Color NightColour;

    public Camera MainCamera;
    public AnimationCurve ColourCurve;
    public AnimationCurve LightIntensityCurve;
    public float LightIntensity;

    void Awake() 
    {
        Instance = this;
        Map = GameObject.FindWithTag("Map");
        SunMoon = Instantiate(SunMoonPrefab, new Vector3(Map.transform.position.x / 2, Map.transform.position.y / 2, Map.transform.position.z / 2), SunMoonPrefab.transform.rotation);
        SunMoon = GameObject.FindWithTag("SunMoon");
        SunLight = SunMoon.transform.GetChild(0).GetComponent<Light>();
        MoonLight = SunMoon.transform.GetChild(1).GetComponent<Light>();
        MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
    }

    void Start()
    {
        Debug.Log("aaaaaaaaaaaaaaa");
    }

    public void Update() 
    {                    
        gameSeconds += Time.deltaTime * TimeScale;
        GameTime = TimeSpan.FromSeconds(gameSeconds); // Convert to a usable format
        TimeText.text = ToString();
        //Debug.Log("Current Game Time: " + ToString());

        if (SunMoon)
        {
            SunMoon.transform.RotateAround(Map.transform.position, Vector3.forward, 360f/86400f * TimeScale * Time.deltaTime);
            
            //SunMoon.transform.GetChild(0).LookAt(Vector3.forward);
            SunMoon.transform.GetChild(0).LookAt(Map.transform);
            SunMoon.transform.GetChild(1).LookAt(Map.transform);
            //SunMoon.transform.GetChild(0).eulerAngles = new Vector3(-90, -180, 0);
            //SunMoon.transform.GetChild(1).eulerAngles = new Vector3(-90, -180, 0);
            //SunMoon.transform.GetChild(1).LookAt(Vector3.forward);
        }
        
        float normalisedTime = ((float)GameTime.TotalSeconds % 86400) / 86400f;
        Debug.Log(normalisedTime);
        float lerpTime = ColourCurve.Evaluate(normalisedTime);
        //MainCamera.backgroundColor = Color.Lerp(NightColour, DayColour, lerpTime);
        MainCamera.backgroundColor = gradient.Evaluate(lerpTime);
        float lightIntensity = LightIntensityCurve.Evaluate(normalisedTime);
        SunLight.intensity = lightIntensity;
        //MoonLight.intensity = lightIntensity;
    }

    public override string ToString() {
        //                        DAYS,  HOUR : MINS : SECS . MS
        return string.Format("{0} Days, {1:00}:{2:00}:{3:00}.{4:000}", GameTime.Days, GameTime.Hours, GameTime.Minutes, GameTime.Seconds, GameTime.Milliseconds);
    }
}
