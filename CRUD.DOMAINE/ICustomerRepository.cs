using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.DOMAINE
{
    public interface ICustomerRepository
    {
        List<Customer> GetAll();

        int Insert(Customer obj);
        bool Update(Customer obj);
        bool Delete(int Id);
    }
}
