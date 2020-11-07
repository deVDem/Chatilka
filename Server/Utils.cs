using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    public static class Utils
    {
        public static ushort getUniqueId(List<Server> list)
        {
            Random random = new Random();
            ushort id = (ushort)random.Next(ushort.MinValue, ushort.MaxValue);

            foreach (Server item in list)
            {
                if (id == item.Id)
                {
                    id++;
                }
            }
            return id;
        }
        public static ushort getUniqueId(List<Room> rooms)
        {
            Random random = new Random();
            int id = random.Next();

            foreach (Room item in rooms)
            {
                if (id == item.Id)
                {
                    return getUniqueId(rooms);
                }
            }
            return (ushort)id;
        }
        public static ushort getUniqueId(List<Client> clients)
        {
            Random random = new Random();
            int id = random.Next();

            foreach (Client item in clients)
            {
                if (id == item.Id)
                {
                    return getUniqueId(clients);
                }
            }
            return (ushort)id;
        }

    }
}
