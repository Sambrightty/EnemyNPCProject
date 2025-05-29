using UnityEngine;

public class TestController : MonoBehaviour
{
    int testCounter = 1;

    void Start()
    {
        StartNextTest();
    }

    void Update()
    {
        // Press F1 to end the current test and start the next one
        if (Input.GetKeyDown(KeyCode.F1))
        {
            TestLogger.Instance.EndTest(
                observedOutcome: "Test completed #" + testCounter,
                passed: true,
                bugs: "",
                fix: "",
                notes: "Notes for test " + testCounter,
                reactionMs: Random.Range(400f, 800f),
                mutationFreq: Random.Range(0.1f, 0.3f),
                dodgeSuccess: Random.Range(0.7f, 0.95f)
            );

            testCounter++;
            StartNextTest();
        }
    }

    void StartNextTest()
    {
        TestLogger.Instance.StartTest(
            id: "TC-" + testCounter.ToString("D3"),
            scenario: "Enemy Test #" + testCounter,
            expectedOutcome: "Enemy should behave correctly in scenario " + testCounter
        );
    }
}
