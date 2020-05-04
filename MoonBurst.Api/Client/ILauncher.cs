﻿using System.Security.Principal;
using System.Windows;
using System.Windows.Controls;

namespace MoonBurst
{
    public interface ILauncher
    {
        void Launch();
        IMainViewHost Host { get; }
    }

    public interface IMainViewHost
    {
        void Start();
    }

    public interface IMainView
    {

    }
}