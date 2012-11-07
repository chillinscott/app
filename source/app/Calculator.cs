using System;
using System.Data;
using System.Security;
using System.Threading;

namespace app
{
    public class Calculator
    {
        IDbConnection connection;

        public Calculator(IDbConnection connection)
        {
            this.connection = connection;
        }

        public int add(int i, int i1)
        {
            if (i < 0 || i1 < 0)
                throw new ArgumentException("Negatives aren't allowed");

            using (connection)
            using (var command = connection.CreateCommand())
            {
                connection.Open();
                command.ExecuteNonQuery();
            }

            return i + i1;
        }

        public void shut_off()
        {
            if (!Thread.CurrentPrincipal.IsInRole("User"))
            {
                throw new SecurityException("Unauthorized");
            }
        }
    }
}