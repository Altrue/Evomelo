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

        MediaPlayer mplayer;
        Boolean isPlaying;
        string strFileName;
        int nbFile = 0;

        public MainWindow()
        {
            InitializeComponent();

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
                        //GD.rectStars[(n * 5 + n2)].MouseLeftButtonDown += StarButton_MouseDown; // <- TODO
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
                    //GD.bt_Generation.Click += PlayButton_Click;//        <- TODO
                    Canvas.SetTop(GD.bt_Generation, (50 + (n * 50)));
                    Canvas.SetLeft(GD.bt_Generation, (30));
                    GD.MainCanvas.Children.Add(GD.bt_Generation);
                }
                
            }

            // Initialisation du lecteur
            mplayer = new MediaPlayer();
            mplayer.MediaEnded += mplayer_MediaEnded;
            isPlaying = false;

            // On s'abonne à la fermeture du programme pour pouvoir nettoyer le répertoire et les fichiers midi
            this.Closed += MainWindow_Closed;
        }

        // On efface les fichiers .mid que l'on avait créé à la fin du programme
        void MainWindow_Closed(object sender, EventArgs e)
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying)
            {
                mplayer.Stop();
                mplayer.Close();
                isPlaying = false;
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
            mplayer.Stop();
            mplayer.Close();
            isPlaying = false;
        }

        // Clic sur le bouton : on lance la création d'un fichier et on le joue
        private void PlayButton_MouseDown(object sender, MouseButtonEventArgs e)
        {
            CreateAndPlayMusic();
        }

        // Méthode principale
        private void CreateAndPlayMusic()
        {
            // s'il y a un fichier en cours de lecture on l'arrête 
            if (isPlaying)
            {
                mplayer.Stop();
                mplayer.Close();
                isPlaying = false;
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
            strFileName = "Fichier" + nbFile + ".mid";
            FileStream objWriter = File.Create(strFileName);
            objWriter.Write(dst, 0, dst.Length);
            objWriter.Close();
            objWriter.Dispose();
            objWriter = null;

            // 2) Jouer un fichier MIDI
            mplayer.Open(new Uri(strFileName, UriKind.Relative));
            nbFile++;
            isPlaying = true;
            mplayer.Play();
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
                    
                    for (int n=(lineNumber)*5 - 5;n<starId; n++)
                    {
                        var logoUri2 = "pack://application:,,,/ressources/" + (sender as Rectangle).Name + "_2.png";
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(logoUri)));
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

                    for (int n = (lineNumber) * 5 - 5; n < starId; n++)
                    {
                        var logoUri2 = "pack://application:,,,/ressources/" + (sender as Rectangle).Name + ".png";
                        GD.rectStars[n].Fill = new ImageBrush(new BitmapImage(new Uri(logoUri)));
                    }
                }
            }
        }
    }
}
