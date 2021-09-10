using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace MissCanApp
{
    #region SolutionProvider CLASS
    class SolutionProvider
    {
        // Instance fields
        private int CURRENT_ROOT_STATE = 0;
        private ArrayList searchAgenda = new ArrayList();

        
        public SolutionProvider() 
        {

        }

       
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



        private void addStateToAgenda(State newState) 
        {
            // Dont allow invalid states to be added to search agenda
            if (newState.InvalidState())
              return;

            //Valid state so add it to the agenda
            searchAgenda.Add(newState);
        }

     
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
