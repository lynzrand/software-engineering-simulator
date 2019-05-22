using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public static class FocusSystem
{
    public static Stack<GameObject> focusStack = new Stack<GameObject>();

    public static void Push(GameObject obj) => focusStack.Push(obj);

    public static GameObject Pop() => focusStack.Pop();

    public static GameObject Active { get => focusStack.Peek(); }


}


public abstract class FocusableElement : MonoBehaviour
{
    public abstract void Update();
}
