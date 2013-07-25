using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace PasswordhashExample
{
    class Program
    {
        static void Main(string[] args)
        {
            Program p = new Program();
            //call the register
            p.Register();

        }

        private int CreatePassKey()
        {
            //generate a random number for
            //a passkey
            Random rand = new Random();
            return rand.Next(1000000, 99999999);
        }

        private void Register()
        {
            //get the user info
            Console.WriteLine("Enter your user name");
            string username = Console.ReadLine();
            Console.WriteLine("Enter your password");
            string pass = Console.ReadLine();
            
            //create passkey
            int passkey = CreatePassKey();

            //initialize the PasswordHash class
            PasswordHash ph = new PasswordHash();
            //convert the password and passkey to a hash
            Byte[] hashedPassword = ph.HashIt(pass, passkey.ToString());

            //create the connection to sql server
            SqlConnection connect = new SqlConnection("Data Source=localhost;initial catalog=TestPassword;integrated security=true");

            //create sql string
            string sql = "Insert into loginTable(username, passkey, userPassword) values (@userName, @passkey, @userPassword)";

            //create the command
            SqlCommand cmd = new SqlCommand(sql, connect);

            //add a parameter for each the sql variables
            cmd.Parameters.AddWithValue("@userName", username);
            cmd.Parameters.AddWithValue("@passkey", passkey);
            cmd.Parameters.AddWithValue("@userPassword", hashedPassword);
            //open the connection
            connect.Open();
            //execute the query
            cmd.ExecuteNonQuery();
            //close the connection
            connect.Close();
        }
    }
}
