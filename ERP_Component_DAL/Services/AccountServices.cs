using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ERP_Component_DAL.Models;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Data;

namespace ERP_Component_DAL.Services
{
    public class AccountServices
    {
        private readonly IConfiguration configuration;
        SqlConnection connection;
        private readonly string _connectionString;


        public AccountServices(IConfiguration config)
        {
            this.configuration = config;
            _connectionString = configuration.GetConnectionString("DefaultConnectionString") ?? throw new ArgumentNullException(nameof(_connectionString));
            connection = new SqlConnection(_connectionString); // Initialize connection
        }

     
        public bool createexpense(Expense e)
        {
            var date =e.entrydate;
            var date2 = date.ToString("yyyy-MM-dd");
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"insert into expense([entrydate],[project],[particular],[debited],[credited],[reference],[narration]) values('{date2}','{e.project}','{e.particular}','{e.debit}','{e.credit}','{e.reference}','{e.narration}');";
  
                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }
        public bool creategeneral(Expense e)
        {
            var date = e.entrydate;
            var date2 = date.ToString("yyyy-MM-dd");
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"insert into Generalentry([entrydate],[project],[particular],[debited],[credited],[reference],[narration]) values('{date2}','{e.project}','{e.particular}','{e.debit}','{e.credit}','{e.reference}','{e.narration}');";

                cmd.Connection = connection;
                connection.Open();
                cmd.ExecuteNonQuery();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public bool addaccountdetails(Expense e)
        {
            
            try
            {
                String ConnectionString = configuration.GetConnectionString("DefaultConnectionString");
                connection = new SqlConnection(ConnectionString);
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;


                cmd.CommandText = $"insert into Accounttype([Accountcode],[AccountName],[Accounttype]) output inserted.AccountId values('{e.accountcode}','{e.accountname}','{e.groupname}')";

                cmd.Connection = connection;

                connection.Open();
                int accountid = (int)cmd.ExecuteScalar();
                connection.Close();


                SqlCommand cmd1 = new SqlCommand();
                cmd1.CommandType = System.Data.CommandType.Text;

                cmd1.CommandText = $"insert into Accountdetails(AccountId,[AccountNo],[AccountHolder],[Bank],[Branch],[IFSC],[balance])values( @accountid,'{e.accountnumber}','{e.Accountholdername}','{e.bank}','{e.branch}','{e.ifsc}','{e.balance}')";
                cmd1.Connection = connection;
                cmd1.Parameters.AddWithValue("@accountid", accountid);
                connection.Open();
                cmd1.ExecuteScalar();
                connection.Close();



                return true;

            }
            catch (Exception)
            {
                throw;
            }
     
        }



        public List<Expense> getexpense()
        {
            List<Expense> cp = new List<Expense>(); // data to be returned
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"select expenseid,entrydate,project,particular,debited,credited,reference,narration from expense";
                    connection.Open();
                    cmd.Connection = connection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cp.Add(new Expense
                            {
                                entrydate = reader["entrydate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["entrydate"]) : DateOnly.MinValue,
                                project = reader["project"] != DBNull.Value ? (string)reader["project"] : string.Empty,
                                particular = reader["particular"] != DBNull.Value ? (string)reader["particular"] : string.Empty,
                                debit = reader["debited"] != DBNull.Value ? (string)reader["debited"] : string.Empty,
                                credit = reader["credited"] != DBNull.Value ? (string)reader["credited"] : string.Empty,
                                reference = reader["reference"] != DBNull.Value ? (string)reader["reference"] : string.Empty,
                                narration = reader["narration"] != DBNull.Value ? (string)reader["narration"] : string.Empty,
                                expenseid = reader["expenseid"] != DBNull.Value ? (int)reader["expenseid"] : 0,
                            });
                        }
                    }
                    connection.Close();

                }
                return cp;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public List<Expense> showacountgroup()
        {
            List<Expense> cp = new List<Expense>(); // data to be returned
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"select Accountcode,AccountName,Accounttype from Accounttype ";
                    connection.Open();
                    cmd.Connection = connection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cp.Add(new Expense
                            {
                                accountcode = reader["Accountcode"] != DBNull.Value ? (string)reader["Accountcode"] : string.Empty,
                                accountname = reader["AccountName"] != DBNull.Value ? (string)reader["AccountName"] : string.Empty,
                                groupname = reader["Accounttype"] != DBNull.Value ? (string)reader["Accounttype"] : string.Empty,

                            });
                        }
                    }
                    connection.Close();

                }
                return cp;
            }
            catch (Exception)
            {
                throw;
            }


        }
        public List<Expense> getgeneral()
        {
            List<Expense> cp = new List<Expense>(); // data to be returned
            try
            {
                string connectionstring = configuration.GetConnectionString("DefaultConnectionString");
                using (SqlConnection connection = new SqlConnection(connectionstring))
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandType = System.Data.CommandType.Text;
                    cmd.CommandText = @"select generalid,entrydate,project,particular,debited,credited,reference,narration from Generalentry";
                    connection.Open();
                    cmd.Connection = connection;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            cp.Add(new Expense
                            {
                                entrydate = reader["entrydate"] != DBNull.Value ? DateOnly.FromDateTime((DateTime)reader["entrydate"]) : DateOnly.MinValue,
                                project = reader["project"] != DBNull.Value ? (string)reader["project"] : string.Empty,
                                particular = reader["particular"] != DBNull.Value ? (string)reader["particular"] : string.Empty,
                                debit = reader["debited"] != DBNull.Value ? (string)reader["debited"] : string.Empty,
                                credit = reader["credited"] != DBNull.Value ? (string)reader["credited"] : string.Empty,
                                reference = reader["reference"] != DBNull.Value ? (string)reader["reference"] : string.Empty,
                                narration = reader["narration"] != DBNull.Value ? (string)reader["narration"] : string.Empty,
                                generalid = reader["generalid"] != DBNull.Value ? (int)reader["generalid"] : 0,
                            });
                        }
                    }
                    connection.Close();

                }
                return cp;
            }
            catch (Exception)
            {
                throw;
            }


        }


    }
}
