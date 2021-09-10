using System;
using System.Collections.Generic;
using System.Text;

namespace MissCanApp
{
    #region State CLASS
    // State - Is a representation of a node with the Search Agenda. Each State
    // holds various bits of data which help to model a specific state.
    // These data elements are described below.
    // 
    // Number of Missionaries within the current state
    // 
    // Number of Cannibals within the current state
    // 
    // Side that the boat is on. False is one side, True is the other side
    // 
    // Name of the State. This will be expanded upon for each successor state
    // so that a full StateName can be printed, to show the full search path
    // that this state used to get to where it is now
    // 
    // PrevState is the previous state, this is the parent state from which this
    // state was created. This will be null for the ROOT state, as it does not
    // have a previous state
    // State Level is the level that this states in on in the search tree
    class State
    {
        // Instance fields
        public int nMiss, nCan;
        public bool Side;
        private int NumOfEachAtStart = 3;
        private String Name;
        private State PrevState;
        private int stateLevel = 0;

        //State Constructer (1), Use this for the root state 
        //Simply creates a new State with the name, number of Missionaries,
        //number of Cannibals and side to match the values supplied by
        //the formal parameters. In this case there will be no PrevState as this
        //is the 1st state
        //
        //param : Name is the name for this State
        //param : nMiss the number on Missionaries for this state
        //param : nCan the number on Cannibals for this state
        //param : Side the side of the river that the boat is now on
        //param : stateLevel the level this state is on, 0=root / 1st layer, 1 = 2nd layer, 2 = 3rd layer
        public State(String Name, int nMiss, int nCan, bool Side, int stateLevel) : this(Name, nMiss, nCan, Side, null, stateLevel)
        {
            //Call the overloaded constructor with the formal parameters
            //provided, but make PrevState=null, as the 1st State does not
            //have a PrevState to point to
            //this(Name, nMiss, nCan, Side, null, stateLevel);
        }

        //State Constructer (2), Use this to create States based upon other States
        //Simply creates a new State with the name, number of Missionaries,
        //number of Cannibals,side and PrevState to match the values supplied by
        //the formal parameters. In this case PrevState will be a pointer to this
        //nodes parent node
        //
        //param : Name is the name for this State
        //
        //param : nMiss the number on Missionaries for this state
        //
        //param : nCan the number on Cannibals for this state
        //
        //param : Side the side of the river that the boat is now on
        //
        //param : PrevState a pointer to this State's PrevState (parent)
        //
        //param stateLevel the level this state is on, 0=root / 1st layer, 1 = 2nd layer, 2 = 3rd layer
        public State(String Name, int nMiss, int nCan, bool Side,
                 State PrevState, int stateLevel)
        {
            //Assign parameters to local instance fields
            this.Name = Name;
            this.nMiss = nMiss;
            this.nCan = nCan;
            this.Side = Side;
            this.PrevState = PrevState;
            this.stateLevel = stateLevel;
        }

        //Simply returns this States stateLevel
        //
        //return : int representing this States stateLevel
        public int getStateLevel() 
        {
            return this.stateLevel;
        }

        //Simply returns this States name
        //
        //return : String representing this States name
        public String getStateName()
        {
            return this.Name;
        }

        //Prints a full search path of how this state came to be at the
        //goal state. Makes use of the PrevState to recursively call
        //the Print method until there is no PrevState. This way each
        //State only prints its own data
        public void Print() 
        {

            //Check that there is a PrevState, Root node will not have one, so
            //that is when all states from Goal - to start have been printed
            if (PrevState != null) {
                //Use recursion to allow Previous state to print its own data paths
                PrevState.Print();
            }

            //Use the conditional operator to figure out what side we are on
            String WhatSide = Side ? "  BOAT RIGHT->" : "<-BOAT LEFT   ";

            //Print the current state.
            Console.WriteLine(nMiss + "M/" + nCan + "C " + WhatSide + " " +
                         (NumOfEachAtStart - nMiss) + "M/" +
                         (NumOfEachAtStart - nCan) + "C");

        }

        //Simply returns true is 2 states are the same
        //
        //param : StateToCheck is the State to check against
        //
        //return : True if the number of Missionaries, number of Cannibals and
        //Side are the same for this State and the StateToCheck against. Otherwise
        //false is returned
        public bool Equals(State StateToCheck) 
        {
            return (nMiss == StateToCheck.nMiss &&
                nCan == StateToCheck.nCan &&
                Side == StateToCheck.Side);
        }

        //Simply returns true if this State is a valid state
        //This method makes use of the command line argument that
        //specfies whether there should be more Cannibals than Missionaries,
        //OR whether there should be more Missionaries than Cannibals. Either
        //way it uses this global flag to work out if the state is valid for the
        //given choice that the user made when running this application.
        //
        //return : True only if the number of PersonType1 does not outnumber
        //the PersonType2 in this state. The allocation of whom PersonType1 and
        //PersonType2 are, is governed by the command line argument to this
        //application.
        public bool InvalidState() 
        {
            int PersonType1 = 0;
            int PersonType2 = 0;

            //Check to see if the user requested that there be more Cannibals than
            //Missionaries. If this is the case set PersonType variables for this
            //situation
            if (Program.CanOutnumberMiss)
            {
                PersonType1 = nCan;
                PersonType2 = nMiss;
            }
            //Otherwise set the siutation to be that there be more Missionaries than
            //Cannibals
            else 
            {
                PersonType1 = nMiss;
                PersonType2 = nCan;
            }
            // Check for < 0, which could actually happen unless it is checked for here
            if (nMiss < 0 || nCan < 0 ||
                nMiss > NumOfEachAtStart ||
                nCan > NumOfEachAtStart)
                return true;
            //Do PersonType2 outnumbers PersonType1(only worry when there is at least
            //one PersonType1) one Side1
            if (PersonType1 < PersonType2 && PersonType1 > 0)
                return true;
            //Do PersonType2 outnumbers PersonType1(only worry when there is at least
            //one PersonType1) one Side2
            if ( (NumOfEachAtStart - PersonType1 <
                  NumOfEachAtStart - PersonType2) &&
                (NumOfEachAtStart - PersonType1 > 0))
                return true;
            //At this point the State must be valid
            return false;
        }

    } //End of State class
    #endregion
}
