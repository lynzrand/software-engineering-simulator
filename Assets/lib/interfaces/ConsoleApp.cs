using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sesim.Library.Interfaces
{
    public interface IInGameConsole
    {
        void write(ICanvasElement element);
        void showList(IList<ICanvasElement> list);
    }


    /// <summary>
    /// An application making use of the in-game console
    /// </summary>
    public interface IConsoleApp
    {
        /// <summary>
        /// The name of this console application
        /// </summary>
        /// <value></value>
        string Name { get; }

        IInGameConsole Console { set; }

        int handle(string input);
    }
}