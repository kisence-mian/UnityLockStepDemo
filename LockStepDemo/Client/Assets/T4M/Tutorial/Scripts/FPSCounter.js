var updateInterval = 0.5;
var x_location = 5;
var y_location = 5;

private var lastInterval : double; // Last interval end time
private var frames = 0; // Frames over current interval
private var fps : float; // Current FPS


function Awake () {
    useGUILayout = false;
}


function OnGUI () {
    GUI.Label (Rect(Screen.width-x_location, Screen.height- y_location, 100, 30), "FPS: " + fps.ToString("f2"));
}


function Start()
{
    lastInterval = Time.realtimeSinceStartup;
    frames = 0;
}


function Update()
{
    ++frames;
    var timeNow = Time.realtimeSinceStartup;
    if( timeNow > lastInterval + updateInterval )
    {
        fps = frames / (timeNow - lastInterval);
        frames = 0;
        lastInterval = timeNow;
    }
}