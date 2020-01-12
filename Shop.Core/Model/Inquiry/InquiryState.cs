using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Model
{
    public enum InquiryState
    {
        AddingToCart = 0,
        Placed = 1,
        PaymentExpected = 2,
        PaymentReceived = 3,
        Dispatched = 4,
    }
}

// Go CreditCard Page
// Enter CreditCard Info
// Confirm -> "We have received intentions of payment"
// ----> Confirmation from Clearance Center of Payment Complete
// -------> Transition to PaymentReceived