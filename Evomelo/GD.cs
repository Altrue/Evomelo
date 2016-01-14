using System;
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
        public static Rectangle bt_Exit = new Rectangle();
        public static Rectangle bt_Generation = new Rectangle();

        // Data Storage
        // - Dictionaries
        public static Dictionary<Int16, List<int>> CoreData = new Dictionary<Int16, List<int>>();
        public static Dictionary<Int16, Polyline> CPUPolyLines = new Dictionary<Int16, Polyline>();
        // - Arrays
        public static PerformanceCounter[] pcArrayCPU = new PerformanceCounter[17];
        public static TextBlock[] tbArrayCPU = new TextBlock[32];
        public static Rectangle[] rectPlayIndividus = new Rectangle[10];
        public static Border[] borderIndividus = new Border[10];
        public static Rectangle[] rectStars = new Rectangle[50];
        public static Rectangle[] rectSaveIndividus = new Rectangle[10];
        public static Canvas[] canvasPreview = new Canvas[10];

        // C'est dégeuullaaassseee
        public static Rectangle[][] rectPreviewArray = new Rectangle [10][]{
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16],
            new Rectangle[16]
        };

    }
}
