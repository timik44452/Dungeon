using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TutorialSystem : MonoBehaviour
{
    public MessageBox messageBox;

    public List<TutorialScenario> scenarioPipeline;
    public TutorialScenario currentScenario
    {
        get => m_currentScenario;
        set
        {
            m_currentScenario = value;
            m_currentScenario?.Start();
            OnScenarioChanged();
        }
    }

    private TutorialScenario m_currentScenario;
    private bool currentScenarioEnded = false;
    private bool isPause = false;


    private void Start()
    {
        messageBox.OnButtonClick += OnButtonClick;
        currentScenario = scenarioPipeline[0];
    }

    private void Update()
    {
        if (currentScenario == null || isPause)
        {
            return;
        }

        if (currentScenarioEnded)
        {
            int currentIndex = scenarioPipeline.IndexOf(currentScenario);

            if (currentIndex + 1 < scenarioPipeline.Count)
            {
                currentScenario = scenarioPipeline[currentIndex + 1];
            }
        }

        if (currentScenario.Update())
        {
            Timeout(currentScenario.timeout);
            currentScenarioEnded = true;
        }
    }

    private void Timeout(float timeout)
    {
        StartCoroutine(TimeoutCoroutine(timeout));
        isPause = true;
    }

    private IEnumerator TimeoutCoroutine(float timeout)
    {
        yield return new WaitForSeconds(timeout);

        isPause = false;
    }

    private void OnScenarioChanged()
    {
        if(currentScenario == null)
        {
            return;
        }

        messageBox.Show(currentScenario.text, (int)MessageBox.ButtonType.OK);
        currentScenarioEnded = false;
    }

    private void OnButtonClick(Component sender, int buttons)
    {
        if (MessageBox.IsButton(buttons, MessageBox.ButtonType.OK))
            messageBox.Hide();
    }
}
