using System;
using System.Collections.Generic;
using System.Linq;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Models
{
    public class HumanStore
    {
        private Dictionary<Guid, Human> _database;

        public HumanStore(Dictionary<Guid, Human> database = null)
        {
            _database = database ?? new Dictionary<Guid, Human>();

        }

        public Human GetHuman(Guid id)
        {
            if (_database.TryGetValue(id, out var human))
            {
                return human;
            }

            return null;
        }

        public List<Human> GetAllHumans()
        {
            return _database.Values.ToList();
        }

        public void AddHuman(Human human)
        {
            if (_database.ContainsKey(human.Id))
            {
                throw new Exception("Repeated id");
            }

            _database.Add(human.Id, human);
        }

        public void UpdateHuman(Human human)
        {
            if (!_database.ContainsKey(human.Id))
            {
                throw new Exception("Id non existant");
            }

            _database[human.Id] = human;
        }

        public void RemoveHuman(Guid id)
        {
            if (!_database.ContainsKey(id))
            {
                throw new Exception("Id non existant");
            }

            _database.Remove(id);
        }
    }
}
