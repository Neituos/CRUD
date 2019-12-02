using CRUD.DOMAINE;
using System;
using System.Collections.Generic;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CRUD.PERSISTANCE
{
    public class CustomerRepository : ICustomerRepository
    {
        public bool Delete(int Id)
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnexionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                int result = db.Execute("delete from Customers where Id = @Id", new { Id =  Id}, commandType: CommandType.Text);
                return result != 0;
            }
        }

        public List<Customer> GetAll()
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnexionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                return db.Query<Customer>("select *from Customers", commandType: CommandType.Text).ToList();
              
            }
        }

        public int Insert(Customer obj)
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnexionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
              
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@Id", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    p.AddDynamicParams(new { Name = obj.Name1, Pseudo = obj.Pseudo1, Gender = obj.Gender1, BirtDay = obj.BirthDay1, Email = obj.Email1, Adress = obj.Adress1, ImageUrl = obj.ImageUrl1 });
                    db.Execute("sp_Customers_Insert", p, commandType: CommandType.StoredProcedure);
                    return p.Get<int>("@Id");
                
                
            }
        }

        public bool Update(Customer obj)
        {
            using (IDbConnection db = new SqlConnection(Helper.ConnexionString))
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();
                
                    int result = db.Execute("sp_Customers_IUpdate", new { Id = obj.Id1, Name = obj.Name1, Pseudo = obj.Pseudo1, Gender = obj.Gender1, BirtDay = obj.BirthDay1, Email = obj.Email1, Adress = obj.Adress1, ImageUrl = obj.ImageUrl1 }, commandType: CommandType.StoredProcedure);

                return result != 0;
                
            }
        }
    }
}
