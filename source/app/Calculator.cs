using System;
using System.Data;

namespace app
{
  public class Calculator
  {
      readonly IDbConnection connection;

      public Calculator(IDbConnection connection)
      {
          this.connection = connection;
      }
    public int add(int i, int i1)
    {
      if(i < 0 || i1 < 0)
          throw new ArgumentException("Negatives aren't allowed");
      connection.Open();
      return i + i1;
    }
  }
}