using System;
using UnityEngine;

public class ContextMenuItem
{
    public string Text { get; set; }
    public Sprite Icon { get; set; }
    /// <summary>
    /// Sub menu items
    /// </summary>
    public ContextMenuItem[] menuItems { get; set; }

    private Action callback;

    public ContextMenuItem(string Text, Action callback = null, params ContextMenuItem[] menuItems)
    {
        this.Text = Text;
        this.menuItems = menuItems;

        this.callback = callback;
    }

    public ContextMenuItem(string Text, Sprite Icon, Action callback = null, params ContextMenuItem[] menuItems)
    {
        this.Text = Text;
        this.Icon = Icon;
        this.menuItems = menuItems;

        this.callback = callback;
    }

    public void Invoke()
    {
        callback?.Invoke();
    }
}
