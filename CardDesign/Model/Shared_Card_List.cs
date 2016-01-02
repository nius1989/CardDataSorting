using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardDesign
{
    class Shared_Card_List
    {
        static List<Card> shardCards = new List<Card>();

        public static List<Card> ShardCards
        {
            get
            {
                return shardCards;
            }

            set
            {
                shardCards = value;
            }
        }

        public static void UpdateSharedCards() {
            shardCards.Clear();
            lock (Card_List.CardList) {
                foreach (News_Card nc in Card_List.CardList)
                {
                    if (STATICS.COLLABORATIVE_ZOON.Contains((int)nc.CurrentPosition.X, (int)nc.CurrentPosition.Y))
                    {
                        shardCards.Add(nc);
                    }
                }
            }
        }
    }
}
