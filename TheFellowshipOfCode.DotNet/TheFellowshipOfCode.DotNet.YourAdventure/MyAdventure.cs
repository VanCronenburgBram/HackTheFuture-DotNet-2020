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
        private DungeonAlgorithm dungeonAlgorithm = new DungeonAlgorithm();

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
            return dungeonAlgorithm.DecideTurn(request);
        }
    }
}