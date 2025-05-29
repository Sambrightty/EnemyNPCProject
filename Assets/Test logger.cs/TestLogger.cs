/*
 * TestLogger.cs  —  Unity utility for CS3IP solo testing
 * © 2025 Mahe Mustapha Hashim
 *
 * Drop this on a persistent GameObject. Call StartTest / EndTest
 * from scripted scenarios. Extend the TestResult class to add
 * extra columns (mutation_count, dodge_success_rate, etc.).
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;

public class TestLogger : MonoBehaviour
{
    #region Singleton
    public static TestLogger Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    // ----- DATA STRUCTURE ---------------------------------------------------
    [Serializable]
    public class TestResult
    {
        public string TestID;
        public string DateTimeISO;
        public string FeatureComponent;
        public string Scenario;
        public string ExpectedOutcome;
        public string ObservedOutcome;
        public float ReactionMs;
        public float AvgFps;
        public bool Passed;
        public string BugsOrIssues;
        public string FixOrAction;
        public string TestedBy;
        public string Notes;

        // Extra telemetry — extend as needed
        public float MutationFreq;
        public float DodgeSuccessRate;

        public string ToCsv()
        {
            // Escape commas by wrapping in quotes
            string esc(string s) => $"\"{s.Replace("\"", "\"\"")}\"";
            return string.Join(",", new[]
            {
                esc(TestID), esc(DateTimeISO), esc(FeatureComponent), esc(Scenario),
                esc(ExpectedOutcome), esc(ObservedOutcome),
                ReactionMs.ToString("F1", CultureInfo.InvariantCulture),
                AvgFps.ToString("F1", CultureInfo.InvariantCulture),
                Passed ? "Pass" : "Fail",
                esc(BugsOrIssues), esc(FixOrAction), esc(TestedBy), esc(Notes),
                MutationFreq.ToString("F2", CultureInfo.InvariantCulture),
                DodgeSuccessRate.ToString("F2", CultureInfo.InvariantCulture)
            });
        }
    }

    // ----- INTERNAL FIELDS --------------------------------------------------
    readonly List<TestResult> _results = new();
    TestResult _currentTest;
    float _fpsSum;
    int   _fpsFrames;
    float _testStartTime;
    string _csvPath;

    // ----- PUBLIC API -------------------------------------------------------
    public void StartTest(string id, string scenario, string expectedOutcome,
                          string featureComponent = "", string testedBy = "Solo-Dev")
    {
        if (_currentTest != null) Debug.LogWarning("Previous test not ended! Auto-ending.");
        _fpsSum = 0; _fpsFrames = 0;
        _testStartTime = Time.realtimeSinceStartup;

        _currentTest = new TestResult
        {
            TestID          = id,
            DateTimeISO     = DateTime.UtcNow.ToString("s", CultureInfo.InvariantCulture),
            FeatureComponent= featureComponent,
            Scenario        = scenario,
            ExpectedOutcome = expectedOutcome,
            TestedBy        = testedBy
        };
    }

  public void EndTest(string observedOutcome, bool passed,
                      string bugs = "", string fix = "", string notes = "",
                      float reactionMs = -1f, float mutationFreq = -1f,
                      float dodgeSuccess = -1f)
  {
    if (_currentTest == null) { Debug.LogError("EndTest called before StartTest"); return; }

    _currentTest.ObservedOutcome = observedOutcome;
    _currentTest.Passed = passed;
    _currentTest.BugsOrIssues = bugs;
    _currentTest.FixOrAction = fix;
    _currentTest.Notes = notes;
    _currentTest.ReactionMs = reactionMs;
    _currentTest.MutationFreq = mutationFreq;
    _currentTest.DodgeSuccessRate = dodgeSuccess;
    _currentTest.AvgFps = _fpsFrames > 0 ? _fpsSum / _fpsFrames : 0;

    _results.Add(_currentTest);
    _currentTest = null;
        Flush(); // Save the result to CSV immediately after each test

    }

    public void Flush()
    {
        if (_results.Count == 0) return;

        if (_csvPath == null)
        {
            _csvPath = Path.Combine(Application.persistentDataPath, "Unity_NPC_Test_Report.csv");
            // Write header if file does not yet exist
            if (!File.Exists(_csvPath))
            {
                var header = "Test ID,DateTime,Feature / Component,Scenario," +
                             "Expected Outcome,Observed Outcome,Reaction_ms,Avg_FPS," +
                             "Pass/Fail,Bugs/Issues,Fix/Action,Tested By,Notes," +
                             "MutationFreq,DodgeSuccessRate";
                File.WriteAllText(_csvPath, header + Environment.NewLine, Encoding.UTF8);
            }
        }

        var sb = new StringBuilder();
        foreach (var r in _results) sb.AppendLine(r.ToCsv());
        File.AppendAllText(_csvPath, sb.ToString(), Encoding.UTF8);

        Debug.Log($"[TestLogger] Wrote {_results.Count} rows to {_csvPath}");
        _results.Clear();
    }

    // ----- UNITY LIFECYCLE --------------------------------------------------
    void Update()
    {
        // Frame-time accumulation for FPS
        float fps = 1f / Time.unscaledDeltaTime;
        _fpsSum   += fps;
        _fpsFrames++;
    }

    void OnApplicationQuit() => Flush();
}
