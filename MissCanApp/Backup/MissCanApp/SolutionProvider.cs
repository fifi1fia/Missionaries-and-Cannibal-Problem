using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace MissCanApp
{
    #region SolutionProvider CLASS

    //SolutionProvider - Provides solutions to the "Cannibals and Missionaries"
    //Search problem.
    //
    //HOW THE SEARCH IS PERFORMED
    //
    //This class uses a breadth 1st search to search for possible solutions.
    //A ArrayList is used to store the agenda. The agenda consists of a list of State
    //classes. The root State will be the 1st item in the agenda. When the root
    //is taken from the agenda, it is compared to the goal state, if it is the goal
    //state it will be stored within a SolutionsFound ArrayList. However the 1st node
    //in the search agenda it not likely to be the goal so different successor
    //states will be generated from the root and these will be added to the agenda.
    //Each of these states will then be taken from the agenda and compared to the
    //goal, if they are equal to the goal they will be stored within a
    //SolutionsFound ArrayList. However if they are not the goal state, they too will
    //be expanded to create successor  states, which will then be added to the agenda.
    //
    //The solutions may be found at various levels within the search tree. This application
    //should return the optimal solustions. To achieve this the 1st solution has its level
    //within the search tree recorded. Then when new solutions are found they are compared
    //to this level, if they are less than or the same they too are stored as valid optimal
    //solutions. Only when the solutions found are found at a higher level does the search know
    //that it has found all the optimal solutions. When this occurs the search is ended, and the
    //optimal solutions returned.
    class SolutionProvider
    {
        // Instance fields
        private int CURRENT_ROOT_STATE = 0;
        private ArrayList searchAgenda = new ArrayList();

        //SolutionProvider Constructor
        //Simply creates a new SolutionProvider object
        public SolutionProvider() 
        {

        }

        //Creats a new State based on a parent state. The formal parameters
        //supplied dictate what changes are to be applied to the parent
        //state to form the new child state
        //
        //@param : StateName represents the new state name
        //
        //param : parent is the parent State that this State should be generated from.
        //Ie the new State will be a child of this parameter
        //
        //param : nMiss number of Missionaries in the boat for the new state
        //
        //param : nCan number of Cannibals in the boat for the new state
        private void addStateToAgenda(String StateName,State parent,
                                    int nMiss, int nCan) 
        {

            // BoatDirection holds either 1 or -1, depending on the side.
            int BoatDirection = parent.Side ? 1 : -1;

            //Get the name of the parent state and add append the new state
            //suffix to it
            String newStateName = parent.getStateName() + StateName;

            //Create a new state based on the parent state parameter that was
            //supplied when calling this method
            State newState = new State(newStateName,
                                  parent.nMiss + nMiss * BoatDirection,
                                  parent.nCan + nCan * BoatDirection,
                                  !parent.Side,
                                  parent, parent.getStateLevel() + 1);
            //Try and add the newly generated State to the search agenda
            addStateToAgenda(newState);
        }


        //Tries to add the NewState parameter provided to the search agenda.
        //If the state parameter provided is not a valid state, the state will not
        //be added to the agenda. The State class deals with checking for a ValidState
        //
        //param : NewState the state to try and add it to the search agenda
        private void addStateToAgenda(State newState) 
        {
            // Dont allow invalid states to be added to search agenda
            if (newState.InvalidState())
              return;

            //Valid state so add it to the agenda
            searchAgenda.Add(newState);
        }

        //This is the main method that does most of the work. It carries out
        //various tasks. These are described below
        //
        //The 1st thing that is done is the internal SolutionsFound collection is
        //initialized and the StartState (From the formal parameter) is added to the
        //Search Agenda
        //
        //Then a loop is enetered that loops through the entire agenda taking off
        //the 0 element when 1st entering the loop. For readability I have defined
        //the 0 elemenet to be an int called CURRENT_ROOT_STATE. This variable is
        //a simply int that is set to 0 when the SolutionProvider class is constructed.
        //
        //When the CURRENT_ROOT_STATE element is removed from the Search Agenda it is
        //cast to a State then compared to the GoalState (From the formal parameter).
        //If it equals the goal state (Dealt with by the State class) and is the 1st
        //solution found the level of the state is recorded, and the state is stored
        //within the SolutionsFound collection. If this is not the 1st solution found
        //the state level of this new solution is compared to the recorded state level
        //of the 1st solution. If this solution is less than or equal to the recorded
        //optimal level, then it too may be added to the SolutionsFound collection.
        //However if it is not the goal state, which is will not be for the 1st State
        //(ROOT) then new succeessor nodes will need to be created based upon this parent
        //node. The generateSucessors method deals with this.
        //
        //param : StartState the StartState, with all Cannibals/Missionaries
        //on one side of the river
        //
        //param : EndState the StartState, with all Cannibals/Missionaries
        //on the opposite side of the river
        //
        //return : ArrayList that holds all the solutions found
        public ArrayList getSolutionStates(State StartState, State EndState) 
        {
            //initialise local fields
            int optimalSolfoundAtLevel = 0;
            bool allOptimalSolutionsFound = false;
            bool foundFirstSolution = false;

            //Initialise SolutionsFound collection
            ArrayList Solutions = new ArrayList();
            //Add StartState to the Search Agenda
            addStateToAgenda(StartState);

            //Loop through search agenda until we have found all the optimal solutions
            while (searchAgenda.Count > 0 && !allOptimalSolutionsFound) {
              //Get the current root state from the Search Agenda
              State CurState = (State)searchAgenda[CURRENT_ROOT_STATE];
              //Remove the current root state from the Search Agenda, is has been
              //dealt with now
              searchAgenda.RemoveAt(CURRENT_ROOT_STATE);

              //Is the current root state the Goal State
              if (CurState.Equals(EndState)) {
                //Has the 1st solution been found
                if (foundFirstSolution) {
                    //YES the 1st solution was found so lets compare this states level to the existing level
                    //from when the 1st solution was found
                    if (CurState.getStateLevel() <= optimalSolfoundAtLevel) {
                        //If it is, store the state in the SolutionsFound collection
                        Solutions.Add(CurState);
                    }
                    else {
                        //since the current state level is greater than the optimalSolfoundAtLevel
                        //this solution must be more costly, we must have already found all the optimal solutions.
                        //so need to break out of loop, so set break condition
                        allOptimalSolutionsFound =true;
                    }
                }
                else {
                    //At this point this must be the 1st solution found, so store it and record its level
                    //in the optimalSolfoundAtLevel, so that this can be used to compare against for the next solutions found.
                    //Also prevent this logic froming running again by setting the foundFirstSolution to true.
                    foundFirstSolution=true;
                    optimalSolfoundAtLevel  = CurState.getStateLevel();
                    Solutions.Add(CurState);
                }
              }
              else {
              //The current root state is NOT Goal State, so create
              //sucessor states based on it
              generateSucessors(CurState);
              }

            }
            return Solutions;
        }

        //This method simply calls the addStateToAgenda method
        //passing in all required derivations of the CurState state
        //to be new sucessor nodes
        //
        //param : CurState the current state to use to create sucessor nodes from
        private void generateSucessors(State CurState) {

            //if this method has been called the CurState, was NOT the goal,
            //so need to create new sucessor states based on it. So try and create
            //some new states.
            int nCan, nMiss =0;
            int stateName =1;
            //loop through all possible combinations
            for (int i = 0; i <= Program.boatsize; i++)
            {
                for (int j = 0; j <= Program.boatsize; j++)
                {
                    //prune the search tree, getting rid of invalid states
                    if (i==0 && j ==0)
                        continue;
                    if (i + j > Program.boatsize)
                        break;
                    //good state found, so add to agenda
                    nMiss = i;
                    nCan = j;
                    addStateToAgenda("_" + stateName++, CurState, nMiss, nCan);
                }
            }
        }

    }  //End of SolutionProvider class
    #endregion
}
