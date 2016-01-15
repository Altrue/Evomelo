using Evomelo.Genetique;
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
        private string _strFileName;
        private int _nbFile = 0;
        private Population _population;
        public Random testRand = new Random(); // A BOUGER
        public MainWindow()
        {
            InitializeComponent();

            //génération de la première population
            _population = new Population();

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

                    // TODO - PREVIEW
                    drawPreview(32, 52 + (n * 50), n);

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
                    GD.rectSaveIndividus[n].MouseLeftButtonDown += PlayButton_MouseDown;
                    GD.rectSaveIndividus[n].MouseEnter += Button_MouseEnter;
                    GD.rectSaveIndividus[n].MouseLeave += Button_MouseLeave;
                    //GD.rectSaveIndividus[n].MouseLeftButtonDown += SaveButton_MouseDown; // <- TODO
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
                    GD.bt_Generation.Fill = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/ressources/button_generation.png")));
                    GD.bt_Generation.MouseEnter += Button_MouseEnter;
                    GD.bt_Generation.MouseLeave += Button_MouseLeave;
                    GD.bt_Generation.Name = "button_generation";
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
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (_isPlaying)
            {
                _mplayer.Stop();
                _mplayer.Close();
                _isPlaying = false;
            }
            var files = Directory.EnumerateFiles("./", "Fichier*.mid");
            foreach (string file in files)
            {
                File.Delete(file);
            }

        }

        // Lancé lorsque le fichier a fini sa lecture, pour le fermer proprement
        void mplayer_MediaEnded(object sender, EventArgs e)
        {
            _mplayer.Stop();
            _mplayer.Close();
            _isPlaying = false;
        }

        // Clic sur le bouton : on lance la création d'un fichier et on le joue
        private void PlayButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateAndPlayMusic();
        }

        // Clic sur le bouton : on génère une nouvelle génération
        private void generationButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            bool fit = true;
            for(int i = 0; i < _population.individus.Length; i++)
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
                //individus est désormais une nouvelle population d'individus
                _population.newGeneration();

                //TODO : actualiser l'affichage avec les nouveaux individus
            }
            else
            {
                //TODO : afficher un message annonçant que toutes les "musique" doivent être notée
            }
        }

        // Méthode principale
        private void CreateAndPlayMusic()
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (_isPlaying)
            {
                _mplayer.Stop();
                _mplayer.Close();
                _isPlaying = false;
            }
            // Générateur aléatoire
            Random rand = new Random();

            // 1) Créer le fichier MIDI
            // a. Créer un fichier et une piste audio ainsi que les informations de tempo
            MIDISong song = new MIDISong();
            song.AddTrack("Piste1");
            song.SetTimeSignature(0, 4, 4);
            song.SetTempo(0, 150);

            // b. Choisir un instrument entre 1 et 128 
            // Liste complète ici : http://fr.wikipedia.org/wiki/General_MIDI
            int instrument = rand.Next(1, 129);
            song.SetChannelInstrument(0, 0, instrument);

            // c. Ajouter des notes
            // Chaque note est comprise entre 0 et 127 (12 correspond au type de note, fixe ici à des 1/4)
            // L'équivalence avec les notes / octaves est disponible ici : https://andymurkin.files.wordpress.com/2012/01/midi-int-midi-note-no-chart.jpg
            // Ici 16 notes aléatoire entre 16 et 96 (pour éviter certaines notes trop aigues ou trop graves)
            for (int i = 0; i < 16; i++)
            {
                int note = rand.Next(24, 96);
                song.AddNote(0, 0, note, 12);
            }

            // d. Enregistrer le fichier .mid (lisible dans un lecteur externe par exemple)
            // on prépare le flux de sortie
            MemoryStream ms = new MemoryStream();
            song.Save(ms);
            ms.Seek(0, SeekOrigin.Begin);
            byte[] src = ms.GetBuffer();
            byte[] dst = new byte[src.Length];
            for (int i = 0; i < src.Length; i++)
            {
                dst[i] = src[i];
            }
            ms.Close();
            // et on écrit le fichier
            _strFileName = "Fichier" + _nbFile + ".mid";
            FileStream objWriter = File.Create(_strFileName);
            objWriter.Write(dst, 0, dst.Length);
            objWriter.Close();
            objWriter.Dispose();
            objWriter = null;

            // 2) Jouer un fichier MIDI
            _mplayer.Open(new Uri(_strFileName, UriKind.Relative));
            _nbFile++;
            _isPlaying = true;
            _mplayer.Play();
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
            int lineNumber = Math.Abs(starId / 5) + 1;
            var starUri = "pack://application:,,,/ressources/icon_star_empty.png";
            var starUri2 = "pack://application:,,,/ressources/icon_star_full.png";

            if (GD.rectStars[starId].Name == "icon_star_empty")
            {
                for (int n = lineNumber * 5 - 5; n < lineNumber * 5; n++)
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
                for (int n = lineNumber * 5 - 5; n < lineNumber * 5; n++)
                {
                    // Do we want to disable the rating? If we clicked on the 5th star (that is full), or if the next star is empty.
                    if ((lineNumber * 5 - 1) == starId)
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
        }

        // WORK IN PROGRESS
        public void drawPreview(int _marginLeft, int _marginTop, int _individuId)
        {
            Individu unIndividu = _population.individus[_individuId];

            // Determines color 1 - 128

            int doubleInstrument = unIndividu.instrument * 2;
            //int doubleInstrument = _individuId * 28; // pour tester la palette de couleurs

            int valueRed = Math.Max(255 - doubleInstrument * 2, 0);
            int valueGreen = (127 - Math.Abs(doubleInstrument - 127)) * 2;
            int valueBlue = Math.Max(doubleInstrument - 127, 0) * 2;

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
    }
}
