using System.ComponentModel;

namespace FAR.Models
{
    public enum Role
        {
            [Description("Покупатель")]
            Buyer,
            [Description("Продавец")]
            Seller
        }
}
