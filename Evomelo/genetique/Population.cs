using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evomelo.Genetique
{
    class Population
    {
        private double _tauxCross = 0.6;
        private int _nbIndividu = 10;
        private int _nbNotes = 16;
        private double _tauxMut;
        private double _tauxSurvie = 2.5;
        private Random _rnd;

        private Individu[] _individus;

        public Population()
        {
            _individus = new Individu[_nbIndividu];
            _tauxMut = 1 / _nbNotes;
            _rnd = new Random();

            for(int i = 0; i < _nbIndividu; i++)
            {
                _individus[i] = new Individu(_nbNotes, _rnd.Next(1,129));
                _individus[i].generateRandomNotes(_rnd);
            }
        }

        //pour accéder à la liste depuis l'interface
        public Individu[] individus
        {
            get
            {
                return _individus;
            }
            set
            {
                _individus = value;
            }
        }

        //fonction de cross-over
        private Individu cross(Individu i1, Individu i2)
        {
            Individu newI = new Individu(_nbNotes);
            int[] tabNote = new int[_nbNotes];
            int nbSeq = (int)(_nbNotes * _rnd.NextDouble());

            for (int i = 0; i < _nbNotes; i++)
            {
                if (i < nbSeq)
                {
                    tabNote[i] = i1.notes[i];
                }
                else
                {
                    tabNote[i] = i2.notes[i];
                }
            }

            newI.notes = tabNote;

            if(i1.fitness >= i2.fitness)
            {
                newI.instrument = i1.instrument;
            }
            else
            {
                newI.instrument = i2.instrument;
            }

            return newI;
        }

        //fonction de mutation
        private Individu mutation(Individu i1)
        {
            for (int i = 0; i < _nbNotes; i++)
            {
                if (_rnd.NextDouble() <= _tauxMut)
                {
                    i1.notes[i] = _rnd.Next(0,127);
                }
            }

            return i1;
        }

        //sélectionne un individu
        private Individu selection()
        {
            int p1 = _rnd.Next(0, 10);
            int p2 = _rnd.Next(0, 10);
            Individu individu = new Individu(_nbNotes);

            if(_individus[p1].fitness <= _individus[p2].fitness)
            {
                individu.notes = _individus[p2].notes;
                individu.instrument = _individus[p2].instrument;
            }
            else
            {
                individu.notes = _individus[p1].notes;
                individu.instrument = _individus[p1].instrument;
            }

            return individu;
        }

        //génère une nouvelle génération
        public void newGeneration()
        {
            Individu[] newPop = new Individu[_nbIndividu];
            int i = 0;

            Individu eliteIndividu = selectElite();
            if (eliteIndividu != null)
            {
                Individu individu = new Individu(_nbNotes);
                individu.instrument = eliteIndividu.instrument;
                individu.notes = eliteIndividu.notes;
                newPop[i] = individu;
                i++;
            }

            for (; i < _nbIndividu; i++)
            {
                Individu individu = selection();

                //si cross
                if (_tauxCross > _rnd.NextDouble())
                {
                    Individu individu2 = selection();
                    individu = cross(individu, individu2);
                }

                individu = mutation(individu);

                individu.fitness = 0;
                newPop[i] = individu;
            }

            _individus = newPop;
        }

        //principe de survie si la moyenne des note est >= _tauxSurvie
        private Individu selectElite()
        {
            Individu elite = new Individu(_nbNotes);
            int sFitness = 0;

            for (int i = 0; i < _nbIndividu; i++)
            {
                sFitness = sFitness + _individus[i].fitness;
                if(_individus[i].fitness > elite.fitness)
                {
                    elite.instrument = _individus[i].instrument;
                    elite.notes = _individus[i].notes;
                }
            }

            if(sFitness/_nbIndividu < _tauxSurvie)
            {
                elite = null;
            }

            return elite;
        }
    }
}
