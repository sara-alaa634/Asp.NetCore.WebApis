using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Services.Exceptions
{
    public abstract class NotFoundException(string message):Exception(message) // primary CTOR
    {
        //C# 12 
        // Primary CTOR

    }



    public class ProductNotFoundException(int id) :NotFoundException($"Product with Id {id} not found.")
    {

    }
   

    public class BasketNotFoundException(string id) : NotFoundException($"Basket with Id {id} not found.")
    {

    }

}
