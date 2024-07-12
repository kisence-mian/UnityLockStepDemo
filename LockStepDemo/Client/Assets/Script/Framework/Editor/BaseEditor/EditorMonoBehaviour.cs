
using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public class EditorMonoBehaviour
{
    static EditorMonoBehaviour()
    {

        var type = Assembly.Load("UnityEditor").GetType("UnityEditor.EditorAssemblies");
        var method = type.GetMethod("SubclassesOf", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Instance, null, new Type[] { typeof(Type) }, null);
        var e = method.Invoke(null, new object[] { typeof(EditorMonoBehaviour) }) as IEnumerable;
        foreach (Type editorMonoBehaviourClass in e)
        {
            method = editorMonoBehaviourClass.BaseType.GetMethod("OnEditorMonoBehaviour", BindingFlags.NonPublic | BindingFlags.Instance);
            if (method != null)
            {
                method.Invoke(System.Activator.CreateInstance(editorMonoBehaviourClass), new object[0]);
            }
        }
    }

    private void OnEditorMonoBehaviour()
    {
        EditorApplication.update += EditorUpdate;
        EditorApplication.hierarchyWindowChanged += OnHierarchyWindowChanged;
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItemOnGUI;
        EditorApplication.projectWindowChanged += OnProjectWindowChanged;
        EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGUI;
        EditorApplication.modifierKeysChanged += OnModifierKeysChanged;

        EditorApplication.CallbackFunction function = () => OnGlobalEventHandler(Event.current);
        FieldInfo info = typeof(EditorApplication).GetField("globalEventHandler", BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic);
        EditorApplication.CallbackFunction functions = (EditorApplication.CallbackFunction)info.GetValue(null);
        functions += function;
        info.SetValue(null, (object)functions);
        EditorApplication.searchChanged += OnSearchChanged;
        EditorApplication.playmodeStateChanged += () =>
        {
            if (EditorApplication.isPaused)
            {
                OnPlaymodeStateChanged(PlayModeState.Paused);
            }
            if (EditorApplication.isPlaying)
            {
                OnPlaymodeStateChanged(PlayModeState.Playing);
            }
            if (EditorApplication.isPlayingOrWillChangePlaymode)
            {
                OnPlaymodeStateChanged(PlayModeState.PlayingOrWillChangePlayMode);
            }
        };
        Start();
    }
    public double deltaTime;
    private double m_PreviousTime;
    private void EditorUpdate()
    {
        deltaTime = EditorApplication.timeSinceStartup - m_PreviousTime;
        m_PreviousTime = EditorApplication.timeSinceStartup;
        Update();
    }

    public virtual void Start()
    { }

    public virtual void Update()
    { }

    public virtual void OnHierarchyWindowChanged()
    { }

    public virtual void HierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
    { }

    public virtual void OnProjectWindowChanged()
    { }

    public virtual void ProjectWindowItemOnGUI(string guid, Rect selectionRect)
    { }

    public virtual void OnModifierKeysChanged()
    { }

    public virtual void OnGlobalEventHandler(Event e)
    { }

    public virtual void OnSearchChanged()
    { }

    public virtual void OnPlaymodeStateChanged(PlayModeState playModeState)
    { }

    public enum PlayModeState
    {
        Playing,
        Paused,
        Stop,
        PlayingOrWillChangePlayMode
    }
}
