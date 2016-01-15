using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evomelo.Genetique
{
    class Individu
    {
        private int[] _notes;
        private int _fitness;
        private int _instrument;

        //initialisation
        public Individu(int nbNotes)
        {
            _notes = new int[nbNotes];
            _instrument = new Random().Next(1, 128);
            _fitness = 0;
        }

        public int instrument
        {
            get
            {
                return _instrument;
            }
            set
            {
                _instrument = value;
            }
        }

        public int[] notes
        {
            get
            {
                return _notes;
            }
            set
            {
                _notes = value;
            }
        }

        //la note utilisateurs
        public int fitness
        {
            get
            {
                return _fitness;
            }
            set
            {
                _fitness = value;
            }
        }

        //génère une piste de façon aléatoire
        public void generateRandomNotes()
        {
            for(int i = 0; i < _notes.Length; i++)
            {
                _notes[i] = new Random().Next(0, 127);
            }
        }
    }
}
