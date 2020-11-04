using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStoreTutorial.Models.DataTransferObjects
{
    /// <summary>
    /// Store CarItem data to persistent cookie
    /// </summary>
    public class CartItemDTO
    {
        public int BookId { get; set; }
        public int Quantity { get; set; }
    }
}
