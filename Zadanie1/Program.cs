using System;
using System.Data.SqlClient;

namespace zad1
{

    class Program
    {
        static void Main(string[] args)
        {

            DBconect k1 = new DBconect();

             k1.display();
             k1.insert();
             k1.update();
             k1.delete();
             k1.closeconn();

        }
    }
}