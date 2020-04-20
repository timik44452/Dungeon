using System;
using System.Collections.Generic;
using UnityEngine;
using Utility.GameTerminal;

public class Terminal : MonoBehaviour
{
    public GUISkin skin;
    public static TerminalEnviropment Enviropment
    {
        get => m_terminal.m_terminalEnviropment;
    }

    private static Terminal m_terminal;
    private TerminalParser m_terminalParser;
    private TerminalEnviropment m_terminalEnviropment = new TerminalEnviropment();

    private bool m_isOpened = false;
    private bool m_isInitialized = false;

    private List<TerminalMessage> m_messageHistory = new List<TerminalMessage>();
    private string m_terminalText = string.Empty;
    private float m_timeout = 0;

    private Rect m_terminalRect;

    private const char cursoreChar = '▌';

    private void Awake()
    {
        m_terminal = this;
    }

    private void Update()
    {
        if (m_timeout > 0)
        {
            m_timeout -= Time.deltaTime;

            return;
        }

        if (Input.GetKeyDown(KeyCode.Tilde) || Input.GetKeyDown(KeyCode.BackQuote))
        {
            m_isOpened = !m_isOpened;
        }
        else
        {
            if (m_isOpened)
            {
                if (Input.GetKey(KeyCode.Backspace) && m_terminalText.Length > 0)
                {
                    m_terminalText = m_terminalText.Substring(0, m_terminalText.Length - 1);
                    m_timeout = 2F * Time.deltaTime;
                }
                else
                {
                    m_terminalText += Input.inputString;
                }

                if (Input.GetKeyDown(KeyCode.Return) && m_terminalParser.TryParse(m_terminalText, out ParseResult result))
                {
                    Log(m_terminalText);
                    m_terminalText = string.Empty;

                    try
                    {
                        Invoke(result.Operand, result.Value);
                    }
                    catch (Exception e)
                    {
                        Error(e.Message);
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        if (!m_isOpened)
        {
            return;
        }

        if (!m_isInitialized)
        {
            Initialize();
            m_isInitialized = true;
        }

        var tempSkin = GUI.skin;

        GUI.skin = skin;

        m_terminalRect = GUI.Window(0, m_terminalRect, WindowFunction, "Terminal");

        GUI.skin = tempSkin;
    }

    private void WindowFunction(int ID)
    {
        float width = m_terminalRect.width + GUI.skin.window.contentOffset.x;
        float height = m_terminalRect.height + GUI.skin.window.contentOffset.y;

        float x = GUI.skin.window.contentOffset.x;
        float y = height;

        for (int i = m_messageHistory.Count; i >= 0; i--)
        {
            string messageText;
            
            if (i == m_messageHistory.Count)
            {
                messageText = $">{m_terminalText}{cursoreChar}";

                GUI.color = Color.white;
            }
            else
            {
                TerminalMessage message = m_messageHistory[i];

                switch(message.Type)
                {
                    case MessageType.Log: GUI.color = Color.white; break;
                    case MessageType.Error: GUI.color = Color.red; break;
                    case MessageType.Warning: GUI.color = Color.yellow; break;
                }

                messageText = message.Text;
            }

            y -= GUI.skin.label.CalcHeight(new GUIContent(messageText), width);

            GUI.Label(new Rect(x, y, width, height), $"{messageText}");

            if (y < 0)
            {
                m_messageHistory.RemoveRange(0, i);
                break;
            }
        }

        GUI.DragWindow();
    }

    private void Initialize()
    {
        m_terminalRect = GetTerminalRect();
        m_terminalParser = new TerminalParser();


        // Variables
        m_terminalEnviropment.AddVariable<int>("fontSize", value =>
        {
            skin.window.fontSize = value;
            skin.label.fontSize = value;
        });

        // Methods
        m_terminalEnviropment.AddMethod("cls", Clear);
        m_terminalEnviropment.AddMethod("help", Help);
    }

    private void Invoke(string operand, string value)
    {
        if(string.IsNullOrEmpty(value))
        {
            m_terminalEnviropment.Invoke(operand);
        }
        else
        {
            m_terminalEnviropment.ChangeValue(operand, value);
        }
    }

    private Rect GetTerminalRect()
    {
        return new Rect(0, 0, Screen.width * 0.45F, Screen.height * 0.45F);
    }

    private void Log(object message)
    {
        m_messageHistory.Add(new TerminalMessage(message.ToString(), MessageType.Log));
    }

    private void Warning(object message)
    {
        m_messageHistory.Add(new TerminalMessage(message.ToString(), MessageType.Warning));
    }

    private void Error(object message)
    {
        m_messageHistory.Add(new TerminalMessage(message.ToString(), MessageType.Error));
    }

    private void Clear()
    {
        m_messageHistory.Clear();
        m_terminalText = string.Empty;
    }

    private void Help()
    {
        throw new NotImplementedException();
    }
}
