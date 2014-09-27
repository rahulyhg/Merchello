﻿namespace Merchello.Plugin.Payments.Braintree.Api
{
    using global::Braintree;

    using Merchello.Core;
    using Merchello.Core.Gateways.Payment;
    using Merchello.Core.Models;
    using Merchello.Plugin.Payments.Braintree.Models;

    using Umbraco.Core;

    /// <summary>
    /// Represents the <see cref="BraintreeTransactionApiProvider"/>.
    /// </summary>
    internal class BraintreeTransactionApiProvider : BraintreeApiProviderBase, IBraintreeTransactionApiProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BraintreeTransactionApiProvider"/> class.
        /// </summary>
        /// <param name="settings">
        /// The settings.
        /// </param>
        public BraintreeTransactionApiProvider(BraintreeProviderSettings settings)
            : this(Core.MerchelloContext.Current, settings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BraintreeTransactionApiProvider"/> class.
        /// </summary>
        /// <param name="merchelloContext">
        /// The merchello context.
        /// </param>
        /// <param name="settings">
        /// The settings.
        /// </param>
        internal BraintreeTransactionApiProvider(IMerchelloContext merchelloContext, BraintreeProviderSettings settings)
            : base(merchelloContext, settings)
        {
        }

        /// <summary>
        /// Performs a Braintree sales transaction.
        /// </summary>
        /// <param name="invoice">
        /// The invoice.
        /// </param>
        /// <param name="paymentMethodNonce">
        /// The payment method nonce.
        /// </param>
        /// <param name="customer">
        /// The customer.
        /// </param>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <returns>
        /// The <see cref="Result{Transaction}"/>.
        /// </returns>
        public Result<Transaction> Sale(IInvoice invoice, string paymentMethodNonce = "", ICustomer customer = null, TransactionOption option = TransactionOption.SubmitForSettlement)
        {
            return Sale(invoice, paymentMethodNonce, customer, null, option);
        }

        /// <summary>
        /// Performs a Braintree sales transaction.
        /// </summary>
        /// <param name="invoice">
        /// The invoice.
        /// </param>
        /// <param name="paymentMethodNonce">
        /// The payment method nonce.
        /// </param>
        /// <param name="customer">
        /// The customer.
        /// </param>
        /// <param name="billingAddress">
        /// The billing address.
        /// </param>
        /// <param name="option">
        /// The transaction option.
        /// </param>
        /// <returns>
        /// The <see cref="Result{Transaction}"/>.
        /// </returns>
        public Result<Transaction> Sale(IInvoice invoice, string paymentMethodNonce, ICustomer customer, IAddress billingAddress, TransactionOption option = TransactionOption.SubmitForSettlement)
        {
            return Sale(invoice, paymentMethodNonce, customer, billingAddress, null, option);
        }

        /// <summary>
        /// Performs a Braintree sales transaction.
        /// </summary>
        /// <param name="invoice">
        /// The invoice.
        /// </param>
        /// <param name="paymentMethodNonce">
        /// The payment method nonce.
        /// </param>
        /// <param name="customer">
        /// The customer.
        /// </param>
        /// <param name="billingAddress">
        /// The billing address.
        /// </param>
        /// <param name="shippingAddress">
        /// The shipping address.
        /// </param>
        /// <param name="option">
        /// The option.
        /// </param>
        /// <returns>
        /// The <see cref="IPaymentResult"/>.
        /// </returns>
        public Result<Transaction> Sale(IInvoice invoice, string paymentMethodNonce, ICustomer customer, IAddress billingAddress, IAddress shippingAddress, TransactionOption option = TransactionOption.SubmitForSettlement)
        {
            var request = RequestFactory.CreateTransactionRequest(invoice, paymentMethodNonce, customer, option);

            if (billingAddress != null) request.BillingAddress = RequestFactory.CreateAddressRequest(billingAddress);
            if (shippingAddress != null) request.ShippingAddress = RequestFactory.CreateAddressRequest(shippingAddress);

            return BraintreeGateway.Transaction.Sale(request);
        }

        /// <summary>
        /// Performs a total refund.
        /// </summary>
        /// <param name="transactionId">
        /// The transaction id.
        /// </param>
        /// <returns>
        /// The <see cref="Result{Transaction}"/>.
        /// </returns>
        public Result<Transaction> Refund(string transactionId)
        {
            return BraintreeGateway.Transaction.Refund(transactionId);
        }

        /// <summary>
        /// Performs a partial refund.
        /// </summary>
        /// <param name="transactionId">
        /// The transaction id.
        /// </param>
        /// <param name="amount">
        /// The amount.
        /// </param>
        /// <returns>
        /// The <see cref="Result{Transaction}"/>.
        /// </returns>
        public Result<Transaction> Refund(string transactionId, decimal amount)
        {
            return BraintreeGateway.Transaction.Refund(transactionId, amount);
        }
    }
}