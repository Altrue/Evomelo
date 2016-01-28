using Evomelo.Genetique;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

namespace Evomelo
{
    public partial class MainWindow : Window
    {

        private MediaPlayer _mplayer;
        private Boolean _isPlaying;
        private Population _population;
        public MainWindow()
        {
            InitializeComponent();
            GD.date_last_save = DateTime.Now;
            //génération de la première population
            _population = new Population();
            createMidiFiles();

            // Initialisation graphique
            Width = GD.WINDOW_WIDTH;
            Height = GD.WINDOW_HEIGHT;
            ResizeMode = ResizeMode.CanMinimize;
            WindowStyle = WindowStyle.None;
            Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x16, 0x16, 0x16));
            Icon = new BitmapImage(new Uri("pack://application:,,,/ressources/icon.ico"));
            MouseLeftButtonDown += MainWindow_MouseDown;

            // Main Canvas Initialization
            AddChild(GD.MainCanvas);
            GD.MainCanvas.Width = GD.WINDOW_WIDTH;
            GD.MainCanvas.Height = GD.WINDOW_HEIGHT;
            GD.MainCanvas.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            GD.MainCanvas.VerticalAlignment = System.Windows.VerticalAlignment.Top;

            // Head Text
            Rectangle rect_head = new Rectangle();
            rect_head.Width = 390;
            rect_head.Height = 35;
            rect_head.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/titre_evomelo.png")));
            Canvas.SetTop(rect_head, (5));
            Canvas.SetLeft(rect_head, (30));
            GD.MainCanvas.Children.Add(rect_head);

            // Exit button
            GD.bt_Exit.Width = 20;
            GD.bt_Exit.Height = 20;
            GD.bt_Exit.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/icon_cross.png")));
            GD.bt_Exit.Name = "icon_cross";
            GD.bt_Exit.MouseLeftButtonDown += ExitButton_MouseDown;
            GD.bt_Exit.MouseEnter += Button_MouseEnter;
            GD.bt_Exit.MouseLeave += Button_MouseLeave;
            Canvas.SetTop(GD.bt_Exit, (10));
            Canvas.SetRight(GD.bt_Exit, (10));
            GD.MainCanvas.Children.Add(GD.bt_Exit);

            // Cadres
            for (int n=0;n<11;n++)
            {
                if (n<10) {
                    GD.borderIndividus[n] = new Border();
                    GD.borderIndividus[n].Width = GD.WINDOW_WIDTH - 60;
                    GD.borderIndividus[n].Height = 35;
                    GD.borderIndividus[n].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x11, 0x11, 0x11));
                    GD.borderIndividus[n].BorderThickness = new Thickness(0);
                    GD.borderIndividus[n].BorderBrush = new SolidColorBrush(Color.FromArgb(0xFF, 0x22, 0x22, 0x22));
                    Canvas.SetTop(GD.borderIndividus[n], (50 + (n * 50)));
                    Canvas.SetLeft(GD.borderIndividus[n], (30));
                    GD.MainCanvas.Children.Add(GD.borderIndividus[n]);

                    // Draw Preview
                    drawPreview(n);

                    // Play button
                    GD.rectPlayIndividus[n] = new Rectangle();
                    GD.rectPlayIndividus[n].Width = 27;
                    GD.rectPlayIndividus[n].Height = 27;
                    GD.rectPlayIndividus[n].Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/icon_play.png")));
                    GD.rectPlayIndividus[n].MouseLeftButtonDown += PlayButton_MouseDown;
                    GD.rectPlayIndividus[n].MouseEnter += Button_MouseEnter;
                    GD.rectPlayIndividus[n].MouseLeave += Button_MouseLeave;
                    GD.rectPlayIndividus[n].Name = "icon_play";
                    Canvas.SetTop(GD.rectPlayIndividus[n], (54 + (n * 50)));
                    Canvas.SetRight(GD.rectPlayIndividus[n], (35));
                    GD.MainCanvas.Children.Add(GD.rectPlayIndividus[n]);

                    // Star buttons
                    for (int n2 = 0; n2 < 5; n2++)
                    {
                        // Star button
                        GD.rectStars[(n * 5 + n2)] = new Rectangle();
                        GD.rectStars[(n * 5 + n2)].Width = 32;
                        GD.rectStars[(n * 5 + n2)].Height = 24;
                        GD.rectStars[(n * 5 + n2)].Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/icon_star_empty.png")));
                        GD.rectStars[(n * 5 + n2)].MouseLeftButtonDown += StarButton_MouseDown;
                        GD.rectStars[(n * 5 + n2)].MouseEnter += Button_MouseEnter;
                        GD.rectStars[(n * 5 + n2)].MouseLeave += Button_MouseLeave;
                        GD.rectStars[(n * 5 + n2)].Name = "icon_star_empty";
                        Canvas.SetTop(GD.rectStars[(n * 5 + n2)], (55 + (n * 50)));
                        Canvas.SetLeft(GD.rectStars[(n * 5 + n2)], (220 + (32 * n2)));
                        GD.MainCanvas.Children.Add(GD.rectStars[(n * 5 + n2)]);
                    }

                    // Save button
                    GD.rectSaveIndividus[n] = new Rectangle();
                    GD.rectSaveIndividus[n].Width = 25;
                    GD.rectSaveIndividus[n].Height = 25;
                    GD.rectSaveIndividus[n].Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/icon_save.png")));
                    GD.rectSaveIndividus[n].MouseEnter += Button_MouseEnter;
                    GD.rectSaveIndividus[n].MouseLeave += Button_MouseLeave;
                    GD.rectSaveIndividus[n].MouseLeftButtonDown += SaveButton_MouseDown;
                    GD.rectSaveIndividus[n].Name = "icon_save";
                    Canvas.SetTop(GD.rectSaveIndividus[n], (55 + (n * 50)));
                    Canvas.SetRight(GD.rectSaveIndividus[n], (246));
                    GD.MainCanvas.Children.Add(GD.rectSaveIndividus[n]);

                    // Here : Generation of each track
                    // Generation of preview graph
                }
                else
                {
                    GD.bt_Generation.Width = 390;
                    GD.bt_Generation.Height = 35;
                    GD.bt_Generation.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/button_generation_err.png")));
                    GD.bt_Generation.MouseEnter += Button_MouseEnter;
                    GD.bt_Generation.MouseLeave += Button_MouseLeave;
                    GD.bt_Generation.Name = "button_generation_err";
                    GD.bt_Generation.MouseDown += generationButton_MouseDown;
                    Canvas.SetTop(GD.bt_Generation, (50 + (n * 50)));
                    Canvas.SetLeft(GD.bt_Generation, (30));
                    GD.MainCanvas.Children.Add(GD.bt_Generation);
                }
                
            }

            // Initialisation du lecteur
            _mplayer = new MediaPlayer();
            _mplayer.MediaEnded += mplayer_MediaEnded;
            _isPlaying = false;

            // On s'abonne à la fermeture du programme pour pouvoir nettoyer le répertoire et les fichiers midi
            this.Closed += MainWindow_Closed;
        }

        // On efface les fichiers .mid que l'on avait créé à la fin du programme
        void MainWindow_Closed(object sender, EventArgs e)
        {
            deleteMidiFiles();
        }

        // Lancé lorsque le fichier a fini sa lecture, pour le fermer proprement
        void mplayer_MediaEnded(object sender, EventArgs e)
        {
            stopMusic();
            Console.WriteLine("Fini !");
        }

        // Clic sur le bouton : on lance la création d'un fichier et on le joue
        private void PlayButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (_isPlaying)
            {
                stopMusic();
            }
            else
            {
                int individuId = Array.IndexOf(GD.rectPlayIndividus, sender as Rectangle);
                playMusic("./fichier/Fichier" + individuId.ToString() + ".mid");
                GD.borderIndividus[individuId].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x22, 0x22, 0x22));
                GD.canvasPreview[individuId].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x22, 0x22, 0x22));
            }
        }

        // Clic sur le bouton : on génère une nouvelle génération
        private void generationButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool fit = true;
            for(int i = 0; i < _population.nbIndividu; i++)
            {
                if(_population.individus[i].fitness == 0)
                {
                    fit = false;
                    break;
                }
            }
            //si tout les individus ont une fitness
            if(fit == true)
            {

                Console.WriteLine("--- DEBUT NEW GENERATION ---");

                var starUri = "pack://application:,,,/ressources/icon_star_empty.png";

                // Erase the rating
                for (int s = 0; s < 50; s++)
                {
                    GD.rectStars[s].Fill = new ImageBrush(new BitmapImage(new Uri(starUri)));
                    GD.rectStars[s].Name = "icon_star_empty";
                }

                //individus est désormais une nouvelle population d'individus
                _population.newGeneration();
                deleteMidiFiles();
                createMidiFiles();

                for (int n = 0; n < _population.nbIndividu; n++)
                {
                    drawPreview(n);
                    // Restoring save button visibility
                    GD.rectSaveIndividus[n].Visibility = Visibility.Visible;
                }

                GD.bt_Generation.Name = "button_generation_err";
                GD.bt_Generation.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/button_generation_err.png")));
                GD.nb_rated = 0;
                Console.WriteLine("--- FIN NEW GENERATION ---");
            }
            else
            {
                Console.WriteLine("Clic enregistré mais il ne fait rien !");
            }
        }

        // DragMove
        public void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // Exit
        public void ExitButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        // These methods enable any rectangle bound to these events, to have an hover effect
        // as long as an appropriate background image exists with "_2.png" added the end of the name.

        // Any Rectangle acting as button MouseEnter
        public void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            var logoUri = "pack://application:,,,/ressources/" + (sender as Rectangle).Name+"_2.png";
            (sender as Rectangle).Fill = new ImageBrush(new BitmapImage(new Uri(logoUri)));

            if ((sender as Rectangle).Name.Length > 13)
            {
                if ((sender as Rectangle).Name.Substring(0,14) == "icon_star_empt" || (sender as Rectangle).Name.Substring(0, 14) == "icon_star_full")
                {
                    int starId = Array.IndexOf(GD.rectStars, sender as Rectangle);
                    int lineNumber = Math.Abs(starId / 5) + 1;
                    
                    for (int n = lineNumber * 5 - 5; n<starId; n++)
                    {
                        var logoUri2 = "pack://application:,,,/ressources/" + GD.rectStars[n].Name + "_2.png";
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(logoUri2)));
                    }
                }
            }
        }

        // Any Rectangle acting as button MouseLeave
        public void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            var logoUri = "pack://application:,,,/ressources/" + (sender as Rectangle).Name + ".png";
            (sender as Rectangle).Fill = new ImageBrush(new BitmapImage(new Uri(logoUri)));

            if ((sender as Rectangle).Name.Length > 13)
            {
                if ((sender as Rectangle).Name.Substring(0, 14) == "icon_star_empt" || (sender as Rectangle).Name.Substring(0, 14) == "icon_star_full")
                {
                    int starId = Array.IndexOf(GD.rectStars, sender as Rectangle);
                    int lineNumber = Math.Abs(starId / 5) + 1;

                    for (int n = lineNumber * 5 - 5; n < starId; n++)
                    {
                        var logoUri2 = "pack://application:,,,/ressources/" + GD.rectStars[n].Name + ".png";
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(logoUri2)));
                    }
                }
            }
        }

        // Clic sur le bouton : on note ou dénote
        private void StarButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            toggleStars(Array.IndexOf(GD.rectStars, sender as Rectangle));
        }

        public void toggleStars(int starId)
        {
            int lineNumber = Math.Abs(starId / 5);

            var starUri = "pack://application:,,,/ressources/icon_star_empty.png";
            var starUri2 = "pack://application:,,,/ressources/icon_star_full.png";
            int fitnessDemande = starId - (lineNumber * 5 - 1);

            Console.WriteLine("Cliqué sur etoile numero " + starId);


            // Data Side of Things
            if (_population.individus[lineNumber].fitness == fitnessDemande)
            {
                _population.individus[lineNumber].fitness = 0; // Disabling rating
                GD.nb_rated--;
            }
            else
            {
                _population.individus[lineNumber].fitness = fitnessDemande; // Notes de 1 à 5. 0 = pas assigné
                GD.nb_rated++;
            }

            for (int n = 0; n < _population.nbIndividu; n++)
            {
                Console.WriteLine("Individu " + n + ", noté " + _population.individus[n].fitness);
            }
            Console.WriteLine("---");
            //Console.WriteLine("Individu " + (lineNumber) + ", noté " + _population.individus[lineNumber].fitness);

            // Visual Side of Things
            if (GD.rectStars[starId].Name == "icon_star_empty")
            {
                for (int n = lineNumber * 5; n < lineNumber * 5 + 5; n++)
                {
                    if (n <= starId)
                    {
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(starUri2)));
                        GD.rectStars[n].Name = "icon_star_full";
                    }
                    else
                    {
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(starUri)));
                        GD.rectStars[n].Name = "icon_star_empty";
                    }
                }
            }
            else if (GD.rectStars[starId].Name == "icon_star_full")
            {
                for (int n = lineNumber * 5; n < lineNumber * 5 + 5; n++)
                {
                    // Do we want to disable the rating? If we clicked on the 5th star (that is full), or if the next star is empty.
                    if ((lineNumber * 5) == starId)
                    {
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(starUri)));
                        GD.rectStars[n].Name = "icon_star_empty";
                    }
                    else if (GD.rectStars[starId + 1].Name == "icon_star_empty") {
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(starUri)));
                        GD.rectStars[n].Name = "icon_star_empty";
                    }
                    // Or just lower the rating?
                    else
                    {
                        if (n <= starId)
                        {
                            GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(starUri2)));
                            GD.rectStars[n].Name = "icon_star_full";
                        }
                        else
                        {
                            GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(starUri)));
                            GD.rectStars[n].Name = "icon_star_empty";
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("OUPS! CA N'AURAIT PAS DU ARRIVER.");
            }

            // Modification possible de l'apparence du boutton generation

            if (GD.nb_rated == _population.nbIndividu)
            {
                GD.bt_Generation.Name = "button_generation";
                GD.bt_Generation.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/button_generation.png")));
            }
            else
            {
                GD.bt_Generation.Name = "button_generation_err";
                GD.bt_Generation.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/button_generation_err.png")));
            }
        }

        // WORK IN PROGRESS
        public void drawPreview(int _individuId)
        {
            Individu unIndividu = _population.individus[_individuId];

            int _marginLeft = 32;
            int _marginTop = 52 + (_individuId * 50);

            // Determines color 1 - 128

            //int doubleInstrument = (unIndividu.instrument - 1) * 2; // 0 --> 254
            int doubleInstrument = _individuId * 28; // pour tester la palette de couleurs

            // RVB default values
            int valueRed = 254;
            int valueGreen = 254;
            int valueBlue = 254;

            // 0 --> 42
            if (doubleInstrument < 43)
            {
                valueRed = 254;
                valueGreen = (doubleInstrument * 6) + 2;
                valueBlue = 0;
            }
            // 43 --> 85
            else if (doubleInstrument < 86)
            {
                valueRed = 254 - (((doubleInstrument - 43) * 6) + 2);
                valueGreen = 254;
                valueBlue = 0;
            }
            // 86 --> 127
            else if (doubleInstrument < 128)
            {
                valueRed = 0;
                valueGreen = 254;
                valueBlue = ((doubleInstrument - 85) * 6) + 2;
            }
            // 128 --> 170
            else if (doubleInstrument < 171)
            {
                valueRed = 0;
                valueGreen = 254 - (((doubleInstrument - 128) * 6) + 2);
                valueBlue = 254;
            }
            // 171 --> 212
            else if (doubleInstrument < 213)
            {
                valueRed = ((doubleInstrument - 170) * 6) + 2;
                valueGreen = 0;
                valueBlue = 254;
            }
            // 213 --> 254
            else {
                valueRed = 254;
                valueGreen = 0;
                valueBlue = 254 - (((doubleInstrument - 212) * 6) + 2);
            }
            

            string valueRedHex = valueRed.ToString("X");
            if (valueRedHex.Length == 1)
            {
                valueRedHex = "0" + valueRedHex;
            }
            string valueGreenHex = valueGreen.ToString("X");
            if (valueGreenHex.Length == 1)
            {
                valueGreenHex = "0" + valueGreenHex;
            }
            string valueBlueHex = valueBlue.ToString("X");
            if (valueBlueHex.Length == 1)
            {
                valueBlueHex = "0" + valueBlueHex;
            }

            var converter = new System.Windows.Media.BrushConverter();

            GD.MainCanvas.Children.Remove(GD.canvasPreview[_individuId]);
            GD.canvasPreview[_individuId] = new Canvas();
            
            GD.canvasPreview[_individuId].Width = 129;
            GD.canvasPreview[_individuId].Height = 31;
            GD.canvasPreview[_individuId].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x11, 0x11, 0x11));

            Canvas.SetTop(GD.canvasPreview[_individuId], (_marginTop));
            Canvas.SetLeft(GD.canvasPreview[_individuId], (_marginLeft));
            GD.MainCanvas.Children.Add(GD.canvasPreview[_individuId]);

            for (int n=0; n < 16; n++)
            {
                GD.rectPreviewArray[_individuId][n] = new Rectangle();

                GD.rectPreviewArray[_individuId][n].Width = 7;
                GD.rectPreviewArray[_individuId][n].Height = 31 * ((double)unIndividu.notes[n]/127); // HEIGHT DEPENDANT ON NOTE TONE
                GD.rectPreviewArray[_individuId][n].Fill = (Brush)converter.ConvertFromString("#FF" + valueRedHex + valueGreenHex + valueBlueHex);

                Canvas.SetBottom(GD.rectPreviewArray[_individuId][n], (0));
                Canvas.SetLeft(GD.rectPreviewArray[_individuId][n], (1 + n*8));
                GD.canvasPreview[_individuId].Children.Add(GD.rectPreviewArray[_individuId][n]);
            }
        }

        private void createMidiFiles()
        {
            stopMusic();

            if (!Directory.Exists("./fichier"))
            {
                Directory.CreateDirectory("./fichier");
            }

            for (int i=0;i < _population.nbIndividu; i++)
            {
                MIDISong song = new MIDISong();
                song.AddTrack("Piste " + i.ToString());
                song.SetTimeSignature(0, 4, 4);
                song.SetTempo(0, 150);
                song.SetChannelInstrument(0, 0, _population.individus[i].instrument);
                for(int x = 0; x < _population.nbNotes; x++)
                {
                    song.AddNote(0, 0, _population.individus[i].notes[x], 12);
                }

                // on prépare le flux de sortie
                MemoryStream ms = new MemoryStream();
                song.Save(ms);
                ms.Seek(0, SeekOrigin.Begin);
                byte[] src = ms.GetBuffer();
                byte[] dst = new byte[src.Length];
                for (int y = 0; y < src.Length; y++)
                {
                    dst[y] = src[y];
                }
                ms.Close();
                // et on écrit le fichier
                string strFileName = "./fichier/Fichier" + i.ToString() + ".mid";
                FileStream objWriter = File.Create(strFileName);
                objWriter.Write(dst, 0, dst.Length);
                objWriter.Close();
                objWriter.Dispose();
                objWriter = null;
            }
        }

        private void deleteMidiFiles()
        {
            stopMusic();
            var files = Directory.EnumerateFiles("./fichier/", "Fichier*.mid");
            foreach (string file in files)
            {
                File.Delete(file);
            }
            Directory.Delete("./fichier");
        }

        private void playMusic(string strFileName)
        {
            _mplayer.Open(new Uri(strFileName, UriKind.Relative));
            Console.WriteLine(strFileName);
            Console.WriteLine(new Uri(strFileName, UriKind.Relative));
            _isPlaying = true;
            for (int n = 0; n < _population.nbIndividu; n++)
            {
                GD.rectPlayIndividus[n].Name = "icon_pause";
                GD.rectPlayIndividus[n].Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/icon_pause.png")));
            }
            _mplayer.Play();
            Console.WriteLine("Lancé !");
        }

        private void stopMusic()
        {
            if (_isPlaying)
            {
                _mplayer.Stop();
                _mplayer.Close();
                _isPlaying = false;
                for (int n = 0; n < _population.nbIndividu; n++)
                {
                    GD.rectPlayIndividus[n].Name = "icon_play";
                    GD.rectPlayIndividus[n].Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/icon_play.png")));
                    GD.borderIndividus[n].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x11, 0x11, 0x11));
                    GD.canvasPreview[n].Background = new SolidColorBrush(Color.FromArgb(0xFF, 0x11, 0x11, 0x11));
                }
            }
        }

        // Clic sur le bouton : on lance la sauvegarde
        private void SaveButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DateTime date = DateTime.Now;

            // Let's wait at least two seconds before saving another file.
            if ((date - GD.date_last_save).TotalSeconds > 1)
            {
                if (!Directory.Exists("./sauvegarde"))
                {
                    Directory.CreateDirectory("./sauvegarde");
                }
                int individuId = Array.IndexOf(GD.rectSaveIndividus, sender as Rectangle);
                string dateString = date.ToString().Replace("/", "");
                dateString = dateString.Replace(" ", "");
                dateString = dateString.Replace(":", "");
                File.Move("./fichier/Fichier" + individuId.ToString() + ".mid", "./sauvegarde/sauvegarde" + dateString + ".mid");

                // Hide the button for now. Not needed anymore.
                GD.rectSaveIndividus[individuId].Visibility = Visibility.Hidden;

                // Save the last save datetime to prevent crashes.
                GD.date_last_save = date;
            }
        }
    }
}
