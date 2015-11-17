using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace CardDesign
{
    /// <summary>
    /// A controler to highight sorting groups
    /// </summary>
    class Sorting_Group_Controler
    {
        Controlers control;
        /// <summary>
        /// Store and manage the sorting groups
        /// </summary>
        /// <param name="gestureControler">The one created in the MainWindow</param>
        public Sorting_Group_Controler(Controlers control) {
            this.control = control;           
        }

        internal void DeleteGroup(Menu_Sort_Box box)
        {
            Card[] cards = Group_List.CardGroups[box].ToArray();
            foreach (Card c in cards) {
                c.RemoveFromGroup(box);
            }
            Group_List.Remove(box);
            control.MainWindow.MenuLayer.RemoveGroupButton(box);
        }

        public bool ContainCard(Menu_Sort_Box key, Card c)
        {
            return Group_List.CardGroups.ContainsKey(key)&&
                Group_List.CardGroups[key].Contains(c);
        }
        public void GroupCard(Menu_Sort_Box key, Card c) {
            Group_List.Add(key, c);
            c.SortToGroup(key);
        }
        public void RemoveCard(Menu_Sort_Box key, Card c) {
            Group_List.RemoveCard(key, c);
            c.RemoveFromGroup(key);
        }
        public void HighlightCards(Menu_Sort_Box key) {
            List<Card> cards = Group_List.CardGroups[key];
            foreach (Card c in cards) {
                c.Hightlight();
            }
        }
        public void DehighlightCards(Menu_Sort_Box key)
        {
            List<Card> cards = Group_List.CardGroups[key];
            foreach (Card c in cards)
            {
                c.Dehightlight();
            }
        }

        public void CreateGroup(String owner, String id, String text, String textbrief, Point position) {
            Menu_Sort_Box box = new Menu_Sort_Box(control.MainWindow.MenuLayer, owner, id, text, textbrief);
            box.IsManipulationEnabled = true;
            box.IsHitTestVisible = true;
            Matrix matrix = new Matrix(1, 0, 0, 1, position.X, position.Y);
            box.RenderTransform = new MatrixTransform(matrix);
            box.SetStartPosition(matrix.OffsetX + box.Width / 2, matrix.OffsetY + box.Height / 2);
            Group_List.CreateGroup(box);
            control.MainWindow.MenuLayer.AddGroupButton(box);
        }
    }
}
