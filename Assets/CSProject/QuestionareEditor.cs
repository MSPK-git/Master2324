using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class QuestionareEditor : EditorWindow
{
    [SerializeField]
    private VisualTreeAsset m_VisualTreeAsset = default;

    // CSV varviables

    [SerializeField]
    private string personName;

    [SerializeField]
    private int age;

    [SerializeField]
    private float speedSpline = 0.0001f;

    [SerializeField]
    private float speedAnimation = 1.0f;

    [SerializeField]
    private string setup = "default";

    [SerializeField]
    private int simulationTime = 60;

    [SerializeField]
    private bool vr = true;

    // other variables
    [SerializeField]
    private string creationTime = DateTime.Now.ToString("yyyyMMddHHmmss");

    [SerializeField]
    private DateTime playStartTime;

    [SerializeField]
    private bool running = false;

    QuestionareEditor()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private void AppendCSV(string path)
    {
        if (!File.Exists(path))
        {
            string csvHeader = "Name;Age;SpeedSpline;SpeedAnimation;Setup;Time;VRExperience" + System.Environment.NewLine;
            File.WriteAllText(path, csvHeader);
        }

        File.AppendAllText(path, $"{personName};{age};{speedSpline};{speedAnimation};{setup};{simulationTime};{vr}" + System.Environment.NewLine);
    }

    private void OnPlayModeChanged(PlayModeStateChange change)
    {
        VisualElement root = rootVisualElement;
        IntegerField simulationField = root.Query<IntegerField>("simulationTime").First();

        if (change == PlayModeStateChange.EnteredPlayMode)
        {
            playStartTime = DateTime.Now;
            running = true;

            if (simulationField != null)
            {
                simulationField.isReadOnly = true;
            }

            // Set simulation parameters
            GameObject breakdance = GameObject.Find("breakdance");
            Animator bdAnimation = breakdance.GetComponent<Animator>();
            SplineAnimate splineAnimate = breakdance.GetComponent<SplineAnimate>();
            bdAnimation.speed = speedAnimation;
            splineAnimate.MaxSpeed = speedSpline;

            //UnityEngine.Rendering.Universal.Vignette vignette;
            //Volume volume = Camera.main.GetComponent<Volume>();
            //volume.profile.TryGet(out vignette);
            //vignette.active = setup.Equals("blur") || setup.Equals("both");

            UnityEngine.Rendering.Universal.DepthOfField depthOfField;
            Volume volume = Camera.main.GetComponent<Volume>();
            volume.profile.TryGet(out depthOfField);
            depthOfField.active = setup.Equals("blur") || setup.Equals("both");

            MeshRenderer renderer = Camera.main.gameObject.GetComponentInChildren<MeshRenderer>();
            renderer.enabled = setup.Equals("companion") || setup.Equals("both");
        }
        else if (change == PlayModeStateChange.ExitingPlayMode)
        {
            running = false;

            // Update simulation TextField with actual running time of simulation
            int secondsSinceSimulationStart = (int) (DateTime.Now - playStartTime).TotalSeconds;
            simulationField.value = secondsSinceSimulationStart;
            simulationField.isReadOnly = false;
        }
    }

    private void OnClickSave()
    {
        string path = "questionare_" + creationTime + ".csv";
        Console.WriteLine("Exiting play mode, writing CSV to " + path);
        AppendCSV(path);
    }

    public void Update()
    {
        if (running)
        {
            // Track running time and stop playing when simulationTime elapsed
            var delta = DateTime.Now - playStartTime;
            if (delta.TotalSeconds >= simulationTime)
            {
                EditorApplication.isPlaying = false;
            }
        }
    }

    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;

        VisualElement uxml = m_VisualTreeAsset.Instantiate();
        root.Add(uxml);

        // Restore values and register change callback

        var nameField = root.Query<TextField>("name").First();
        nameField.RegisterCallback<ChangeEvent<string>>((value) => personName = value.newValue);
        nameField.value = personName;

        IntegerField ageField = root.Query<IntegerField>("age").First();
        ageField.RegisterCallback<ChangeEvent<int>>((value) => age = value.newValue);
        ageField.value = age;

        FloatField speedField1 = root.Query<FloatField>("speedSpline").First();
        speedField1.RegisterCallback<ChangeEvent<float>>((value) => speedSpline = value.newValue);
        speedField1.value = speedSpline;

        FloatField speedField2 = root.Query<FloatField>("speedAnimation").First();
        speedField2.RegisterCallback<ChangeEvent<float>>((value) => speedAnimation = value.newValue);
        speedField2.value = speedAnimation;

        DropdownField setupField = root.Query<DropdownField>("setup").First();
        setupField.RegisterCallback<ChangeEvent<string>>((value) => setup = value.newValue);
        setupField.value = setup;

        IntegerField simuTimeField = root.Query<IntegerField>("simulationTime").First();
        simuTimeField.RegisterCallback<ChangeEvent<int>>((value) => simulationTime = value.newValue);
        simuTimeField.value = simulationTime;

        Toggle vrField = root.Query<Toggle>("vr").First();
        vrField.RegisterCallback<ChangeEvent<bool>>((value) => vr = value.newValue);
        vrField.value = vr;

        // Link save button

        root.Query<Button>("save").First().clicked += OnClickSave;
    }

    [MenuItem("Tools/Questionare")]
    public static void ShowEditor()
    {
        // This method is called when the user selects the menu item in the Editor
        EditorWindow wnd = GetWindow<QuestionareEditor>();
        wnd.titleContent = new GUIContent("Questionare");
    }
}
