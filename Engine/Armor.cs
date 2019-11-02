using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine {
    public class Armor : Item {
        public int Defense { get; set; }

        public Armor(int id, string name, string namePlural, int defense, int price) : base(id, name,namePlural, price) {
            Defense = defense;
        }
    }
}
