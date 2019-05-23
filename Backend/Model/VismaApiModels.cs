using NAS;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web;

namespace NAS.Model
{
    /// <summary>
    /// This class is the model for the response from Visma when requesting data from the customers endpoint in their eAccounting API.
    /// </summary>
    public class CustomerResponse
    {
        /// <summary>
        /// Store for the Meta property, which contains the meta information of the returned data from the request. See <see cref="Meta"/> for more details.
        /// </summary>
        public Meta Meta { get; set; }

        /// <summary>
        /// Store for the Data property, which contains the array of customers returned from the request to Visma.
        /// </summary>
        public List<DataCustomer> Data { get; set; }
    }

    /// <summary>
    /// This class is the model for the response from Visma when requesting data from the invoices endpoint in their eAccounting API.
    /// </summary>
    public class InvoiceResponse
    {
        /// <summary>
        /// Store for the Meta property, which contains the meta information of the returned data from the request. See <see cref="Meta"/> for more details.
        /// </summary>
        public Meta Meta { get; set; }

        /// <summary>
        /// Store for the Data property, which contains the array of invoices returned from the request to Visma.
        /// </summary>
        public List<DataInvoice> Data { get; set; }
    }

    /// <summary>
    /// This class is the model for the meta information from Visma when requesting data from the invoices or customers endpoint in their eAccounting API.
    /// </summary>
    public class Meta
    {
        /// <summary>
        /// Store for the TotalNumberOfResults property, which contains the number of invoices or customers in the response from Visma.
        /// </summary>
        public int TotalNumberOfResults { get; set; }

        /// <summary>
        /// Store for the ServerTimeUtc property, which contains the local time of the server when the request is made.
        /// </summary>
        public DateTime ServerTimeUtc { get; set; }
    }

    /// <summary>
    /// This class is the model for the customer labels each customer has in Visma eAccounting. When the membership base is administered
    /// this property is used to mark what kind of membership the member has.
    /// </summary>
    public class CustomerLabels
    {
        /// <summary>
        /// Store for the Name property, which contains the name of the customer label and therefore type of membership in this context.
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// This class is the model for the data of each customer returned from Visma eAccounting.
    /// </summary>
    public class DataCustomer
    {
        /// <summary>
        /// Store for the Id property, which contains the uniq customer Id each user has in Visma eAccounting.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Store for the EmailAddress property, which contains the email the user has in Visma eAccounting.
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// Store for the Name property, which contains the full name the user has in Visma eAccounting.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Store for the CustomerLabels property, which contains the array of customer labels a user has in Visma eAccounting. See <see cref="CustomerLabels"/> for more details.
        /// </summary>
        public List<CustomerLabels> CustomerLabels { get; set; }
    }

    /// <summary>
    /// This class is the model for the data of each invoices returned from Visma eAccounting.
    /// </summary>
    public class DataInvoice
    {
        /// <summary>
        /// Store for the CustomerId property, which contains the uniq customer Id of the user the invoice was sent to.
        /// </summary>
        public Guid CustomerId { get; set; }

        /// <summary>
        /// Store for the DueDate property, which contains the date when the invoice is due for payment.
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Store for the RemainingAmount property, which contains the amount remaining to be paid on the invoice.
        /// </summary>
        public decimal? RemainingAmount { get; set; }
    }
}
