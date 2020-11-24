using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HTF2020.Contracts;
using HTF2020.Contracts.Enums;
using HTF2020.Contracts.Models;
using HTF2020.Contracts.Models.Adventurers;
using HTF2020.Contracts.Requests;

namespace TheFellowshipOfCode.DotNet.YourAdventure
{
    public class MyAdventure : IAdventure
    {
        private readonly Random _random = new Random();

        private WalkingAlgorithm walkingAlgorithm = new WalkingAlgorithm();

        public Task<Party> CreateParty(CreatePartyRequest request)
        {
            var party = new Party
            {
                Name = "NullReferenceException",
                Members = new List<PartyMember>()
            };

            
                party.Members.Add(new Fighter()
                {
                    Id = 1,
                    Name = "Brambo",
                    Constitution = 11,
                    Strength = 12,
                    Intelligence = 11
                });
                party.Members.Add(new Fighter()
                {
                    Id = 2,
                    Name = "Chrissy",
                    Constitution = 11,
                    Strength = 12,
                    Intelligence = 11
                });
            

            return Task.FromResult(party);
        }

        public Task<Turn> PlayTurn(PlayTurnRequest request)
        {
            
            return walkingAlgorithm.DecideTurn(request);

            /*Task<Turn> PlayToEnd()
            {
                return Task.FromResult(request.PossibleActions.Contains(TurnAction.WalkSouth) ? new Turn(TurnAction.WalkSouth) : new Turn(request.PossibleActions[_random.Next(request.PossibleActions.Length)]));
            }

            Task<Turn> Strategic()
            {
                const double goingEastBias = 0.35;
                const double goingSouthBias = 0.25;
                if (request.PossibleActions.Contains(TurnAction.Loot))
                {
                    return Task.FromResult(new Turn(TurnAction.Loot));
                }

                if (request.PossibleActions.Contains(TurnAction.Attack))
                {
                    return Task.FromResult(new Turn(TurnAction.Attack));
                }

                if (request.PossibleActions.Contains(TurnAction.WalkEast) && _random.NextDouble() > (1 - goingEastBias))
                {
                    return Task.FromResult(new Turn(TurnAction.WalkEast));
                }

                if (request.PossibleActions.Contains(TurnAction.WalkSouth) && _random.NextDouble() > (1 - goingSouthBias))
                {
                    return Task.FromResult(new Turn(TurnAction.WalkSouth));
                }

                return Task.FromResult(new Turn(request.PossibleActions[_random.Next(request.PossibleActions.Length)]));
            }*/
        }
    }
}