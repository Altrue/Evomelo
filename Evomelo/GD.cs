﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Evomelo
{
    // ===============================
    // Global Data class
    // ===============================
    public static class GD
    {
        // ========== DEBUG MODE CONTROL ==========
        public const bool DEBUGMODE = false;
        // ========================================

        // Constants & Variables
        // - Constants
        public const int WINDOW_WIDTH = 450;
        public const int WINDOW_HEIGHT = 600;
        // - Variables
        public static int coreNumber = 0;

        // UI Elements
        // - Canvas
        public static Canvas GraphCanvasCPU = new Canvas();
        public static Canvas MainCanvas = new Canvas();
        // - Brushes

        // - Buttons
        public static Button bt_Play = new Button();
        public static Button bt_Generation = new Button();

        // Data Storage
        // - Dictionaries
        public static Dictionary<Int16, List<int>> CoreData = new Dictionary<Int16, List<int>>();
        public static Dictionary<Int16, Polyline> CPUPolyLines = new Dictionary<Int16, Polyline>();
        // - Arrays
        public static PerformanceCounter[] pcArrayCPU = new PerformanceCounter[17];
        public static TextBlock[] tbArrayCPU = new TextBlock[32];
        public static Rectangle[] rectCPU = new Rectangle[32];
        public static Border[] borderIndividus = new Border[10];
    }
}
