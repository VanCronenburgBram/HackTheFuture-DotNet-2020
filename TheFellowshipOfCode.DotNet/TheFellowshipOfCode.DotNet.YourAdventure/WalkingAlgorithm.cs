using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class WalkingAlgorithm
    {
        private readonly Random _random = new Random();
        //private TurnAction lastAction;
        private List<TurnAction> actionList = new List<TurnAction>();
        private double goingEastBias = 1.00;
        private double goingSouthBias = 1.00;
        private double goingNorthBias = 1.00;
        private double goingWestBias = 1.00;

        const double BIASDECREASE = 0.04;

        public WalkingAlgorithm()
        {
            actionList.Add(TurnAction.Pass);
            actionList.Add(TurnAction.Pass);
            actionList.Add(TurnAction.Pass);
        }

        public Task<Turn> DecideTurn(PlayTurnRequest request)
        {

            if (CanLoot(request))
            {
                return Loot();
            }

            if (CanBattle(request))
            {
                return Battle(request);
            }

            return Move(request);
        }

        public Task<Turn> Move(PlayTurnRequest request)
        {
            if (CanWalkEast(request) && _random.NextDouble() > (1 - goingEastBias))
            {
                return WalkEast();
            }

            if (CanWalkSouth(request) && _random.NextDouble() > (1 - goingSouthBias))
            {
                return WalkSouth();
            }

            if (CanWalkWest(request) && _random.NextDouble() > (1 - goingWestBias))
            {
                return WalkWest();
            }

            if (CanWalkNorth(request) && _random.NextDouble() > (1 - goingNorthBias))
            {
                return WalkNorth();
            }

            return Move(request);
            //return Task.FromResult(new Turn(request.PossibleActions[_random.Next(request.PossibleActions.Length)]));
        }

        public Task<Turn> Battle(PlayTurnRequest request)
        {
            if (request.IsCombat && request.PartyMember.CurrentHealthPoints <= 5)
            {
                return DrinkPotion();
            }
            return Attack();
        }

        //Actions

        public Task<Turn> WalkSouth()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkSouth))
            {
                actionList.Add(TurnAction.WalkSouth);
            }
            goingSouthBias -= BIASDECREASE;
            return Task.FromResult(new Turn(TurnAction.WalkSouth));
        }

        public Task<Turn> WalkEast()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkEast))
            {
                actionList.Add(TurnAction.WalkEast);
            }
            goingEastBias -= BIASDECREASE;
            return Task.FromResult(new Turn(TurnAction.WalkEast));
        }

        public Task<Turn> WalkNorth()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkNorth))
            {
                actionList.Add(TurnAction.WalkNorth);
            }
            goingNorthBias -= BIASDECREASE;
            return Task.FromResult(new Turn(TurnAction.WalkNorth));
        }

        public Task<Turn> WalkWest()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkWest))
            {
                actionList.Add(TurnAction.WalkWest);
            }
            goingWestBias -= BIASDECREASE;
            return Task.FromResult(new Turn(TurnAction.WalkWest));
        }

        public Task<Turn> Attack()
        {
            return Task.FromResult(new Turn(TurnAction.Attack));
        }

        public Task<Turn> Loot()
        {
            return Task.FromResult(new Turn(TurnAction.Loot));
        }

        public Task<Turn> DrinkPotion()
        {
            return Task.FromResult(new Turn(TurnAction.DrinkPotion));
        }

        //Checks

        public bool CanWalkSouth(PlayTurnRequest request)
        {
            if (actionList[actionList.Count - 1] == TurnAction.WalkNorth)
            {
                return false;
            }
            /*if (request.PossibleActions.Contains(TurnAction.WalkEast) && actionList[actionList.Count - 3] == TurnAction.WalkEast && actionList[actionList.Count - 2] == TurnAction.WalkNorth && actionList[actionList.Count - 1] == TurnAction.WalkWest)
            {
                return false;
            }
            if (request.PossibleActions.Contains(TurnAction.WalkWest) && actionList[actionList.Count - 3] == TurnAction.WalkWest && actionList[actionList.Count - 2] == TurnAction.WalkNorth && actionList[actionList.Count - 1] == TurnAction.WalkEast)
            {
                return false;
            }*/
            return request.PossibleActions.Contains(TurnAction.WalkSouth);
        }

        public bool CanWalkWest(PlayTurnRequest request)
        {
            if (actionList[actionList.Count - 1] == TurnAction.WalkEast)
            {
                return false;
            }
            return request.PossibleActions.Contains(TurnAction.WalkWest);
        }

        public bool CanWalkEast(PlayTurnRequest request)
        {
            if (actionList[actionList.Count - 1] == TurnAction.WalkWest)
            {
                return false;
            }
            return request.PossibleActions.Contains(TurnAction.WalkEast);
        }

        public bool CanWalkNorth(PlayTurnRequest request)
        {
            if (actionList[actionList.Count - 1] == TurnAction.WalkSouth)
            {
                return false;
            }
            return request.PossibleActions.Contains(TurnAction.WalkNorth);
        }

        public bool CanAttack(PlayTurnRequest request)
        {
            return request.PossibleActions.Contains(TurnAction.Attack);
        }

        public bool CanLoot(PlayTurnRequest request)
        {
            return request.PossibleActions.Contains(TurnAction.Loot);
        }

        public bool CanDrinkPotion(PlayTurnRequest request)
        {
            return request.PossibleActions.Contains(TurnAction.DrinkPotion);
        }

        public bool CanBattle(PlayTurnRequest request)
        {
            if (CanAttack(request) && CanDrinkPotion(request))
            {
                return true;
            }
            return false;
        }
    }
}
