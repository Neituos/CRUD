using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.DOMAINE
{
    public class Customer
    {
        private int Id;
        private string Name;
        private string Pseudo;
        private bool Gender;
        private string BirthDay;
        private string Email;
        private string Adress;
        private string ImageUrl;

        public int Id1 { get => Id; set => Id = value; }
        public string Name1 { get => Name; set => Name = value; }
        public string Pseudo1 { get => Pseudo; set => Pseudo = value; }
        public bool Gender1 { get => Gender; set => Gender = value; }
        public string BirthDay1 { get => BirthDay; set => BirthDay = value; }
        public string Email1 { get => Email; set => Email = value; }
        public string Adress1 { get => Adress; set => Adress = value; }
        public string ImageUrl1 { get => ImageUrl; set => ImageUrl = value; }
    }
}
