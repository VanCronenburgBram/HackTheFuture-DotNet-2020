using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class DungeonAlgorithm
    {
        private readonly Random _random = new Random();
        private List<TurnAction> actionList = new List<TurnAction>();

        private double goingEastBias = 1.00;
        private double goingSouthBias = 1.00;
        private double goingNorthBias = 1.00;
        private double goingWestBias = 1.00;

        const double BIASDECREASE = 0.08;
        const double BIASINCREASE = 0.02;

        private bool BiasSet = false;

        private int potionAmount = 0;

        public DungeonAlgorithm()
        {
            actionList.Add(TurnAction.Pass);
        }

        public Task<Turn> DecideTurn(PlayTurnRequest request)
        {
            if (!BiasSet)
            {
                SetBias(request);
            }

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
        }

        public Task<Turn> Battle(PlayTurnRequest request)
        {
            if (request.IsCombat && request.PartyMember.CurrentHealthPoints <= 20 && 0 < request.PartyMember.CurrentHealthPoints)
            {
                //return DrinkPotion();
                return Attack();
            }

            return Attack();
        }

        public void SetBias(PlayTurnRequest request)
        {
            int startX = request.PartyLocation.X;
            int startY = request.PartyLocation.Y;
            BiasSet = true;

            if (startX < 5)
                goingEastBias = 1.25;
            else
                goingWestBias = 1.25;
            
            if (startY < 5)
                goingSouthBias = 1.25;
            else
                goingNorthBias = 1.25;
        }

        //Actions

        public Task<Turn> WalkSouth()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkSouth))
            {
                actionList.Add(TurnAction.WalkSouth);
            }

            if (goingSouthBias > 0.4)
            {
                goingSouthBias -= BIASDECREASE;
            }

            goingNorthBias += BIASINCREASE;
            return Task.FromResult(new Turn(TurnAction.WalkSouth));
        }

        public Task<Turn> WalkEast()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkEast))
            {
                actionList.Add(TurnAction.WalkEast);
            }

            if (goingEastBias > 0.4)
            {
                goingEastBias -= BIASDECREASE;
            }

            goingWestBias += BIASINCREASE;
            return Task.FromResult(new Turn(TurnAction.WalkEast));
        }

        public Task<Turn> WalkNorth()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkNorth))
            {
                actionList.Add(TurnAction.WalkNorth);
            }

            if (goingNorthBias > 0.4)
            {
                goingNorthBias -= BIASDECREASE;
            }

            goingSouthBias += BIASINCREASE;
            return Task.FromResult(new Turn(TurnAction.WalkNorth));
        }

        public Task<Turn> WalkWest()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.WalkWest))
            {
                actionList.Add(TurnAction.WalkWest);
            }

            if (goingWestBias > 0.4)
            {
                goingWestBias -= BIASDECREASE;
            }

            goingEastBias += BIASINCREASE;
            return Task.FromResult(new Turn(TurnAction.WalkWest));
        }

        public Task<Turn> Attack()
        {
            return Task.FromResult(new Turn(TurnAction.Attack));
        }

        public Task<Turn> Loot()
        {
            if (!(actionList[actionList.Count - 1] == TurnAction.Attack))
            {
                potionAmount++;
            }

            return Task.FromResult(new Turn(TurnAction.Loot));
        }

        public Task<Turn> DrinkPotion()
        {
            actionList.Add(TurnAction.DrinkPotion);
            potionAmount--;
            return Task.FromResult(new Turn(TurnAction.DrinkPotion));
        }

        //Checks

        public bool CanWalkSouth(PlayTurnRequest request)
        {
            if (actionList[actionList.Count - 1] == TurnAction.WalkNorth)
            {
                return false;
            }

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
            if (actionList[actionList.Count - 1] == TurnAction.DrinkPotion || potionAmount <= 0)
            {
                return false;
            }

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
