﻿using System;
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

        public int[] notes
        {
            get
            {
                return this._notes;
            }
            set
            {
                this._notes = value;
            }
        }

        //la note utilisateurs
        public int fitness
        {
            get
            {
                return this._fitness;
            }
            set
            {
                this._fitness = value;
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
